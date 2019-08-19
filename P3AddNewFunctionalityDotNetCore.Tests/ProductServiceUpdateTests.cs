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
    // @TODO
    // Fix this stuff

    public class ProductServiceUpdateMethodsTests
    {
        private readonly ProductViewModel _testProductViewModel;
        //private readonly IEnumerable<Product> _testListAllProducts;
        //private readonly Mock<IProductRepository> _mockProductRepository;

        public ProductServiceUpdateMethodsTests()
        {
            //_testListAllProducts = new List<Product>()
            //{
            //    new Product
            //    {
            //        Id = 1,
            //        Description = "one",
            //        Details = "one_details"
            //    },
            //    new Product
            //    {
            //        Id = 2,
            //        Description = "two",
            //        Details = "two_details"
            //    },
            //    new Product
            //    {
            //        Id = 3,
            //        Description = "three",
            //        Details = "three_details"
            //    },
            //};

            _testProductViewModel = new ProductViewModel
            {
                Id = 1,
                Stock = "1",
                Price = "10",
                Name = "Test Product",
                Description = "test description",
                Details = "test details"
            };

            //_mockProductRepository = new Mock<IProductRepository>();

            //_mockProductRepository
            //    .Setup(x => x.GetAllProducts())
            //    .Returns(_testListAllProducts);
        }

        [Fact]
        public void TestSaveProductPopulatedProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_populated")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                productService.SaveProduct(_testProductViewModel);
                var result = context.Product.ToList();

                // Assert
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Equal("Test Product", result.First().Name);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                Assert.Single(context.Product.ToList());
                Assert.Equal(_testProductViewModel.Name, context.Product.ToList().First().Name);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductIdFieldMissing()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_missing_id")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                productService.SaveProduct(new ProductViewModel
                {
                    Name = _testProductViewModel.Name,
                    Description = _testProductViewModel.Description,
                    Details = _testProductViewModel.Details,
                    Stock = _testProductViewModel.Stock,
                    Price = _testProductViewModel.Price
                });

                var result = context.Product.ToList();

                // Assert
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Equal(1, result.First().Id);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                Assert.Single(result);
                Assert.Equal(1, result.First().Id);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductNameFieldNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_name_null")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                productService.SaveProduct(new ProductViewModel
                {
                    Id = _testProductViewModel.Id,
                    Name = null,
                    Description = _testProductViewModel.Description,
                    Details = _testProductViewModel.Details,
                    Stock = _testProductViewModel.Stock,
                    Price = _testProductViewModel.Price
                });

                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Null(result.First().Name);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

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
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_desc_null")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                productService.SaveProduct(new ProductViewModel
                {
                    Id = _testProductViewModel.Id,
                    Name = _testProductViewModel.Name,
                    Description = null,
                    Details = _testProductViewModel.Details,
                    Stock = _testProductViewModel.Stock,
                    Price = _testProductViewModel.Price
                });

                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Null(result.First().Description);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

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
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_details_null")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                productService.SaveProduct(new ProductViewModel
                {
                    Id = _testProductViewModel.Id,
                    Name = _testProductViewModel.Name,
                    Description = _testProductViewModel.Description,
                    Details = null,
                    Stock = _testProductViewModel.Stock,
                    Price = _testProductViewModel.Price
                });

                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.Null(result.First().Details);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

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
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_stock_null")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                Action testAction = () => productService.SaveProduct(new ProductViewModel
                {
                    Id = _testProductViewModel.Id,
                    Name = _testProductViewModel.Name,
                    Description = _testProductViewModel.Description,
                    Details = _testProductViewModel.Details,
                    Stock = null,
                    Price = _testProductViewModel.Price
                });

                // Assert
                Assert.Throws<ArgumentNullException>(testAction);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                Assert.Empty(context.Product);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestSaveProductPriceFieldNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_price_null")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                Action testAction = () => productService.SaveProduct(new ProductViewModel
                {
                    Id = _testProductViewModel.Id,
                    Name = _testProductViewModel.Name,
                    Description = _testProductViewModel.Description,
                    Details = _testProductViewModel.Details,
                    Stock = _testProductViewModel.Stock,
                    Price = null
                });

                // Assert
                Assert.Throws<ArgumentNullException>(testAction);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                Assert.Empty(context.Product);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("123abc ")]
        [InlineData("\"123\"")]
        public void TestSaveProductInvalidStockField(string testString)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_stock_bad")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                Action testAction = () => productService.SaveProduct(new ProductViewModel
                {
                    Id = _testProductViewModel.Id,
                    Name = _testProductViewModel.Name,
                    Description = _testProductViewModel.Description,
                    Details = _testProductViewModel.Details,
                    Stock = testString,
                    Price = _testProductViewModel.Price
                });

                // Assert
                Assert.Throws<FormatException>(testAction);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                Assert.Empty(context.Product);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("123abc ")]
        [InlineData("\"123\"")]
        public void TestSaveProductInvalidPriceField(string testString)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_stock_bad")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                Action testAction = () => productService.SaveProduct(new ProductViewModel
                {
                    Id = _testProductViewModel.Id,
                    Name = _testProductViewModel.Name,
                    Description = _testProductViewModel.Description,
                    Details = _testProductViewModel.Details,
                    Stock = _testProductViewModel.Price,
                    Price = testString
                });

                // Assert
                Assert.Throws<FormatException>(testAction);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                Assert.Empty(context.Product);

                context.Database.EnsureDeleted();
            }
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
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_price_valid")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                productService.SaveProduct(new ProductViewModel
                {
                    Stock = _testProductViewModel.Stock,
                    Price = testString
                });

                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.IsAssignableFrom<double>(result.First().Price);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

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
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("save_product_test_db_stock_valid")
                .Options;

            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);

                // Act
                productService.SaveProduct(new ProductViewModel
                {
                    Stock = _testProductViewModel.Stock,
                    Price = testString
                });

                var result = context.Product.ToList();

                // Assert
                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.IsAssignableFrom<double>(result.First().Price);
            }

            // Use a separate instance of the context to verify 
            // correct data was saved to database
            using (var context = new P3Referential(options))
            {
                var result = context.Product.ToList();

                Assert.NotNull(result.First());
                Assert.Single(result);
                Assert.IsAssignableFrom<List<Product>>(result);
                Assert.IsAssignableFrom<double>(result.First().Price);

                context.Database.EnsureDeleted();
            }
        }
    }
}
