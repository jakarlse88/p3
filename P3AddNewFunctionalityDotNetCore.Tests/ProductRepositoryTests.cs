using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System.Threading.Tasks;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductRepositoryTests
    {
        private readonly P3Referential _context;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(
                    "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            _context = new P3Referential(options);
        }

        // https://www.loganfranken.com/blog/517/mocking-dbset-queries-in-ef6/?fbclid=IwAR3WRTViz-9uk9QZhODGzWhtd5VcZ0gl4I58Wg16iWt0wUN8tlrwLDyVQ_8
        private DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider).Returns(queryable.Provider);

            dbSet.As<IQueryable<T>>()
                .Setup(m => m.Expression).Returns(queryable.Expression);
            
            dbSet.As<IQueryable<T>>()
                .Setup(m => m.ElementType).Returns(queryable.ElementType);

            dbSet.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }

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
            var productRepository = new ProductRepository(_context);

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
            var productRepository = new ProductRepository(_context);

            // Act
            var result = await productRepository.GetProduct(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductByIdReturnsNullGivenInvalidPositiveId()
        {
            // Arrange
            var productRepository = new ProductRepository(_context);

            // Act
            var result = await productRepository.GetProduct(10);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductByIdReturnsNullGivenZeroId()
        {
            // Arrange
            var productRepository = new ProductRepository(_context);

            // Act
            var result = await productRepository.GetProduct(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductReturnsCorrectNumberOfProducts()
        {
            // Arrange
            ProductRepository productRepository = new ProductRepository(_context);

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
            ProductRepository productRepository = new ProductRepository(_context);

            // Act
            var result = productRepository.GetAllProducts().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void SaveProductSavesValidProductToDb()
        {
            // Arrange
            var productList = new List<Product>();

            var testProduct = new Product
            {
                Id = 1,
                Name = "Name"
            };

            // Hacky, but seems necessary to mock P3Referential
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(
                    "asdasd")
                .Options;

            var mockContext = new Mock<P3Referential>(options);
            mockContext
                .Setup(x => x.Product.Add(It.IsAny<Product>()))
                .Callback((Product product) => productList.Add(product));

            mockContext
                .Setup(x => x.SaveChanges());

            var productRepository = new ProductRepository(mockContext.Object);


            // Act
            productRepository.SaveProduct(testProduct);

            // Assert
            Assert.Single(productList);
            Assert.Equal("Name", productList.First(p => p.Id == 1).Name);
        }
    }
}
