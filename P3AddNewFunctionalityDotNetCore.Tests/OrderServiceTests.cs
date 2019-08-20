using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System;
using System.Collections.Generic;
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
                .UseInMemoryDatabase("order_service_test_db", new InMemoryDatabaseRoot())
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
}