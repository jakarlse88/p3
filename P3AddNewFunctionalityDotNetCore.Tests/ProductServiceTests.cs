﻿using Moq;
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
                .Returns((List<Product>)null);

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act
            var result = productService.GetAllProducts();

            // Assert
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

            var mockRepository = new Mock<IProductRepository>();
            mockRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Single(productsMockDb);
            Assert.Contains(productsMockDb, p => p.Name == "Test Product");
        }

        [Fact]
        public void SaveProductThrowsGivenNullArg()
        {
            // Arrange
            var productsMockDb = new List<Product>();

            ProductViewModel testProductViewModel = null;

            var mockRepository = new Mock<IProductRepository>();
            mockRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockRepository.Object, null, null);

            // Act
            Action testAction = () => productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
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

            var mockRepository = new Mock<IProductRepository>();
            mockRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockRepository.Object, null, null);

            // Act
            Action testAction = () => productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Throws<ArgumentNullException>(testAction);
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

            var mockRepository = new Mock<IProductRepository>();
            mockRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Collection(productsMockDb, p => p.Name?.Equals(null));
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

            var mockRepository = new Mock<IProductRepository>();
            mockRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Collection(productsMockDb, p => p.Description?.Equals(null));
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

            var mockRepository = new Mock<IProductRepository>();
            mockRepository
                .Setup(x => x.SaveProduct(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    if (product != null) productsMockDb.Add(product);
                });

            var productService = new ProductService(null, mockRepository.Object, null, null);

            // Act
            productService.SaveProduct(testProductViewModel);

            // Assert
            Assert.Collection(productsMockDb, p => p.Details?.Equals(null));
        }

        [Fact]
        public void UpdateProductStocksDecrementsProductQuantityGivenPositiveQtyToRemove()
        {
            // Arrange
            var _mockCart = new Cart();
            var _mockDb = new List<Product>
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

            foreach (var p in _mockDb)
            {
                _mockCart.AddItem(p, 1);
            }

            var _mockRepository = new Mock<IProductRepository>();
            _mockRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        _mockDb.Remove(product);
                });

            var _productService = new ProductService(_mockCart, _mockRepository.Object, null, null);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(3, _mockDb.Count);
            Assert.Equal(1, _mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(2, _mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(3, _mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductStocksRemovesProductFromDbGivenQuantityZero()
        {
            // Arrange
            var _mockCart = new Cart();
            var _mockDb = new List<Product>
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

            foreach (var p in _mockDb)
            {
                _mockCart.AddItem(p, 2);
            }

            var _mockRepository = new Mock<IProductRepository>();
            _mockRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        _mockDb.Remove(product);
                });

            var _productService = new ProductService(_mockCart, _mockRepository.Object, null, null);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(2, _mockDb.Count);
            Assert.Equal(1, _mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(2, _mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductStocksSetsNegativeProductQtyGivenTooLargeQtyToRemove()
        {
            // Arrange
            var _mockCart = new Cart();
            var _mockDb = new List<Product>
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

            foreach (var p in _mockDb)
            {
                _mockCart.AddItem(p, 3);
            }

            var _mockRepository = new Mock<IProductRepository>();
            _mockRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        _mockDb.Remove(product);
                });

            var _productService = new ProductService(_mockCart, _mockRepository.Object, null, null);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(2, _mockDb.Count);
            Assert.Equal(-1, _mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(1, _mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductQuantityIncrementsQuantityGivenNegativeArg()
        {
            // Arrange
            var _mockCart = new Cart();

            var _mockDb = new List<Product>
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

            foreach (var p in _mockDb)
            {
                _mockCart.AddItem(p, -1);
            }

            var _mockRepository = new Mock<IProductRepository>();

            _mockRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        _mockDb.Remove(product);
                });

            var _productService = new ProductService(_mockCart, _mockRepository.Object, null, null);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(3, _mockDb.Count);
            Assert.Equal(3, _mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(4, _mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(5, _mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductQuantityDoesNothingGivenEmptyCart()
        {
            // Arrange
            var _mockCart = new Cart();

            var _mockDb = new List<Product>
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

            var _mockRepository = new Mock<IProductRepository>();

            _mockRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        _mockDb.Remove(product);
                });

            var _productService = new ProductService(_mockCart, _mockRepository.Object, null, null);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(3, _mockDb.Count);
            Assert.Equal(2, _mockDb.First(p => p.Id == 1).Quantity);
            Assert.Equal(3, _mockDb.First(p => p.Id == 2).Quantity);
            Assert.Equal(4, _mockDb.First(p => p.Id == 3).Quantity);
        }

        [Fact]
        public void UpdateProductQuantityThrowsGivenEmptyCart()
        {
            // Arrange
            Cart _mockCart = null;

            var _mockDb = new List<Product>
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

            var _mockRepository = new Mock<IProductRepository>();
            _mockRepository
                .Setup(x => x.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int quantityToRemove) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= quantityToRemove;

                    if (product.Quantity == 0)
                        _mockDb.Remove(product);
                });

            var _productService = new ProductService(_mockCart, _mockRepository.Object, null, null);

            // Act
            Action testAction = () => _productService.UpdateProductQuantities();

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }
<<<<<<< HEAD
=======

        [Fact]
        public void DeleteProductRemovesCartLineAndDeletesProductFromDb()
        {
            // Arrange
            var _mockDb = new List<Product>
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

            var _mockCartLines = new List<CartLine>();

            var _mockCart = new Mock<ICart>();
            _mockCart
                .Setup(x => x.AddItem(It.IsAny<Product>(), It.IsAny<int>()))
                .Callback((Product product, int quantity) => {
                    CartLine line = _mockCartLines.FirstOrDefault(p => p.Product.Id == product.Id);

                    if (line == null)
                    {
                        _mockCartLines.Add(new CartLine { Product = product, Quantity = quantity });
                    }
                    else
                    {
                        line.Quantity += quantity;
                    }
                });

            _mockCart
                .Setup(x => x.RemoveLine(It.IsAny<Product>()))
                .Callback((Product product) => _mockCartLines.RemoveAll(l => l.Product.Id == product.Id));


            var _mockRepository = new Mock<IProductRepository>();
            _mockRepository
                .Setup(x => x.DeleteProduct(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    if (product != null)
                    {
                        _mockDb.Remove(product);
                    }
                });

            _mockRepository
                .Setup(x => x.GetAllProducts())
                .Returns(_mockDb.Where(p => p.Id > 0).ToList());

            var _productService = new ProductService(_mockCart.Object, _mockRepository.Object, null, null);

            foreach (var p in _mockDb)
            {
                _mockCart.Object.AddItem(p, 1);
            }

            // Act
            _productService.DeleteProduct(1);

            // Assert
            Assert.Equal(2, _mockDb.Count);
            Assert.Equal(2, _mockCartLines.Count);
        }

        [Fact]
        public void DeleteProductThrowsGivenInvalidId()
        {
            // Arrange
            var _mockDb = new List<Product>
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

            var _mockCartLines = new List<CartLine>();

            var _mockCart = new Mock<ICart>();
            _mockCart
                .Setup(x => x.AddItem(It.IsAny<Product>(), It.IsAny<int>()))
                .Callback((Product product, int quantity) => {
                    CartLine line = _mockCartLines.FirstOrDefault(p => p.Product.Id == product.Id);

                    if (line == null)
                    {
                        _mockCartLines.Add(new CartLine { Product = product, Quantity = quantity });
                    }
                    else
                    {
                        line.Quantity += quantity;
                    }
                });

            _mockCart
                .Setup(x => x.RemoveLine(It.IsAny<Product>()))
                .Callback((Product product) => _mockCartLines.RemoveAll(l => l.Product.Id == product.Id));


            var _mockRepository = new Mock<IProductRepository>();
            _mockRepository
                .Setup(x => x.DeleteProduct(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    Product product = _mockDb.FirstOrDefault(p => p.Id == id);
                    if (product != null)
                    {
                        _mockDb.Remove(product);
                    }
                });

            _mockRepository
                .Setup(x => x.GetAllProducts())
                .Returns(_mockDb.Where(p => p.Id > 0).ToList());

            var _productService = new ProductService(_mockCart.Object, _mockRepository.Object, null, null);

            foreach (var p in _mockDb)
            {
                _mockCart.Object.AddItem(p, 1);
            }

            // Act
            Action testAction = () => _productService.DeleteProduct(-1);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }
>>>>>>> 57fe56e640d30388fc9945f71cbd6457e2069446
    }
}
