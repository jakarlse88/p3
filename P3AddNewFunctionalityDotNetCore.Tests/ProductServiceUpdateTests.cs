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
    public class ProductServiceUpdateMethodsTests : IDisposable
    {
        private readonly ProductViewModel _testProductViewModel;
        private readonly IEnumerable<Product> _testProductsList;
        private readonly P3Referential _context;
        private readonly IProductRepository _productRepository;
        private readonly ICart _cart;
        private readonly IProductService _productService;

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

            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("product_service_test_db", new InMemoryDatabaseRoot())
                .Options;

            _context = new P3Referential(options);

            _productRepository = new ProductRepository(_context);

            _cart = new Cart();

            _productService = new ProductService(_cart, _productRepository, null, null);
        }

        // Ensure fresh context for each test
        public void Dispose()
        {
            _cart.Clear();
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        private void SeedTestDb()
        {
            foreach (Product p in _testProductsList)
            {
                _context.Product.Add(p);
            }

            _context.SaveChanges();
        }

        [Fact]
        public void TestSaveProductPopulatedProduct()
        {
            // Act
            _productService.SaveProduct(_testProductViewModel);
            var result = _context.Product.ToList();

            // Assert
            Assert.Single(result);
            Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Equal("Test Product", result.First().Name);
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

            // Act
            _productService.SaveProduct(testObject);

            var result = _context.Product.ToList();

            // Assert
            Assert.Single(result);
            Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Equal(1, result.First().Id);
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

            // Act
            _productService.SaveProduct(testObject);

            var result = _context.Product.ToList();

            // Assert
            Assert.NotNull(result.First());
            Assert.Single(result);
            Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Null(result.First().Name);
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

            // Act
            _productService.SaveProduct(testObject);

            var result = _context.Product.ToList();

            // Assert
            Assert.NotNull(result.First());
            Assert.Single(result);
            Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Null(result.First().Description);
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

            // Act
            _productService.SaveProduct(testObject);

            var result = _context.Product.ToList();

            // Assert
            Assert.NotNull(result.First());
            Assert.Single(result);
            Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Null(result.First().Details);
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
            // Act
            Action testAction = () => _productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(testAction);
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

            // Act
            Action testAction = () => _productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<ArgumentNullException>(testAction);
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

            // Act
            Action testAction = () => _productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<FormatException>(testAction);
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

            // Act
            Action testAction = () => _productService.SaveProduct(testObject);

            // Assert
            Assert.Throws<FormatException>(testAction);
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

            // Act
            _productService.SaveProduct(testObject);

            var result = _context.Product.ToList();

            // Assert
            Assert.NotNull(result.First());
            Assert.Single(result);
            Assert.IsAssignableFrom<List<Product>>(result);
            Assert.IsAssignableFrom<double>(result.First().Price);
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

            // Act
            _productService.SaveProduct(testObject);

            var result = _context.Product.ToList();

            // Assert
            Assert.NotNull(result.First());
            Assert.Single(result);
            Assert.IsAssignableFrom<List<Product>>(result);
            Assert.IsAssignableFrom<double>(result.First().Price);
        }

        [Fact]
        public void TestUpdateProductQuantitiesEmptyCart()
        {
            // Arrange
            SeedTestDb();

            // Act
            _productService.UpdateProductQuantities();
            var result = _context.Product.ToList();

            // Assert
            Assert.Equal(2, result.FirstOrDefault(p => p.Name == "one name").Quantity);
            Assert.Equal(4, result.FirstOrDefault(p => p.Name == "two name").Quantity);
            Assert.Equal(6, result.FirstOrDefault(p => p.Name == "three name").Quantity);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(3, 3, 3)]
        public void TestUpdateProductQuantitiesSingleItemCart(int testProdId, int testQty, int expectedQty)
        {
            // Arrange
            SeedTestDb();

            _cart.AddItem(_testProductsList.FirstOrDefault(p => p.Id == testProdId), testQty);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(expectedQty,
                _context.Product.ToList().FirstOrDefault(p => p.Id == testProdId).Quantity);
        }

        [Fact]
        public void TestUpdateProductQuantitiesMultipleItemsCart()
        {
            // Arrange
            SeedTestDb();

            for (int i = 0; i < _testProductsList.ToList().Count; i++)
            {
                _cart.AddItem(_testProductsList.ToArray()[i], i + 1);
            }

            // Act
            _productService.UpdateProductQuantities();
            var result = _context.Product.ToList();

            // Assert
            Assert.Equal(1, result.FirstOrDefault(p => p.Id == 1).Quantity);
            Assert.Equal(2, result.FirstOrDefault(p => p.Id == 2).Quantity);
            Assert.Equal(3, result.FirstOrDefault(p => p.Id == 3).Quantity);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public void TestUpdateProductQuantitiesInvalidProductId(int prodId)
        {
            // Arrange
            SeedTestDb();

            _cart.AddItem(new Product { Id = prodId }, 1);

            // Act
            Action testAction = () => _productService.UpdateProductQuantities();

            // Assert
            Assert.Throws<InvalidOperationException>(testAction);
        }

        [Fact]
        public void TestUpdateProductQuantitiesCartQuantityExceedingProductStock()
        {
            // Arrange
            SeedTestDb();

            _cart.AddItem(_testProductsList.First(), 3);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(-1, _context.Product.ToList().First().Quantity);
        }

        [Fact]
        public void TestUpdateProductQuantitiesCartQuantityEqualProductStock()
        {
            // Arrange
            SeedTestDb();

            _cart.AddItem(_testProductsList.First(), 2);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Null(_context.Product.ToList().FirstOrDefault(p => p.Id == 1));
        }

        [Fact]
        public void TestUpdateProductQuantitiesCartQuantityLessProductStock()
        {
            // Arrange
            SeedTestDb();

            _cart.AddItem(_testProductsList.ToList().FirstOrDefault(p => p.Id == 3), 3);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            Assert.Equal(3, _context.Product.ToList().FirstOrDefault(p => p.Id ==3).Quantity);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(0)]
        [InlineData(-1)]
        public void TestDeleteProductIdInvalidId(int testId)
        {
            // Arrange
            SeedTestDb();

            // Act
            Action testAction = () => _productService.DeleteProduct(testId);

            // Assert
            Assert.Throws<InvalidOperationException>(testAction);
        }

        [Fact]
        public void TestDeleteProductValidIdEmptyCart()
        {
            // Arrange
            SeedTestDb();

            // Act
            _productService.DeleteProduct(2);

            // Assert
            Assert.Equal(2, _context.Product.ToList().Count);
            Assert.DoesNotContain(_context.Product.ToList(), p => p.Id == 2);
        }

        [Fact]
        public void TestDeleteProductValidIdCartSingleItem()
        {
            // Arrange
            SeedTestDb();
            var cart = _cart as Cart;
            cart.AddItem(_context.Product.ToList().First(p => p.Id == 2), 2);

            // Act
            _productService.DeleteProduct(2);

            // Assert
            Assert.Equal(2, _context.Product.ToList().Count);
            Assert.DoesNotContain(_context.Product.ToList(), p => p.Id == 2);
            Assert.Empty(cart.Lines.ToList());
        }

        [Fact]
        public void TestDeleteProductValidIdCartMultipleItems()
        {
            // Arrange
            SeedTestDb();

            var cart = _cart as Cart;
            cart.AddItem(_context.Product.ToList().First(p => p.Id == 1), 1);
            cart.AddItem(_context.Product.ToList().First(p => p.Id == 2), 2);

            // Act
            _productService.DeleteProduct(2);

            // Assert
            Assert.Equal(2, _context.Product.ToList().Count);
            Assert.DoesNotContain(_context.Product.ToList(), p => p.Id == 2);
            Assert.Single(cart.Lines.ToList());
            Assert.Equal("one name", 
                cart.Lines.FirstOrDefault(p => p.OrderLineId == 0).Product.Name);
        }
    }
}
