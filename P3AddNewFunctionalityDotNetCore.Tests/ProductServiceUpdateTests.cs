using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    [Collection("InMemDbCollection")]
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
                foreach (Product p in _testProductsList)
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
                context.Dispose();
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
                context.Dispose();
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
                context.Dispose();
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
                context.Dispose();
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
                context.Dispose();
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
                context.Dispose();
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
                context.Dispose();
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
                context.Dispose();
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

                for (int i = 0; i < _testProductsList.ToList().Count; i++)
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
            }
        }
    }
}
