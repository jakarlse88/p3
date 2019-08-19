using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using Xunit;

// @TODO
// Numeric:
//      Double.MaxValue(), Double.MaxValue()+1, whitespace end/beginning, string with garbage + legal values
// String:
//      Quotes, illegal chars, blank, enforce arbitrary char limit in validation method

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CheckProductModelErrorsTests
    {
        private IProductService _productService;

        public CheckProductModelErrorsTests()
        {
            // Localizer mock:
            // https://stackoverflow.com/a/43461506
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            string missingNameKey = "Please enter a name";
            var localizedMissingName = new LocalizedString(missingNameKey, missingNameKey);
            mockLocalizer.Setup(x => x["MissingName"]).Returns(localizedMissingName);

            string missingPriceKey = "Please enter a price";
            var localizedMissingPrice = new LocalizedString(missingPriceKey, missingPriceKey);
            mockLocalizer.Setup(x => x["MissingPrice"]).Returns(localizedMissingPrice);

            string missingStockKey = "Please enter a stock value";
            var localizedMissingStock = new LocalizedString(missingStockKey, missingStockKey);
            mockLocalizer.Setup(x => x["MissingStock"]).Returns(localizedMissingStock);

            string priceNotANumberKey = "The value entered for the price must be a number";
            var localizedPriceNotANumber = new LocalizedString(priceNotANumberKey, priceNotANumberKey);
            mockLocalizer.Setup(x => x["PriceNotANumber"]).Returns(localizedPriceNotANumber);

            string priceNotGreaterThanZeroKey = "The price must be greater than zero";
            var localizedPriceNotGreaterThanZero = new LocalizedString(priceNotGreaterThanZeroKey, priceNotGreaterThanZeroKey);
            mockLocalizer.Setup(x => x["PriceNotGreaterThanZero"]).Returns(localizedPriceNotGreaterThanZero);

            string stockNotAnIntegerKey = "The value entered for the stock must be a integer";
            var localizedStockNotAnInteger = new LocalizedString(stockNotAnIntegerKey, stockNotAnIntegerKey);
            mockLocalizer.Setup(x => x["StockNotAnInteger"]).Returns(localizedStockNotAnInteger);

            string stockNotGreaterThanZeroKey = "The stock must be greater than zero";
            var localizedStockNotGreaterThanZero = new LocalizedString(stockNotGreaterThanZeroKey, stockNotGreaterThanZeroKey);
            mockLocalizer.Setup(x => x["StockNotGreaterThanZero"]).Returns(localizedStockNotGreaterThanZero);

            _productService = new ProductService(null, null, null, mockLocalizer.Object);
        }

        [Fact]
        public void ReturnsCorrectlErrorsGivenEmptyViewModel()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel();

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Contains("Please enter a name", result);
            Assert.Contains("Please enter a price", result);
            Assert.Contains("Please enter a stock value", result);
            Assert.Contains("The value entered for the price must be a number", result);
            Assert.Contains("The value entered for the stock must be a integer", result);
        }

        [Fact]
        public void ReturnsMissingNameErrorGivenEmptyNameString()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = " "
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter a name", result);
        }

        [Fact]
        public void ReturnsMissingPriceWhenGivenEmptyPriceString()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Price = " "
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter a price", result);
        }

        [Fact]
        public void ReturnsPriceNotANumberErrorGivenAlphaPriceString()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Price = "ABC"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The value entered for the price must be a number", result);
        }

        [Fact]
        public void ReturnsPriceNotGreaterThanZeroErrorGivenNegativeNumber()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Price = "-10"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The price must be greater than zero", result);
        }

        [Fact]
        public void ReturnsMissingStockErrorGivenEmptyStockString()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = " "
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter a stock value", result);
        }

        [Fact]
        public void ReturnsStockNotAnIntegerErrorWhenGivenADecimalString()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "35,85719"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The value entered for the stock must be a integer", result);
        }

        [Fact]
        public void ReturnsStockNotGreaterThanZeroErrorGivenNegativeStockString()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "-35"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The stock must be greater than zero", result);
        }

        [Fact]
        public void ReturnsNoErrorsGivenValidObject()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = "Test Product",
                Price = "10",
                Stock = "20",
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Empty(result);
        }
    }

}
