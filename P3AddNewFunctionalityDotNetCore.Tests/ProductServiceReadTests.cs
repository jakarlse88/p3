using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
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

    public class ProductServiceReadTests
    {
        private readonly P3Referential _context;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public ProductServiceReadTests()
        {
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer("Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            _context = new P3Referential(options);

            _productRepository = new ProductRepository(_context);

            _productService = new ProductService(null, _productRepository, null, null);
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
        [InlineData(Int32.MaxValue)]
        [InlineData(Int32.MinValue)]
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
        [InlineData(Int32.MaxValue)]
        [InlineData(Int32.MinValue)]
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
        [InlineData(Int32.MaxValue)]
        [InlineData(Int32.MinValue)]
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
}

