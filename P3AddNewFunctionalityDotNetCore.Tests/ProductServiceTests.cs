using Microsoft.EntityFrameworkCore;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System;
using Microsoft.Extensions.Localization;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        public class TheExampleTestMethods
        {
            private P3Referential _context;
            public TheExampleTestMethods()
            {
                var options = new DbContextOptionsBuilder<P3Referential>().
                           UseSqlServer("Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                           .Options;

                _context = new P3Referential(options);
            }

            /// <summary>
            /// Take this test method as a template to write your test method.
            /// A test method must check if a definite method does its job:
            /// returns an expected value from a particular set of parameters
            /// </summary>
            [Fact]
            public async Task TestGetProductByIdAsyncMockTest()
            {
                // Arrange
                //      public int Id { get; set; }
                //public string Description { get; set; }
                //public string Details { get; set; }
                //public string Name { get; set; }
                //public double Price { get; set; }
                //public int Quantity { get; set; }

                //public virtual ICollection<OrderLine> OrderLine { get; set; }

                Product product = new Product
                {
                    Id = 1,
                    Description = "one",
                    Details = "onedetails",
                };

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository.Setup(x => x.GetProduct(1)).Returns(Task.FromResult(product));

                // Act
                var productService = new ProductService(null, mockProductRepository.Object, null, null);
                var result = await productService.GetProduct(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("one", result.Description);
            }

            [Theory]
            [InlineData(1, "one", "oneDetails")]
            [InlineData(2, "two", "twoDetails")]
            [InlineData(3, "three", "threeDetails")]
            public async Task TestGetProductByIdAsyncMockTestParameterizedTestData(int id, string description, string details)
            {
                // Arrange
                Product product = new Product
                {
                    Id = id,
                    Description = description,
                    Details = details,
                };

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository.Setup(x => x.GetProduct(id)).Returns(Task.FromResult(product));

                // Act
                var productService = new ProductService(null, mockProductRepository.Object, null, null);
                var result = await productService.GetProduct(id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(description, result.Description);
            }

            [Fact]
            public async Task TestGetProductByIdIntegrationTestAsync()
            {
                //Arrange
                ProductRepository productRepository = new ProductRepository(_context);

                // Act
                var result = await productRepository.GetProduct(5);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Cell Phone", result.Description);
            }
        }

        public class TheGetAllProductsMethd
        {
            [Fact]
            public void ReturnsAListOfAllProductsMock()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                List<Product> result = productService.GetAllProducts();
                Product resultTwo = result.FirstOrDefault(x => x.Id == 2);

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(resultTwo);
                Assert.Equal(3, result.Count);
                Assert.Equal("two", resultTwo.Description);
            }

            [Fact]
            public void ReturnsNullWhenRepositoryMockSetToNull()
            {
                // Arrange
                List<Product> mockAllProductsNull = null;

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProductsNull);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                List<Product> result = productService.GetAllProducts();

                // Assert
                Assert.Null(result);
            }
        }

        public class TheGetProductByIdMethod
        {
            [Fact]
            public void ReturnsCorrectProductWithGoodId()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = 3;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = productService.GetProductById(testId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("three", result.Description);
            }

            [Fact]
            public void ReturnsNUllWhenPassedBadIdNegative()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = -1;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = productService.GetProductById(testId);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void ReturnsNUllWhenPassedBadIdZero()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = 0;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = productService.GetProductById(testId);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void ReturnsNUllWhenPassedBadIdPositive()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = 10;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = productService.GetProductById(testId);

                // Assert
                Assert.Null(result);
            }
        }

        public class TheGetProductMethod
        {
            [Fact]
            public async Task ReturnsCorrectProductGivenValidId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = 1;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetProduct(testId))
                    .Returns(Task.FromResult(mockAllProducts.First(y => y.Id == testId)));

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = await productService.GetProduct(testId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("one", result.Description);
            }

            [Fact]
            public async Task ReturnsNullWhenPassedNegativeId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = -1;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetProduct(testId))
                    .Returns(Task.FromResult(mockAllProducts.SingleOrDefault(y => y.Id == testId)));

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = await productService.GetProduct(testId);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task ReturnsNullWhenPassedBadId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = 666;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetProduct(testId))
                    .Returns(Task.FromResult(mockAllProducts.SingleOrDefault(y => y.Id == testId)));

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = await productService.GetProduct(testId);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task ReturnsNullWhenPassedZeroId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                int testId = 0;

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetProduct(testId))
                    .Returns(Task.FromResult(mockAllProducts.SingleOrDefault(y => y.Id == testId)));

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = await productService.GetProduct(testId);

                // Assert
                Assert.Null(result);
            }
        }

        public class TheGetProductMethodReturnListOverload
        {
            [Fact]
            public async Task ReturnsCorrectListOfAllProductsPopulated()
            {
                // Arrange 
                IList<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Description = "one",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Description = "two",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Description = "three",
                        Details = "three_details"
                    },
                };

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetProduct())
                    .Returns(Task.FromResult(mockAllProducts));

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = await productService.GetProduct();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
            }

            [Fact]
            public async Task ReturnsEmptyListWithProductsNotPopulated()
            {
                // Arrange 
                IList<Product> mockAllProducts = new List<Product>();

                var mockRepository = new Mock<IProductRepository>();
                mockRepository
                    .Setup(x => x.GetProduct())
                    .Returns(Task.FromResult(mockAllProducts));

                var productService = new ProductService(null, mockRepository.Object, null, null);

                // Act
                var result = await productService.GetProduct();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(0, result.Count);
            }
        }

        public class TheGetAllProductsViewModelMethod
        {
            [Fact]
            public void ReturnsAListOfCorrectlyMappedViewModelsGivenGoodInput()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Quantity = 1,
                        Price = 10D,
                        Name = "One",
                        Description = "one_description",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Quantity = 2,
                        Price = 20D,
                        Name = "Two",
                        Description = "two_description",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Quantity = 3,
                        Price = 30D,
                        Name = "Three",
                        Description = "three_description",
                        Details = "three_details"
                    },
                };

                int testId = 2;

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                List<ProductViewModel> result = productService.GetAllProductsViewModel();
                ProductViewModel resultSpecific = result.FirstOrDefault(x => x.Id == testId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);

                Assert.NotNull(resultSpecific);
                Assert.Equal(testId, resultSpecific.Id);
                Assert.Equal("2", resultSpecific.Stock);
                Assert.Equal("20", resultSpecific.Price);
                Assert.Equal("Two", resultSpecific.Name);
                Assert.Equal("two_description", resultSpecific.Description);
                Assert.Equal("two_details", resultSpecific.Details);
            }

            [Fact]
            public void ReturnsListOfPartialProductViewModelsGivenPartialProducts()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Quantity = 1,
                        Price = 10D,
                        Name = null,
                        Description = "one_description",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Quantity = 2,
                        Price = 20D,
                        Name = "Two",
                        Description = null,
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Quantity = 3,
                        Price = 30D,
                        Name = "Three",
                        Description = "three_description",
                        Details = null
                    },
                };

                int testId = 2;

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                List<ProductViewModel> result = productService.GetAllProductsViewModel();
                ProductViewModel resultSpecific = result.FirstOrDefault(x => x.Id == testId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);

                Assert.NotNull(resultSpecific);
                Assert.Equal("2", resultSpecific.Stock);
                Assert.Null(resultSpecific.Description);
            }
        }

        public class TheGetProductViewModelByIdMethod
        {
            [Fact]
            public void ReturnsCorrectViewModelGivenGoodId()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Quantity = 1,
                        Price = 10D,
                        Name = "One",
                        Description = "one_description",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Quantity = 2,
                        Price = 20D,
                        Name = "Two",
                        Description = "two_description",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Quantity = 3,
                        Price = 30D,
                        Name = "Three",
                        Description = "three_description",
                        Details = "three_details"
                    },
                };

                int testId = 2;

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                ProductViewModel result = productService.GetProductByIdViewModel(testId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(testId, result.Id);
            }

            [Fact]
            public void ReturnsNullGivenInvalidId()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Quantity = 1,
                        Price = 10D,
                        Name = "One",
                        Description = "one_description",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Quantity = 2,
                        Price = 20D,
                        Name = "Two",
                        Description = "two_description",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Quantity = 3,
                        Price = 30D,
                        Name = "Three",
                        Description = "three_description",
                        Details = "three_details"
                    },
                };

                int testId = 10;

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                ProductViewModel result = productService.GetProductByIdViewModel(testId);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void ReturnsNullGivenInvalidIdNegative()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Quantity = 1,
                        Price = 10D,
                        Name = "One",
                        Description = "one_description",
                        Details = "one_details"
                    },
                    new Product
                    {
                        Id = 2,
                        Quantity = 2,
                        Price = 20D,
                        Name = "Two",
                        Description = "two_description",
                        Details = "two_details"
                    },
                    new Product
                    {
                        Id = 3,
                        Quantity = 3,
                        Price = 30D,
                        Name = "Three",
                        Description = "three_description",
                        Details = "three_details"
                    },
                };

                int testId = -10;

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProducts);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                ProductViewModel result = productService.GetProductByIdViewModel(testId);

                // Assert
                Assert.Null(result);
            }
        }

        public class TheCheckProductModelErrorsMethod
        {
            private IProductService _productService;

            public TheCheckProductModelErrorsMethod()
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
}