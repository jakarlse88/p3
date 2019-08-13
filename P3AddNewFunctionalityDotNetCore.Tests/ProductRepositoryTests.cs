using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductRepositoryTests
    {
        private static readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        [Theory]
        [InlineData(1, "Echo Dot")]
        [InlineData(2, "Anker 3ft / 0.9m Nylon Braided")]
        [InlineData(3, "JVC HAFX8R Headphone")]
        [InlineData(4, "VTech CS6114 DECT 6.0")]
        [InlineData(5, "NOKIA OEM BL-5J")]
        public async Task GetProductByIdReturnsCorrectProductGivenGoodId(
            int id,
            string name
        )
        {
            // Arrange
            P3Referential context;
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(
                    "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            context = new P3Referential(options);

            var productRepository = new ProductRepository(context);

            // Act
            var result = await productRepository.GetProduct(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public async void GetProductByIdReturnsNullGivenNegativeId()
        {
            // Arrange
            P3Referential context;
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(
                    "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            context = new P3Referential(options);

            var productRepository = new ProductRepository(context);

            // Act
            var result = await productRepository.GetProduct(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductByIdReturnsNullGivenInvalidPositiveId()
        {
            // Arrange
            P3Referential context;
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(
                    "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            context = new P3Referential(options);

            var productRepository = new ProductRepository(context);

            // Act
            var result = await productRepository.GetProduct(10);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductByIdReturnsNullGivenZeroId()
        {
            // Arrange
            P3Referential context;
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(
                    "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            context = new P3Referential(options);

            var productRepository = new ProductRepository(context);

            // Act
            var result = await productRepository.GetProduct(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductReturnsCorrectNumberOfProducts()
        {
            // Arrange
            P3Referential context;
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(
                    "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            context = new P3Referential(options);

            ProductRepository productRepository = new ProductRepository(context);

            // Act
            var result = await productRepository.GetProduct();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void GetAllProductsReturnsCorrectNumberOfProducts()
        {
            // Arrange
            P3Referential context;
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(
                    "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            context = new P3Referential(options);

            ProductRepository productRepository = new ProductRepository(context);

            // Act
            var result = productRepository.GetAllProducts().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }
    }
}
