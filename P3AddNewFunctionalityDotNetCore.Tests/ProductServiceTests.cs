using Microsoft.EntityFrameworkCore;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        public class TheExampleTestMethods
        {
            private P3Referential _context;
            public TheExampleTestMethods()
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
        }

        public class TheGetAllProductsMethd
        {
            [Fact]
            public void ReturnsAListOfAllProductsMock()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
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
                List<Product> result = productService.GetAllProducts();
                Product resultTwo = result.FirstOrDefault(x => x.Id == 2);

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(resultTwo);
                Assert.Equal(3, result.Count);
                Assert.Equal("two", resultTwo.Description);
            }

            [Fact]
            public void ReturnsNullWhenRepositoryMockSetToNull()
            {
                // Arrange
                List<Product> mockAllProductsNull = null;

                var mockProductRepository = new Mock<IProductRepository>();
                mockProductRepository
                    .Setup(x => x.GetAllProducts())
                    .Returns(mockAllProductsNull);

                var productService = new ProductService(null, mockProductRepository.Object, null, null);

                // Act
                List<Product> result = productService.GetAllProducts();

                // Assert
                Assert.Null(result);
            }
        }

        public class TheGetProductByIdMethod
        {
            [Fact]
            public void ReturnsCorrectProductWithGoodId()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = 3;

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
            public void ReturnsNUllWhenPassedBadIdNegative()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = -1;

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
            public void ReturnsNUllWhenPassedBadIdZero()
            {
                // Arrange
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = 0;

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
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = 10;

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
        }

        public class TheGetProductMethod
        {
            [Fact]
            public async Task ReturnsCorrectProductGivenValidId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = 1;

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
            public async Task ReturnsNullWhenPassedNegativeId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = -1;

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
            public async Task ReturnsNullWhenPassedBadId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = 666;

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
            public async Task ReturnsNullWhenPassedZeroId()
            {
                // Arrange 
                List<Product> mockAllProducts = new List<Product>()
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

                int testId = 0;

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
        }
    }
}