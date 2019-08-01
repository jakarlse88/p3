using Microsoft.EntityFrameworkCore;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        private P3Referential _context;
        public ProductServiceTests()
        {
            var options = new DbContextOptionsBuilder<P3Referential>().
                       UseSqlServer("Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true")
                       .Options;

            _context = new P3Referential(options);
        }

        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public async Task TestGetProductByIdAsyncMockTest()
        {
            // Arrange
            //      public int Id { get; set; }
            //public string Description { get; set; }
            //public string Details { get; set; }
            //public string Name { get; set; }
            //public double Price { get; set; }
            //public int Quantity { get; set; }

            //public virtual ICollection<OrderLine> OrderLine { get; set; }

            Product product = new Product
            {
                Id = 1,
                Description = "one",
                Details = "onedetails",

            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetProduct(1)).Returns(Task.FromResult(product));

            // Act
            var productService = new ProductService(null, mockProductRepository.Object, null, null);
            var result = await productService.GetProduct(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("one", result.Description);
        }

        [Theory]
        [InlineData(1, "one", "oneDetails")]
        [InlineData(2, "two", "twoDetails")]
        [InlineData(3, "three", "threeDetails")]
        public async Task TestGetProductByIdAsyncMockTestParameterizedTestData(int id, string description, string details)
        {
            // Arrange
            Product product = new Product
            {
                Id = id,
                Description = description,
                Details = details,
            };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetProduct(id)).Returns(Task.FromResult(product));

            // Act
            var productService = new ProductService(null, mockProductRepository.Object, null, null);
            var result = await productService.GetProduct(id);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(description, result.Description);
        }

        [Fact]
        public async Task TestGetProductByIdIntegrationTestAsync()
        {
            //Arrange
            ProductRepository productRepository = new ProductRepository(_context);

            // Act
            var result = await productRepository.GetProduct(5);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cell Phone", result.Description);
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }



}