using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
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

            // Assert
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("two", result.FirstOrDefault(x => x.Id == 2)?.Description);
        }

        [Fact]
        public void GetAllProductsReturnsNullWhenRepositoryMockSetToNull()
        {
            // Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns((List<Product>)null);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetAllProducts();

            // Assert
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
            Assert.Null(result);
        }

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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockAllProducts);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetProductById(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockAllProducts);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetProductById(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockAllProducts);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetProductById(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockAllProducts);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetProductById(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
            Assert.Null(result);
        }

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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetProduct(testId))
                .Returns(Task.FromResult(mockAllProducts.First(y => y.Id == testId)));

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = await productService.GetProduct(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetProduct(It.IsAny<int>()), Times.Once);
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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetProduct(testId))
                .Returns(Task.FromResult(mockAllProducts.SingleOrDefault(y => y.Id == testId)));

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = await productService.GetProduct(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetProduct(It.IsAny<int>()), Times.Once);
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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetProduct(testId))
                .Returns(Task.FromResult(mockAllProducts.SingleOrDefault(y => y.Id == testId)));

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = await productService.GetProduct(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetProduct(It.IsAny<int>()), Times.Once);
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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetProduct(testId))
                .Returns(Task.FromResult(mockAllProducts.SingleOrDefault(y => y.Id == testId)));

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = await productService.GetProduct(testId);

            // Assert
            mockProductRepository.Verify(x => x.GetProduct(It.IsAny<int>()), Times.Once);
            Assert.Null(result);
        }

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

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetProduct())
                .Returns(Task.FromResult(mockAllProducts));

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = await productService.GetProduct();

            // Assert
            mockProductRepository.Verify(x => x.GetProduct(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetProductSynchronousReturnsEmptyListWithProductsNotPopulated()
        {
            // Arrange 
            IList<Product> mockAllProducts = new List<Product>();

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.GetProduct())
                .Returns(Task.FromResult(mockAllProducts));

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = await productService.GetProduct();

            // Assert
            mockProductRepository.Verify(x => x.GetProduct(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

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
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
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
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.NotNull(resultSpecific);
            Assert.Equal("2", resultSpecific.Stock);
            Assert.Null(resultSpecific.Description);
        }

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
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
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
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
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
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
            Assert.Null(result);
        }

        [Fact]
        public void SaveProductCorrectlySavesProductToDb()
        {
            // Arrange
            var productsMockDb = new List<Product>();

            var testProductViewModel = new ProductViewModel
            {
                Id = 1,
                Stock = "1",
                Price = "10",
                Name = "Test Product",
                Description = "test description",
                Details = "test details"
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            mockProductRepository.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Once);
            Assert.Single(productsMockDb);
            Assert.Contains(productsMockDb, p => p.Name == "Test Product");
        }

        [Fact]
        public void SaveProductThrowsGivenNullArg()
        {
            // Arrange
            var productsMockDb = new List<Product>();

            ProductViewModel testProductViewModel = null;

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            Action testAction = () => productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
            mockProductRepository.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Never);
        }

        [Theory]
        [InlineData(null, "10", "one name", "one description", "one details")]
        [InlineData("2", null, "two name", "two description", "two details")]
        public void SaveProductThrowsGivenNullStockPriceFields(string stock, string price, string name,
            string description, string details)
        {
            // Arrange
            var productsMockDb = new List<Product>();

            var testProductViewModel = new ProductViewModel
            {
                Stock = stock,
                Price = price,
                Name = name,
                Description = description,
                Details = details
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            Action testAction = () => productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Throws<ArgumentNullException>(testAction);
            mockProductRepository.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void SaveProductToleratesNullNameField()
        {
            // Arrange
            var productsMockDb = new List<Product>();

            var testProductViewModel = new ProductViewModel
            {
                Stock = "1",
                Price = "1",
                Name = null,
                Description = "description",
                Details = "details"
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            mockProductRepository.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Once);
            Assert.Null(productsMockDb.First().Name);
        }

        [Fact]
        public void SaveProductToleratesNullDescriptionField()
        {
            // Arrange
            var productsMockDb = new List<Product>();

            var testProductViewModel = new ProductViewModel
            {
                Stock = "1",
                Price = "1",
                Name = "name",
                Description = null,
                Details = "details"
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            mockProductRepository.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Once);
            Assert.Null(productsMockDb.First().Description);
        }

        [Fact]
        public void SaveProductToleratesNullDetailsField()
        {
            // Arrange
            var productsMockDb = new List<Product>();

            var testProductViewModel = new ProductViewModel
            {
                Stock = "1",
                Price = "1",
                Name = "name",
                Description = "description",
                Details = null
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            mockProductRepository.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Once);
            Assert.Null(productsMockDb.First().Details);
        }

        [Fact]
        public void UpdateProductStocksDecrementsProductQuantityGivenPositiveQtyToRemove()
        {
            // Arrange
            var mockCart = new Cart();
            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            foreach (var p in mockDb)
            {
                mockCart.AddItem(p, 1);
            }

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        mockDb.Remove(product);
                });

            var _productService = new ProductService(mockCart, mockProductRepository.Object, null, null);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            mockProductRepository.Verify(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            Assert.Equal(3, mockDb.Count);
            Assert.Equal(1, mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(2, mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(3, mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductStocksRemovesProductFromDbGivenQuantityZero()
        {
            // Arrange
            var cart = new Cart();
            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            foreach (var p in mockDb)
            {
                cart.AddItem(p, 2);
            }

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        mockDb.Remove(product);
                });

            var productService = new ProductService(cart, mockProductRepository.Object, null, null);

            // Act
            productService.UpdateProductQuantities();

            // Assert
            mockProductRepository.Verify(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            Assert.Equal(2, mockDb.Count);
            Assert.Equal(1, mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(2, mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductStocksSetsNegativeProductQtyGivenTooLargeQtyToRemove()
        {
            // Arrange
            var cart = new Cart();
            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            foreach (var p in mockDb)
            {
                cart.AddItem(p, 3);
            }

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        mockDb.Remove(product);
                });

            var productService = new ProductService(cart, mockProductRepository.Object, null, null);

            // Act
            productService.UpdateProductQuantities();

            // Assert
            mockProductRepository.Verify(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            Assert.Equal(2, mockDb.Count);
            Assert.Equal(-1, mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(1, mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductQuantityIncrementsQuantityGivenNegativeArg()
        {
            // Arrange
            var cart = new Cart();

            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            foreach (var p in mockDb)
            {
                cart.AddItem(p, -1);
            }

            var mockProductRepository = new Mock<IProductRepository>();

            mockProductRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        mockDb.Remove(product);
                });

            var productService = new ProductService(cart, mockProductRepository.Object, null, null);

            // Act
            productService.UpdateProductQuantities();

            // Assert
            mockProductRepository.Verify(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            Assert.Equal(3, mockDb.Count);
            Assert.Equal(3, mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(4, mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(5, mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductQuantityDoesNothingGivenEmptyCart()
        {
            // Arrange
            var cart = new Cart();

            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            var mockProductRepository = new Mock<IProductRepository>();

            mockProductRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        mockDb.Remove(product);
                });

            var productService = new ProductService(cart, mockProductRepository.Object, null, null);

            // Act
            productService.UpdateProductQuantities();

            // Assert
            mockProductRepository.Verify(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()), Times.Never  );
            Assert.Equal(3, mockDb.Count);
            Assert.Equal(2, mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(3, mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(4, mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductQuantityThrowsGivenEmptyCart()
        {
            // Arrange
            Cart mockCart = null;

            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        mockDb.Remove(product);
                });

            var productService = new ProductService(mockCart, mockProductRepository.Object, null, null);

            // Act
            Action testAction = () => productService.UpdateProductQuantities();

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
            mockProductRepository.Verify(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteProductRemovesCartLineAndDeletesProductFromDb()
        {
            // Arrange
            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            var cartLinesList = new List<CartLine>();

            var mockCart = new Mock<ICart>();
            mockCart
                .Setup(x => x.AddItem(It.IsAny<Product>(), It.IsAny<int>()))
                .Callback((Product product, int quantity) => {
                    CartLine line = cartLinesList.FirstOrDefault(p => p.Product.Id == product.Id);

                    if (line == null)
                    {
                        cartLinesList.Add(new CartLine { Product = product, Quantity = quantity });
                    }
                    else
                    {
                        line.Quantity += quantity;
                    }
                });

            mockCart
                .Setup(x => x.RemoveLine(It.IsAny<Product>()))
                .Callback((Product product) => cartLinesList.RemoveAll(l => l.Product.Id == product.Id));

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.DeleteProduct(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    if (product != null)
                    {
                        mockDb.Remove(product);
                    }
                });

            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockDb.Where(p => p.Id > 0).ToList());

            var productService = new ProductService(mockCart.Object, mockProductRepository.Object, null, null);

            foreach (var p in mockDb)
            {
                mockCart.Object.AddItem(p, 1);
            }

            // Act
            productService.DeleteProduct(1);

            // Assert
            mockCart.Verify(x => x.AddItem(It.IsAny<Product>(), It.IsAny<int>()), Times.Exactly(3));
            mockCart.Verify(x => x.RemoveLine(It.IsAny<Product>()), Times.Once);
            Assert.Equal(2, mockDb.Count);
            Assert.Equal(2, cartLinesList.Count);
        }

        [Fact]
        public void DeleteProductThrowsGivenInvalidId()
        {
            // Arrange
            var cart = new Cart();

            var mockDb = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Quantity = 2,
                    Price = 1,
                    Name = "One Name",
                    Description = "one description",
                    Details = "one details"
                },
                new Product
                {
                    Id = 2,
                    Quantity = 3,
                    Price = 2,
                    Name = "Two Name",
                    Description = "two description",
                    Details = "two details"
                },
                new Product
                {
                    Id = 3,
                    Quantity = 4,
                    Price = 3,
                    Name = "Three Name",
                    Description = "three description",
                    Details = "three details"
                },
            };

            foreach (var p in mockDb)
            {
                cart.AddItem(p, 1);
            }

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(x => x.DeleteProduct(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    Product product = mockDb.FirstOrDefault(p => p.Id == id);
                    if (product != null)
                    {
                        mockDb.Remove(product);
                    }
                });

            mockProductRepository
                .Setup(x => x.GetAllProducts())
                .Returns(mockDb.Where(p => p.Id > 0).ToList());

            var productService = new ProductService(cart, mockProductRepository.Object, null, null);

            // Act
            Action testAction = () => productService.DeleteProduct(-1);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
            mockProductRepository.Verify(x => x.DeleteProduct(It.IsAny<int>()), Times.Never);
            mockProductRepository.Verify(x => x.GetAllProducts(), Times.Once);
        }
    }
}
