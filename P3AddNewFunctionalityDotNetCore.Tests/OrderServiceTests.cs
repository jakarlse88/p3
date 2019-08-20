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
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class DbFixture : IDisposable
    {
        public P3Referential Context { get; private set; }

        public DbFixture()
        {
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase("order_service_read_tests_db", new InMemoryDatabaseRoot())
                .Options;

            Context = new P3Referential(options);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

    public class OrderServiceReadTests : IClassFixture<DbFixture>
    {
        private readonly IEnumerable<Order> _testOrderList;
        private readonly DbFixture _fixture;
        private readonly ICart _cart;
        private readonly IProductService _productService;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public OrderServiceReadTests(DbFixture fixture)
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

            _fixture = fixture;

            _orderRepository = new OrderRepository(_fixture.Context);

            foreach (var o in _testOrderList)
            {
                _fixture.Context.Add(o);
                _fixture.Context.SaveChanges();
            }

            _cart = new Cart();

            _productService = new ProductService(_cart, null, _orderRepository, null);
            _orderService = new OrderService(_cart, _orderRepository, _productService);
        }

        [Fact]
        public async Task TestGetOrderIdNegative()
        {
            // Act
            var result = await _orderService.GetOrder(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetOrderIdZero()
        {
            // Act
            var result = await _orderService.GetOrder(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetOrderIdPositiveInvalid()
        {
            // Act
            var result = await _orderService.GetOrder(4);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetOrderIdValid()
        {
            // Act
            var result = await _orderService.GetOrder(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Order>(result);
            Assert.Equal("one", result.Name);
        }

        [Fact]
        public async Task TestGetOrders()
        {
            // Act
            var result = await _orderService.GetOrders();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IList<Order>>(result);
            Assert.Equal(3, result.Count);
        }
    }

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
            Action testAction = () => _orderService.SaveOrder(testOrder);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderAllFieldsNull()
        {
            // Arrange
            var testOrder = new OrderViewModel();

            // Act
            Action testAction = () => _orderService.SaveOrder(testOrder);

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
            Action testAction = () => _orderService.SaveOrder(testObject);

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
            Action testAction = () => _orderService.SaveOrder(testObject);

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
            Action testAction = () => _orderService.SaveOrder(testObject);

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
            Action testAction = () => _orderService.SaveOrder(testObject);

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
            Action testAction = () => _orderService.SaveOrder(testObject);

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
            Action testAction = () => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderLinesFieldEmpty()
        {
            // Act
            _orderService.SaveOrder(_testOrderViewModel);

            var result = _context.Order.ToList().First();

            // Assert
            Assert.Single(_context.Order.ToList());
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Order>(result);
            Assert.Equal("name one", result.Name);
        }

        [Fact]
        public void TestSaveOrderAllFieldsValidLinesSeveral()
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
                    new CartLine { OrderLineId = 1, Product = new Product { Name = "product one" }, Quantity = 1 },
                    new CartLine { OrderLineId = 2, Product = new Product { Name = "product two" }, Quantity = 2 },
                    new CartLine { OrderLineId = 3, Product = new Product { Name = "product three" }, Quantity = 3 }
                }
            };

            // Act
            _orderService.SaveOrder(testObject);

            var result = _context.Order.ToList().First();

            // Assert
            Assert.Single(_context.Order.ToList());
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Order>(result);
            Assert.Equal("name one", result.Name);
            Assert.Equal(3, result.OrderLines.Count);
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
            Action testAction = () => _orderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
        }

        [Fact]
        public void TestSaveOrderLinesEmptyProduct()
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
                    new CartLine { OrderLineId = 1, Product = new Product(), Quantity = 1 },
                }
            };

            // Act
            _orderService.SaveOrder(testObject);

            var result = _context.Order.ToList().First();

            // Assert
            Assert.Single(_context.Order.ToList());
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Order>(result);
            Assert.Equal("name one", result.Name);
            Assert.Single(result.OrderLines.ToList());
        }
    }
}