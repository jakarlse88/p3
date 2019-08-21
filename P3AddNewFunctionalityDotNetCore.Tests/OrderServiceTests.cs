using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    [Collection("InMemDbCollection")]
    public class OrderServiceReadTests
    {
        private readonly IEnumerable<Order> _testOrderList;

        public OrderServiceReadTests()
        {
            _testOrderList = new List<Order>
                {
                new Order {
                    Id = 1,
                    Name = "one"
                },
                new Order {
                    Id = 2,
                    Name = "two"
                },
                new Order {
                    Id = 3,
                    Name = "three"
                },
            };
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public async Task TestGetOrderIdInvalid(int testId)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            // Run test against one instance of context
            using (var context = new P3Referential(options))
            {
                foreach (var o in _testOrderList)
                {
                    context.Order.Add(o);
                }

                context.SaveChanges();
            }

            // Verify data using separate context instance
            using (var context = new P3Referential(options))
            {
                var orderRepository = new OrderRepository(context);
                var orderService = new OrderService(null, orderRepository, null);

                // Act
                var result = await orderService.GetOrder(testId);

                // Assert
                Assert.Null(result);
                Assert.Equal(3, context.Order.Count());

                // Clean up
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task TestGetOrderIdValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            // Run test against one instance of context
            using (var context = new P3Referential(options))
            {
                foreach (var o in _testOrderList)
                {
                    context.Order.Add(o);
                }

                context.SaveChanges();
            }

            // Verify data using separate context instance
            using (var context = new P3Referential(options))
            {
                var orderRepository = new OrderRepository(context);
                var orderService = new OrderService(null, orderRepository, null);

                // Act
                var result = await orderService.GetOrder(1);

                // Assert
                Assert.NotNull(result);
                Assert.IsAssignableFrom<Order>(result);
                Assert.Equal("one", result.Name);

                // Clean up
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task TestGetOrders()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            // Run test against one instance of context
            using (var context = new P3Referential(options))
            {
                foreach (var o in _testOrderList)
                {
                    context.Order.Add(o);
                }

                context.SaveChanges();
            }

            // Verify data using separate context instance
            using (var context = new P3Referential(options))
            {
                var orderRepository = new OrderRepository(context);
                var orderService = new OrderService(null, orderRepository, null);

                // Act
                var result = await orderService.GetOrders();

                // Assert
                Assert.NotNull(result);
                Assert.IsAssignableFrom<IList<Order>>(result);
                Assert.Equal(3, result.Count);

                // Clean up
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }

    [Collection("InMemDbCollection")]
    public class OrderServiceUpdateTests : IDisposable
    {
        private readonly OrderViewModel _testOrderViewModel;
        private readonly P3Referential _context;
        private readonly ICart _cart;
        private readonly IProductService _productService;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public OrderServiceUpdateTests()
        {
            _testOrderViewModel = new OrderViewModel
            {
                OrderId = 1,
                Name = "name one",
                Address = "address one",
                City = "city",
                Zip = "zip",
                Country = "country",
                Lines = new List<CartLine>()
            };

            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("order_service_update_tests_db", new InMemoryDatabaseRoot())
                .Options;

            _context = new P3Referential(options);

            _cart = new Cart();

            _orderRepository = new OrderRepository(_context);

            _productService = new ProductService(_cart, null, _orderRepository, null);

            _orderService = new OrderService(_cart, _orderRepository, _productService);
        }

        public void Dispose()
        {
            _cart.Clear();
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void TestSaveOrderNullOrder()
        {
            // Arrange
            OrderViewModel testOrder = null;

            // Act
            void testAction() => _orderService.SaveOrder(testOrder);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderAllFieldsNull()
        {
            // Arrange
            var testOrder = new OrderViewModel();

            // Act
            void testAction() => _orderService.SaveOrder(testOrder);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderNameFieldNull()
        {
            // Arrange
            var testObject = new OrderViewModel
            {
                Name = null,
            };

            // Act
            void testAction() => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderAddressFieldNull()
        {
            // Arrange
            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = null
            };

            // Act
            void testAction() => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderCityFieldNull()
        {
            // Arrange
            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = _testOrderViewModel.Address,
                City = null
            };

            // Act
            void testAction() => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderZipFieldNull()
        {
            // Arrange
            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = _testOrderViewModel.Address,
                City = _testOrderViewModel.City,
                Zip = null
            };

            // Act
            void testAction() => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderCountryFieldNull()
        {
            // Arrange
            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = _testOrderViewModel.Address,
                City = _testOrderViewModel.City,
                Zip = _testOrderViewModel.Zip,
                Country = null
            };

            // Act
            void testAction() => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderLinesFieldNull()
        {
            // Arrange
            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = _testOrderViewModel.Address,
                City = _testOrderViewModel.City,
                Zip = _testOrderViewModel.Zip,
                Country = _testOrderViewModel.Country,
                Lines = null
            };

            // Act
            void testAction() => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderLinesFieldEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            var cart = new Cart();

            var mockProductService = new Mock<IProductService>();
                mockProductService
                    .Setup(x => x.UpdateProductQuantities());

            // Run test against one instance of context
            using (var context = new P3Referential(options))
            {
                var orderRepository = new OrderRepository(context);
                var orderService = new OrderService(cart, orderRepository, mockProductService.Object);

                // Act
                orderService.SaveOrder(_testOrderViewModel);
            }

            // Verify data using separate context instance
            using (var context = new P3Referential(options))
            {
                var result = context
                    .Order
                    .Include(x => x.OrderLines)
                    .First();

                // Assert
                Assert.Single(context.Order.ToList());
                Assert.NotNull(result);
                Assert.IsAssignableFrom<Order>(result);
                Assert.Equal("name one", result.Name);

                mockProductService
                    .Verify(x => x.UpdateProductQuantities(), Times.Once);

                // Clean up
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public void TestSaveOrderAllFieldsValidLinesSeveral()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = _testOrderViewModel.Address,
                City = _testOrderViewModel.City,
                Zip = _testOrderViewModel.Zip,
                Country = _testOrderViewModel.Country,
                Lines = new List<CartLine>
                {
                    new CartLine { OrderLineId = 1, Product = new Product { Id = 1, Quantity = 1 }, Quantity = 1 },
                    new CartLine { OrderLineId = 2, Product = new Product { Id = 2, Quantity = 2 }, Quantity = 2 },
                    new CartLine { OrderLineId = 3, Product = new Product { Id = 3, Quantity = 3 }, Quantity = 3 }
                }
            };

            var cart = new Cart();

            var mockProductService = new Mock<IProductService>();
                mockProductService
                    .Setup(x => x.UpdateProductQuantities());

            // Run test against one instance of context
            using (var context = new P3Referential(options))
            {
                var orderRepository = new OrderRepository(context);

                var orderService = new OrderService(cart, orderRepository, mockProductService.Object);

                // Act
                orderService.SaveOrder(testObject);
            }

            // Verify data using separate context instance
            using (var context = new P3Referential(options))
            {
                var result = context
                    .Order
                    .Include(x => x.OrderLines)
                    .First();

                // Assert
                Assert.Single(context.Order.ToList());
                Assert.NotNull(result);
                Assert.IsAssignableFrom<Order>(result);
                Assert.Equal("name one", result.Name);
                Assert.Equal(3, result.OrderLines.Count);

                mockProductService
                    .Verify(x => x.UpdateProductQuantities(), Times.Once);

                // Clean up
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public void TestSaveOrderLinesOrderLineProductNull()
        {
            // Arrange
            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = _testOrderViewModel.Address,
                City = _testOrderViewModel.City,
                Zip = _testOrderViewModel.Zip,
                Country = _testOrderViewModel.Country,
                Lines = new List<CartLine>
                {
                    new CartLine { OrderLineId = 1, Product = null, Quantity = 1 },
                }
            };

            // Act
            void testAction() => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderLinesEmptyProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            var testObject = new OrderViewModel
            {
                Name = _testOrderViewModel.Name,
                Address = _testOrderViewModel.Address,
                City = _testOrderViewModel.City,
                Zip = _testOrderViewModel.Zip,
                Country = _testOrderViewModel.Country,
                Lines = new List<CartLine>
                {
                    new CartLine { OrderLineId = 1, Product = new Product(), Quantity = 1 },
                }
            };

            var cart = new Cart();

            var mockProductService = new Mock<IProductService>();
                mockProductService
                    .Setup(x => x.UpdateProductQuantities());

            // Run test against one instance of context
            using (var context = new P3Referential(options))
            {
                var orderRepository = new OrderRepository(context);
                var orderService = new OrderService(cart, orderRepository, mockProductService.Object);

                // Act
                orderService.SaveOrder(testObject);
            }

            // Verify data using separate context instance
            using (var context = new P3Referential(options))
            {
                var result = context
                    .Order
                    .Include(x => x.OrderLines)
                    .First();

                // Assert
                Assert.Single(context.Order.ToList());
                Assert.NotNull(result);
                Assert.IsAssignableFrom<Order>(result);
                Assert.Equal("name one", result.Name);
                Assert.Single(result.OrderLines.ToList());

                mockProductService
                    .Verify(x => x.UpdateProductQuantities(), Times.Once);

                // Clean up
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}