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

        }
    }
}

// @TODO
// implement additional validation logic
// refactor test names + add to test log
// implement missing tests (here)