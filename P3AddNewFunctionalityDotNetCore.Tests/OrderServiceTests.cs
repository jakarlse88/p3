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
        //private readonly OrderViewModel _testObjectViewModel1, _testObjectViewModel2;
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

            //_testObjectViewModel1 = new OrderViewModel
            //{
            //    OrderId = 1,
            //    Name = "name one",
            //    Address = "address one",
            //    City = "city",
            //    Zip = "zip",
            //    Country = "country",
            //};

            //_testObjectViewModel2 = new OrderViewModel
            //{
            //    OrderId = 2,
            //    Name = "name two",
            //    Address = "address two",
            //    City = "city",
            //    Zip = "zip",
            //    Country = "country",
            //};

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
        public async Task TestGetProductIdNegative()
        {
            // Act
            var result = await _orderService.GetOrder(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetProductIdZero()
        {
            // Act
            var result = await _orderService.GetOrder(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetProductIdPositiveInvalid()
        {
            // Act
            var result = await _orderService.GetOrder(4);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetProductIdValid()
        {
            // Act
            var result = await _orderService.GetOrder(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Order>(result);
            Assert.Equal("one", result.Name);
        }

        //[Fact]
        //public async void GetOrderReturnsCorrectOrderGivenValidId()
        //{
        //    // Arrange
        //    IQueryable<Order> mockQueryableList =
        //        _testOrderList.AsQueryable();

        //    var mockOrderRepository = new Mock<IOrderRepository>();

        //    mockOrderRepository
        //        .Setup(x => x.GetOrder(It.IsAny<int?>()))
        //        .ReturnsAsync((int? id) =>
        //            mockQueryableList
        //                .Include(x => x.OrderLine)
        //                .ThenInclude(product => product.Product)
        //                .FirstOrDefault(m => m.Id == id)
        //        );

        //    var mockOrderService =
        //        new OrderService(null, mockOrderRepository.Object, null);

        //    // Act
        //    var result = await mockOrderService.GetOrder(1);

        //    // Assert
        //    Assert.Equal("One", result.Name);
        //    mockOrderRepository
        //        .Verify(x => x.GetOrder(It.IsAny<int>()), Times.Once);
        //}

        //[Fact]
        //public async void GetOrderReturnsNothingGivenInvalidId()
        //{
        //    // Arrange
        //    IQueryable<Order> mockQueryableList =
        //        _testOrderList.AsQueryable();

        //    var mockOrderRepository = new Mock<IOrderRepository>();

        //    mockOrderRepository
        //        .Setup(x => x.GetOrder(It.IsAny<int?>()))
        //        .ReturnsAsync((int? id) =>
        //            mockQueryableList
        //                .Include(x => x.OrderLine)
        //                .ThenInclude(product => product.Product)
        //                .FirstOrDefault(m => m.Id == id)
        //        );

        //    var mockOrderService =
        //        new OrderService(null, mockOrderRepository.Object, null);

        //    // Act
        //    var result = await mockOrderService.GetOrder(5);

        //    // Assert
        //    Assert.Null(result);
        //    mockOrderRepository
        //        .Verify(x => x.GetOrder(It.IsAny<int>()), Times.Once);
        //}

        //[Fact]
        //public async Task GetOrderReturnsNothingGivenNegativeInvalidId()
        //{
        //    // Arrange
        //    IQueryable<Order> mockQueryableList =
        //        _testOrderList.AsQueryable();

        //    var mockOrderRepository = new Mock<IOrderRepository>();

        //    mockOrderRepository
        //        .Setup(x => x.GetOrder(It.IsAny<int?>()))
        //        .ReturnsAsync((int? id) =>
        //            mockQueryableList
        //                .Include(x => x.OrderLine)
        //                .ThenInclude(product => product.Product)
        //                .FirstOrDefault(m => m.Id == id)
        //        );

        //    var mockOrderService =
        //        new OrderService(null, mockOrderRepository.Object, null);

        //    // Act
        //    var result = await mockOrderService.GetOrder(-5);

        //    // Assert
        //    Assert.Null(result);
        //    mockOrderRepository
        //        .Verify(x => x.GetOrder(It.IsAny<int>()), Times.Once);
        //}

        //[Fact]
        //public async void GetOrdersReturnsAllOrders()
        //{
        //    // Arrange
        //    IQueryable<Order> mockQueryableList =
        //        _testOrderList.AsQueryable();

        //    var mockOrderRepository = new Mock<IOrderRepository>();

        //    mockOrderRepository
        //        .Setup(x => x.GetOrders())
        //        .ReturnsAsync(() =>
        //            mockQueryableList
        //                .Include(x => x.OrderLine)
        //                .ThenInclude(product => product.Product)
        //                .ToList()
        //        );

        //    var orderService = new OrderService(null, mockOrderRepository.Object, null);

        //    // Act
        //    var result = await orderService.GetOrders();

        //    // Assert
        //    mockOrderRepository.Verify(x => x.GetOrders(), Times.Once);
        //    Assert.Equal(3, result.Count);
        //    Assert.Equal("One", result.FirstOrDefault(o => o.Id == 1).Name);
        //    Assert.Equal("Two", result.FirstOrDefault(o => o.Id == 2).Name);
        //    Assert.Equal("Three", result.FirstOrDefault(o => o.Id == 3).Name);
        //}

        //[Fact]
        //public void SaveOrderSavesProductToDbGivenGoodArg()
        //{
        //    // Arrange
        //    var testOrderList = new List<Order>();

        //    var testCartLine = new CartLine();
        //    testCartLine.OrderLineId = 1;
        //    testCartLine.Product = new Product
        //    {
        //        Id = 1
        //    };
        //    testCartLine.Quantity = 10;

        //    var testObject = new OrderViewModel
        //    {
        //        OrderId = 1,
        //        Name = "name one",
        //        Address = "address one",
        //        City = "city",
        //        Zip = "zip",
        //        Country = "country",
        //        Lines = new List<CartLine>()
        //    };

        //    testObject.Lines.Add(testCartLine);

        //    var mockOrderRepository = new Mock<IOrderRepository>();
        //    mockOrderRepository
        //        .Setup(x => x.Save(It.IsAny<Order>()))
        //        .Callback((Order order) => testOrderList.Add(order));

        //    var mockProductService = new Mock<IProductService>();
        //    mockProductService
        //        .Setup(x => x.UpdateProductQuantities());

        //    var mockCart = new Cart();

        //    var orderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

        //    // Act
        //    orderService.SaveOrder(testObject);

        //    // Assert
        //    mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
        //    mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Once);
        //    Assert.Single(testOrderList);
        //}

        //[Fact]
        //public void SaveOrdersSavesSeveralProductsToDbGivenGoodArgs()
        //{
        //    // Arrange
        //    var testOrderList = new List<Order>();

        //    var testCartLine1 = new CartLine();
        //    testCartLine1.OrderLineId = 1;
        //    testCartLine1.Product = new Product
        //    {
        //        Id = 1
        //    };
        //    testCartLine1.Quantity = 10;

        //    var testObject1 = new OrderViewModel
        //    {
        //        OrderId = _testObjectViewModel1.OrderId,
        //        Name = _testObjectViewModel1.Name,
        //        Address = _testObjectViewModel1.Address,
        //        City = _testObjectViewModel1.City,
        //        Zip = _testObjectViewModel1.Zip,
        //        Country = _testObjectViewModel1.Country,
        //        Lines = new List<CartLine>()
        //    };

        //    testObject1.Lines.Add(testCartLine1);

        //    var testObject2 = new OrderViewModel
        //    {
        //        OrderId = _testObjectViewModel2.OrderId,
        //        Name = _testObjectViewModel2.Name,
        //        Address = _testObjectViewModel2.Address,
        //        City = _testObjectViewModel2.City,
        //        Zip = _testObjectViewModel2.Zip,
        //        Country = _testObjectViewModel2.Country,
        //        Lines = new List<CartLine>()
        //    };

        //    var testCartLine2 = new CartLine();
        //    testCartLine2.OrderLineId = 2;
        //    testCartLine2.Product = new Product
        //    {
        //        Id = 2
        //    };
        //    testCartLine2.Quantity = 20;

        //    testObject2.Lines.Add(testCartLine1);

        //    var mockOrderRepository = new Mock<IOrderRepository>();
        //    mockOrderRepository
        //        .Setup(x => x.Save(It.IsAny<Order>()))
        //        .Callback((Order order) => testOrderList.Add(order));

        //    var mockProductService = new Mock<IProductService>();
        //    mockProductService
        //        .Setup(x => x.UpdateProductQuantities());

        //    var mockCart = new Cart();

        //    var orderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

        //    // Act
        //    orderService.SaveOrder(testObject1);
        //    orderService.SaveOrder(testObject2);

        //    // Assert
        //    mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.AtLeastOnce);
        //    mockProductService.Verify(x => x.UpdateProductQuantities(), Times.AtLeastOnce);
        //    Assert.Equal(2, testOrderList.Count);
        //    Assert.Equal("address one", testOrderList.First(o => o.Name == "name one").Address);
        //    Assert.Equal("address two", testOrderList.First(o => o.Name == "name two").Address);
        //}

        //[Fact]
        //public void SaveOrderThrowsGivenNullArgument()
        //{
        //    // Arrange
        //    var testOrderList = new List<Order>();

        //    var testCartLine = new CartLine();
        //    testCartLine.OrderLineId = 1;
        //    testCartLine.Product = new Product
        //    {
        //        Id = 1
        //    };
        //    testCartLine.Quantity = 10;

        //    var testOrderViewModel = new OrderViewModel
        //    {
        //        OrderId = _testObjectViewModel1.OrderId,
        //        Name = _testObjectViewModel1.Name,
        //        Address = _testObjectViewModel1.Address,
        //        City = _testObjectViewModel1.City,
        //        Zip = _testObjectViewModel1.Zip,
        //        Country = _testObjectViewModel1.Country,
        //        Lines = new List<CartLine> { testCartLine }
        //    };

        //    var mockOrderRepository = new Mock<IOrderRepository>();
        //    mockOrderRepository
        //        .Setup(x => x.Save(It.IsAny<Order>()))
        //        .Callback((Order order) => testOrderList.Add(order));

        //    var mockProductService = new Mock<IProductService>();
        //    mockProductService
        //        .Setup(x => x.UpdateProductQuantities());

        //    var mockCart = new Cart();

        //    var orderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

        //    // Act
        //    void testAction() => orderService.SaveOrder(null);

        //    // Assert
        //    Assert.Throws<NullReferenceException>(testAction);
        //    mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
        //    mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Never);
        //}

        //[Fact]
        //public void SaveOrderThrowsGivenArgWithNullLinesField()
        //{
        //    // Arrange
        //    var testOrderList = new List<Order>();

        //    var testOrderViewModel = new OrderViewModel
        //    {
        //        OrderId = _testObjectViewModel1.OrderId,
        //        Name = _testObjectViewModel1.Name,
        //        Address = _testObjectViewModel1.Address,
        //        City = _testObjectViewModel1.City,
        //        Zip = _testObjectViewModel1.Zip,
        //        Country = _testObjectViewModel1.Country,
        //    };

        //    var mockOrderRepository = new Mock<IOrderRepository>();
        //    mockOrderRepository
        //        .Setup(x => x.Save(It.IsAny<Order>()))
        //        .Callback((Order order) => testOrderList.Add(order));

        //    var mockProductService = new Mock<IProductService>();
        //    mockProductService
        //        .Setup(x => x.UpdateProductQuantities());

        //    var cart = new Cart();

        //    var orderService = new OrderService(cart, mockOrderRepository.Object, mockProductService.Object);

        //    // Act
        //    Action testAction = () => orderService.SaveOrder(testOrderViewModel);

        //    // Assert
        //    Assert.Throws<NullReferenceException>(testAction);
        //    mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
        //    mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Never);
        //}

        //[Fact]
        //public void SaveOrderToleratesMissingAndNullFieldsNotLines()
        //{
        //    // Arrange
        //    var testOrderList = new List<Order>();

        //    var testCartLine = new CartLine();
        //    testCartLine.OrderLineId = 1;
        //    testCartLine.Product = new Product
        //    {
        //        Id = 1
        //    };
        //    testCartLine.Quantity = 10;

        //    var testObject = new OrderViewModel
        //    {
        //        Name = null,
        //        City = null,
        //        Country = null,
        //        Lines = new List<CartLine>()
        //    };

        //    testObject.Lines.Add(testCartLine);

        //    var mockOrderRepository = new Mock<IOrderRepository>();
        //    mockOrderRepository
        //        .Setup(x => x.Save(It.IsAny<Order>()))
        //        .Callback((Order order) => testOrderList.Add(order));

        //    var mockProductService = new Mock<IProductService>();
        //    mockProductService
        //        .Setup(x => x.UpdateProductQuantities());

        //    var cart = new Cart();

        //    var orderService = new OrderService(cart, mockOrderRepository.Object, mockProductService.Object);

        //    // Act
        //    orderService.SaveOrder(testObject);

        //    // Assert
        //    Assert.Single(testOrderList);
        //    mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
        //    mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Once);
        //}
    }
}