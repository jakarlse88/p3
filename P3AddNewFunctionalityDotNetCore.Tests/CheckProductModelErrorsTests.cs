using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CheckProductModelErrorsTests
    {
        private IProductService _productService;
        private Mock<IStringLocalizer<ProductService>> _mockLocalizer;

        public CheckProductModelErrorsTests()
        {
            _mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            var resourcesDictionary = new Dictionary<string, string>
            {
                { "DescriptionTooLong", "The description cannot be longer than 100 letters" },
                { "DescriptionTooShort", "The description must be longer than ten letters" },
                { "DetailsTooLong", "The details cannot be longer than 200 letters" },
                { "DetailsTooShort", "The details must be longer than ten letters" },
                { "MissingDescription", "Please enter a description" },
                { "MissingDetails", "Please enter details" },
                { "MissingName", "Please enter a name" },
                { "MissingPrice", "Please enter a price" },
                { "MissingStock", "Please enter a stock value" },
                { "NameTooLong", "The name cannot be longer than 100 letters" },
                { "NameTooShort", "The name must be at least four letters long" },
                { "PriceNotANumber", "The value entered for the price must be a number" },
                { "PriceNotGreaterThanZero", "The price must be greater than zero" },
                { "StockNotAnInteger", "The value entered for the stock must be an integer" },
                { "StockNotGreaterThanZero", "The stock must be greater than zero" },
                { "NameIllegalCharacter", "Illegal character in name" },
                { "DetailsIllegalCharacter", "Illegal character in details" },
                { "DescriptionIllegalCharacter", "Illegal character in description" }
            };

            // Localizer mock:
            // https://stackoverflow.com/a/43461506
            foreach (KeyValuePair<string, string> entry in resourcesDictionary)
            {
                _mockLocalizer
                    .Setup(x => x[entry.Key])
                    .Returns(new LocalizedString(entry.Value, entry.Value));
            }

            _productService = new ProductService(null, null, null, _mockLocalizer.Object);
        }

        [Fact]
        public void TestCheckProductModelErrorsEmptyModel()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel();

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Count);
            Assert.Contains("Please enter a name", result);
            Assert.Contains("Please enter details", result);
            Assert.Contains("Please enter a description", result);
            Assert.Contains("Please enter a price", result);
            Assert.Contains("Please enter a stock value", result);
            Assert.Contains("The value entered for the price must be a number", result);
            Assert.Contains("The value entered for the stock must be an integer", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsNameNull()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = null
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter a name", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelNameEmpty()
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
        public void TestCheckProductModelErrorsNameQuotes()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = "\"asd\""
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.DoesNotContain("Illegal character in name", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsNameControlChars()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = "\nasd\t"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Illegal character in name", result);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        public void TestCheckProductModelErrorsNameShort(string testString)
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = testString
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The name must be at least four letters long", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsNameLong()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = "lknaskdaksdmaslkmdkalsmdklasmdklamsdklamsdklmaskldmaklsmdlk1moi3j12oi3n12kenkdkaslndlikasjndklasndkajlhneiouansdjklanheuioaskndklasndasjkdnaskjldnaksjdnalksndjkasndaksdiu12he1kn2e12iueh12ien12jkebn1kj2e 12e"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The name cannot be longer than 100 letters", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsNameValid()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Name = "test product"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);
            
            // Assert
            Assert.DoesNotContain("The name cannot be longer than 100 letters", result);
            Assert.DoesNotContain("The name must be at least four letters long", result);
            Assert.DoesNotContain("Illegal character in name", result);
            Assert.DoesNotContain("Please enter a name", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDetailsNull()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Details = null
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter details", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDetailsEmpty()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Details = " "
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter details", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDetailsQuotes()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Details = "\"asd\""
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.DoesNotContain("Illegal character in details", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDetailsControlChars()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Details = "\nasd\t"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Illegal character in details", result);
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("abcdd")]
        [InlineData("abcefghi")]
        public void TestCheckProductModelErrorsDetailsShort(string testString)
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Details = testString
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The details must be longer than ten letters", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDetailsLong()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Details = "lknaskdaksdmaslkmdkalsmdklasmdklamsdklamsdklmaskldmaklsmdlk1moi3j12oi3n12kenkdkaslndlikasjndklasndkajlhneiouansdjklanheuioaskndklasndasjkdnaskjldnaksjdnalksndjkasndaksdiu12he1kn2e12iueh12ien12jkebn1kj2e 12e"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The details cannot be longer than 200 letters", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDetailsValid()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Details = "test product"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);
            
            // Assert
            Assert.DoesNotContain("The details cannot be longer than 200 letters", result);
            Assert.DoesNotContain("The details must be longer than ten letters", result);
            Assert.DoesNotContain("Illegal character in details", result);
            Assert.DoesNotContain("Please enter details", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDescriptionNull()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Description = null
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter a description", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDescriptionEmpty()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Description = " "
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Please enter a description", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDescriptionQuotes()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Description = "\"asd\""
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.DoesNotContain("Illegal character in description", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDescriptionControlChars()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Description = "\nasd\t"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("Illegal character in description", result);
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("abcdd")]
        [InlineData("abcefghi")]
        public void TestCheckProductModelErrorsDescriptionShort(string testString)
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Description = testString
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The description must be longer than ten letters", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDescriptionLong()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Description = "lknaskdaksdmaslkmdkalsmdklasmdklamsdklamsdklmaskldmaklsmdlk1moi3j12oi3n12kenkdkaslndlikasjndklasndkajlhneiouansdjklanheuioaskndklasndasjkdnaskjldnaksjdnalksndjkasndaksdiu12he1kn2e12iueh12ien12jkebn1kj2e 12e"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The description cannot be longer than 100 letters", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsDescriptionValid()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Description = "test product"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);
            
            // Assert
            Assert.DoesNotContain("The description cannot be longer than 100 letters", result);
            Assert.DoesNotContain("The description must be longer than ten letters", result);
            Assert.DoesNotContain("Illegal character in description", result);
            Assert.DoesNotContain("Please enter a description", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelEmptyPrice()
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
        public void TestCheckProductModelErrorsModelPriceCharacters()
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
        public void TestCheckProductModelErrorsModelPriceCharactersMixed()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Price = "ABC123"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The value entered for the price must be a number", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelPriceCtrlChars()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Price = "\t-35\n"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The price must be greater than zero", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelPriceMixedWhitespace()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Price = "    -35    "
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The price must be greater than zero", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelPriceNegative()
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
        public void TestCheckProductModelErrorsModelPriceValid()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Price = "9123"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.DoesNotContain("The price must be greater than zero", result);
            Assert.DoesNotContain("The value entered for the price must be a number", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelStockEmpty()
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
        public void TestCheckProductModelErrorsModelStocCharacters()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "ABC"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The value entered for the stock must be an integer", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelStocCharactersMixed()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "ABC123"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The value entered for the stock must be an integer", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelStocCtrlChars()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "\t-35\n"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The stock must be greater than zero", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelStocMixedWhitespace()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "    -35    "
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The stock must be greater than zero", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelStocNegative()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "-10"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.Contains("The stock must be greater than zero", result);
        }

        [Fact]
        public void TestCheckProductModelErrorsModelStocValid()
        {
            // Arrange
            ProductViewModel testObject = new ProductViewModel
            {
                Stock = "9123"
            };

            // Act
            List<string> result = _productService.CheckProductModelErrors(testObject);

            // Assert
            Assert.DoesNotContain("The stock must be greater than zero", result);
            Assert.DoesNotContain("The value entered for the stock must be an integer", result);
        }

    }
}
