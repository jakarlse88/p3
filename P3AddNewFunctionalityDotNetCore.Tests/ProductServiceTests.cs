using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        #region GetAllProducts

        [Fact]
        public void GetAllProductsReturnsListOfAllProducts()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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
            var result = productService.GetAllProducts();
            var resultTwo = result.FirstOrDefault(x => x.Id == 2);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(resultTwo);
            Assert.Equal(3, result.Count);
            Assert.Equal("two", resultTwo.Description);
        }

        [Fact]
        public void GetAllProductsReturnsNullWhenRepositoryMockSetToNull()
        {
            // Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns((List<Product>) null);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetAllProducts();

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetProductById

        [Fact]
        public void GetProductByIdReturnsCorrectProductWithGoodId()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = 3;

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
        public void GetProductByIdReturnsNUllWhenPassedBadIdNegative()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = -1;

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
        public void GetProductByIdReturnsNUllWhenPassedBadIdZero()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = 0;

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
            var mockAllProducts = new List<Product>()
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

            const int testId = 10;

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

        #endregion

        #region GetProductMethod

        [Fact]
        public async Task GetProductMethodReturnsCorrectProductGivenValidId()
        {
            // Arrange 
            var mockAllProducts = new List<Product>()
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

            const int testId = 1;

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
        public async Task GetProductMethodReturnsNullWhenPassedNegativeId()
        {
            // Arrange 
            var mockAllProducts = new List<Product>()
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

            const int testId = -1;

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
        public async Task GetProductMethodReturnsNullWhenPassedBadId()
        {
            // Arrange 
            var mockAllProducts = new List<Product>()
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

            const int testId = 666;

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
        public async Task GetProductMethodReturnsNullWhenPassedZeroId()
        {
            // Arrange 
            var mockAllProducts = new List<Product>()
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

            const int testId = 0;

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

        #endregion

        #region GetProductSynchronous

        [Fact]
        public async Task GetProductSynchronousReturnsCorrectListOfAllProductsPopulated()
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
        public async Task GetProductSynchronousReturnsEmptyListWithProductsNotPopulated()
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

        #endregion

        #region GetAllProductsViewModel

        [Fact]
        public void GetAllProductsViewModelReturnsAListOfCorrectlyMappedViewModelsGivenGoodInput()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = 2;

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
        public void GetAllProductsViewModelReturnsListOfPartialProductViewModelsGivenPartialProducts()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = 2;

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
        #endregion

        #region GetProductViewModelById

        [Fact]
        public void GetProductViewModelByIdReturnsCorrectViewModelGivenGoodId()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = 2;

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockAllProducts);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetProductByIdViewModel(testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testId, result.Id);
        }

        [Fact]
        public void GetProductViewModelByIdReturnsNullGivenInvalidId()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = 10;

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockAllProducts);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetProductByIdViewModel(testId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetProductViewModelByIdReturnsNullGivenInvalidIdNegative()
        {
            // Arrange
            var mockAllProducts = new List<Product>()
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

            const int testId = -10;

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockAllProducts);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetProductByIdViewModel(testId);

            // Assert
            Assert.Null(result);
        }
        #endregion
    }
}
