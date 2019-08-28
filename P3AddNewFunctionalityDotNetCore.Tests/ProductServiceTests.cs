using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Models;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    [Collection("InteractsWithDb")]
    public class ProductServiceReadTests
    {
        private readonly IProductService _productService;

        public ProductServiceReadTests()
        {
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer("Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            var context = new P3Referential(options);

            IProductRepository productRepository = new ProductRepository(context);

            _productService = new ProductService(null, productRepository, null, null);
        }

        [Fact]
        public void TestGetAllProducts()
        {
            // Arrange

            // Act
            var result = _productService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.IsAssignableFrom<Product>(result.First());
        }

        [Fact]
        public void TestGetAllProductsViewModel()
        {
            // Act
            var result = _productService.GetAllProductsViewModel();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.IsAssignableFrom<ProductViewModel>(result.First());
        }

        [Theory]
        [InlineData(1, "Echo Dot")]
        [InlineData(2, "Anker 3ft / 0.9m Nylon Braided")]
        [InlineData(3, "JVC HAFX8R Headphone")]
        [InlineData(4, "VTech CS6114 DECT 6.0")]
        [InlineData(5, "NOKIA OEM BL-5J")]
        public void TestGetProductByIdValidId(int testId, string expectedName)
        {
            // Act
            var result = _productService.GetProductById(testId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Product>(result);
            Assert.Equal(expectedName, result.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-7)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void TestGetProductByIdInvalidId(int testId)
        {
            // Act
            var result = _productService.GetProductById(testId);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1, "Echo Dot")]
        [InlineData(2, "Anker 3ft / 0.9m Nylon Braided")]
        [InlineData(3, "JVC HAFX8R Headphone")]
        [InlineData(4, "VTech CS6114 DECT 6.0")]
        [InlineData(5, "NOKIA OEM BL-5J")]
        public async void TestGetProductValidId(int testId, string expectedName)
        {
            // Act
            var result = await _productService.GetProduct(testId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Product>(result);
            Assert.Equal(expectedName, result.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-7)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async void TestGetProductInvalidId(int testId)
        {
            // Act
            var result = await _productService.GetProduct(testId);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1, "Echo Dot")]
        [InlineData(2, "Anker 3ft / 0.9m Nylon Braided")]
        [InlineData(3, "JVC HAFX8R Headphone")]
        [InlineData(4, "VTech CS6114 DECT 6.0")]
        [InlineData(5, "NOKIA OEM BL-5J")]
        public void TestGetProductByIdViewModelValidId(int testId, string expectedName)
        {
            // Act
            var result = _productService.GetProductByIdViewModel(testId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ProductViewModel>(result);
            Assert.Equal(expectedName, result.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-7)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void TestGetProductByIdViewModelInvalidId(int testId)
        {
            // Act
            var result = _productService.GetProductById(testId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void TestGetProduct()
        {
            // Act
            var result = await _productService.GetProduct();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IList<Product>>(result);
            Assert.Equal(5, result.Count);
        }
    }
    
    [Collection("InteractsWithDb")]
    public class ProductServiceUpdateMethodsTests
    {
        private readonly ProductViewModel _testProductViewModel;
        private readonly IEnumerable<Product> _testProductsList;

        public ProductServiceUpdateMethodsTests()
        {
            _testProductViewModel = new ProductViewModel
            {
                Id = 1,
                Stock = "1",
                Price = "10",
                Name = "Test Product",
                Description = "test description",
                Details = "test details"
            };

            _testProductsList = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "one name",
                    Description = "one description",
                    Details = "one details",
                    Quantity = 2,
                    Price = 10
                },
                new Product
                {
                    Id = 2,
                    Name = "two name",
                    Description = "two description",
                    Details = "two details",
                    Quantity = 4,
                    Price = 20
                },
                new Product
                {
                    Id = 3,
                    Name = "three name",
                    Description = "three description",
                    Details = "three details",
                    Quantity = 6,
                    Price = 30
                }
            };
        }

        private DbContextOptions<P3Referential> TestDbContextOptionsBuilder()
        {
            return new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;
        }

        private void SeedTestDb(DbContextOptions<P3Referential> options)
        {
            using (var context = new P3Referential(options))
            {
                foreach (var p in _testProductsList)
                {
                    context.Product.Add(p);
                }

                context.SaveChanges();
            }
        }

        [Fact]
        public void TestSaveProductPopulatedProduct()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.SaveProduct(_testProductViewModel);
            }

            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Equal("Test Product", result.First().Name);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductIdFieldMissing()
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Name = _testProductViewModel.Name,
                Description = _testProductViewModel.Description,
                Details = _testProductViewModel.Details,
                Stock = _testProductViewModel.Stock,
                Price = _testProductViewModel.Price
            };

            var options = TestDbContextOptionsBuilder();

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.SaveProduct(testObject);
            }

            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Equal(1, result.First().Id);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductNameFieldNull()
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Id = _testProductViewModel.Id,
                Name = null,
                Description = _testProductViewModel.Description,
                Details = _testProductViewModel.Details,
                Stock = _testProductViewModel.Stock,
                Price = _testProductViewModel.Price
            };

            var options = TestDbContextOptionsBuilder();

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.SaveProduct(testObject);
            }

            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Null(result.First().Name);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductDescriptionFieldNull()
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Id = _testProductViewModel.Id,
                Name = _testProductViewModel.Name,
                Description = null,
                Details = _testProductViewModel.Details,
                Stock = _testProductViewModel.Stock,
                Price = _testProductViewModel.Price
            };

            var options = TestDbContextOptionsBuilder();

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.SaveProduct(testObject);
            }

            // Act
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Null(result.First().Description);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductDetailsFieldNull()
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Id = _testProductViewModel.Id,
                Name = _testProductViewModel.Name,
                Description = _testProductViewModel.Description,
                Details = null,
                Stock = _testProductViewModel.Stock,
                Price = _testProductViewModel.Price
            };

            var options = TestDbContextOptionsBuilder();

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.SaveProduct(testObject);
            }

            // Act
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Null(result.First().Details);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductStockFieldNull()
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Id = _testProductViewModel.Id,
                Name = _testProductViewModel.Name,
                Description = _testProductViewModel.Description,
                Details = _testProductViewModel.Details,
                Stock = null,
                Price = _testProductViewModel.Price
            };

            var productService = new ProductService(null, null, null, null);

            // Act
            void TestAction() => productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Fact]
        public void TestSaveProductPriceFieldNull()
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Id = _testProductViewModel.Id,
                Name = _testProductViewModel.Name,
                Description = _testProductViewModel.Description,
                Details = _testProductViewModel.Details,
                Stock = _testProductViewModel.Stock,
                Price = null
            };

            var productService = new ProductService(null, null, null, null);

            // Act
            void TestAction() => productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(TestAction);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("123abc ")]
        [InlineData("\"123\"")]
        public void TestSaveProductInvalidStockField(string testString)
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Id = _testProductViewModel.Id,
                Name = _testProductViewModel.Name,
                Description = _testProductViewModel.Description,
                Details = _testProductViewModel.Details,
                Stock = testString,
                Price = _testProductViewModel.Price
            };

            var productService = new ProductService(null, null, null, null);

            // Act
            void TestAction() => productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<FormatException>(TestAction);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("123abc ")]
        [InlineData("\"123\"")]
        public void TestSaveProductInvalidPriceField(string testString)
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Id = _testProductViewModel.Id,
                Name = _testProductViewModel.Name,
                Description = _testProductViewModel.Description,
                Details = _testProductViewModel.Details,
                Stock = _testProductViewModel.Price,
                Price = testString
            };

            var productService = new ProductService(null, null, null, null);

            // Act
            void TestAction() => productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<FormatException>(TestAction);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("1")]
        [InlineData("\t1")]
        [InlineData(" 1")]
        public void TestSaveProductValidPriceField(string testString)
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Stock = _testProductViewModel.Stock,
                Price = testString
            };

            var options = TestDbContextOptionsBuilder();

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.SaveProduct(testObject);
            }

            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.IsAssignableFrom<double>(result.First().Price);
                
                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("1")]
        [InlineData("\t1")]
        [InlineData(" 1")]
        public void TestSaveProductValidStockField(string testString)
        {
            // Arrange
            var testObject = new ProductViewModel
            {
                Stock = _testProductViewModel.Stock,
                Price = testString
            };

            var options = TestDbContextOptionsBuilder();

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.SaveProduct(testObject);
            }

            // Act
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.IsAssignableFrom<double>(result.First().Price);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestUpdateProductQuantitiesEmptyCart()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productService = new ProductService(cart, null, null, null);

                // Act
                productService.UpdateProductQuantities();
            }

            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.Equal(2,
                    result.FirstOrDefault(p => p.Name == "one name").Quantity);
                Assert.Equal(4,
                    result.FirstOrDefault(p => p.Name == "two name").Quantity);
                Assert.Equal(6,
                    result.FirstOrDefault(p => p.Name == "three name").Quantity);
                
                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(3, 3, 3)]
        public void TestUpdateProductQuantitiesSingleItemCart(
            int testProdId, int testQty, int expectedQty)
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();
                cart.AddItem(
                    _testProductsList.FirstOrDefault(p => p.Id == testProdId), testQty);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.UpdateProductQuantities();
            }

            using (var context = new P3Referential(options))
            {
                // Assert
                Assert.Equal(expectedQty,
                    context.Product.ToList().FirstOrDefault(p => p.Id == testProdId).Quantity);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestUpdateProductQuantitiesMultipleItemsCart()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                for (var i = 0; i < _testProductsList.ToList().Count; i++)
                {
                    cart.AddItem(_testProductsList.ToArray()[i], i + 1);
                }

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.UpdateProductQuantities();
            }

            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                // Assert
                Assert.Equal(1, result.FirstOrDefault(p => p.Id == 1).Quantity);
                Assert.Equal(2, result.FirstOrDefault(p => p.Id == 2).Quantity);
                Assert.Equal(3, result.FirstOrDefault(p => p.Id == 3).Quantity);
                
                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public void TestUpdateProductQuantitiesInvalidProductId(int prodId)
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                cart.AddItem(new Product { Id = prodId }, 1);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                void TestAction() => productService.UpdateProductQuantities();

                // Assert
                Assert.Throws<InvalidOperationException>(TestAction);
            }
        }

        [Fact]
        public void TestUpdateProductQuantitiesCartQuantityExceedingProductStock()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                cart.AddItem(_testProductsList.First(), 3);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.UpdateProductQuantities();
            }

            // Assert
            using (var context = new P3Referential(options))
            {
                Assert.Equal(-1, context.Product.First().Quantity);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestUpdateProductQuantitiesCartQuantityEqualProductStock()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                cart.AddItem(_testProductsList.First(), 2);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.UpdateProductQuantities();
            }

            // Assert
            using (var context = new P3Referential(options))
            {
                Assert.Null(context.Product.ToList().FirstOrDefault(p => p.Id == 1));
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestUpdateProductQuantitiesCartQuantityLessProductStock()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                cart.AddItem(_testProductsList.ToList().FirstOrDefault(p => p.Id == 3), 3);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.UpdateProductQuantities();
            }

            using (var context = new P3Referential(options))
            {
                // Assert
                Assert.Equal(3, context.Product.ToList().FirstOrDefault(p => p.Id == 3).Quantity);
                
                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(4)]
        [InlineData(0)]
        [InlineData(-1)]
        public void TestDeleteProductIdInvalidId(int testId)
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                cart.AddItem(_testProductsList.ToList().FirstOrDefault(p => p.Id == 3), 3);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                void TestAction() => productService.DeleteProduct(testId);

                // Assert
                Assert.Throws<NullReferenceException>(TestAction);
            }
        }

        [Fact]
        public void TestDeleteProductValidIdEmptyCart()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            using (var context = new P3Referential(options))
            {
                var cart = new Cart();

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.DeleteProduct(2);
            }

            using (var context = new P3Referential(options))
            {
                // Assert
                Assert.Equal(2, context.Product.ToList().Count);
                Assert.DoesNotContain(context.Product.ToList(), p => p.Id == 2);
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestDeleteProductValidIdCartSingleItem()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            var cart = new Cart();

            using (var context = new P3Referential(options))
            {
                cart.AddItem(context.Product.ToList().First(p => p.Id == 2), 2);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.DeleteProduct(2);
            }

            using (var context = new P3Referential(options))
            {
                // Assert
                Assert.Equal(2, context.Product.ToList().Count);
                Assert.DoesNotContain(context.Product.ToList(), p => p.Id == 2);
                Assert.Empty(cart.Lines.ToList());
                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestDeleteProductValidIdCartMultipleItems()
        {
            // Arrange
            var options = TestDbContextOptionsBuilder();

            SeedTestDb(options);

            var cart = new Cart();

            using (var context = new P3Referential(options))
            {
                cart.AddItem(context.Product.First(p => p.Id == 1), 1);
                cart.AddItem(context.Product.First(p => p.Id == 2), 2);

                var productRepository = new ProductRepository(context);

                var productService = new ProductService(cart, productRepository, null, null);

                // Act
                productService.DeleteProduct(2);
            }

            using (var context = new P3Referential(options))
            {
                // Assert
                Assert.Equal(2, context.Product.ToList().Count);
                Assert.DoesNotContain(context.Product.ToList(), p => p.Id == 2);
                Assert.Single(cart.Lines.ToList());
                Assert.Equal("one name",
                    cart.Lines.FirstOrDefault(p => p.OrderLineId == 0).Product.Name);
                
                context.Database.EnsureDeleted();
            }
        }
    }
}

