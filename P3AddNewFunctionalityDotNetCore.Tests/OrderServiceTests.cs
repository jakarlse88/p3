using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async void GetOrderReturnsCorrectOrderGivenValidId()
        {
            // Arrange
            var mockOrderList = new List<Order>
            {
                new Order {
                    Id = 1,
                    Name = "One"
                },
                new Order {
                    Id = 2,
                    Name = "Two"
                },
                new Order {
                    Id = 3,
                    Name = "Three"
                },
            };

            IQueryable<Order> mockQueryableList =
                mockOrderList.AsQueryable();

            var mockOrderRepository = new Mock<IOrderRepository>();

            mockOrderRepository
                .Setup(x => x.GetOrder(It.IsAny<int?>()))
                .ReturnsAsync((int? id) => 
                    mockQueryableList
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .FirstOrDefault(m => m.Id == id)
                );

            var mockOrderService =
                new OrderService(null, mockOrderRepository.Object, null);

            // Act
            var result = await mockOrderService.GetOrder(1);

            // Assert
            Assert.Equal("One", result.Name);
            mockOrderRepository
                .Verify(x => x.GetOrder(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void GetOrderReturnsNothingGivenInvalidId()
        {
            // Arrange
            var mockOrderList = new List<Order>
            {
                new Order {
                    Id = 1,
                    Name = "One"
                },
                new Order {
                    Id = 2,
                    Name = "Two"
                },
                new Order {
                    Id = 3,
                    Name = "Three"
                },
            };

            IQueryable<Order> mockQueryableList =
                mockOrderList.AsQueryable();

            var mockOrderRepository = new Mock<IOrderRepository>();

            mockOrderRepository
                .Setup(x => x.GetOrder(It.IsAny<int?>()))
                .ReturnsAsync((int? id) => 
                    mockQueryableList
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .FirstOrDefault(m => m.Id == id)
                );

            var mockOrderService =
                new OrderService(null, mockOrderRepository.Object, null);

            // Act
            var result = await mockOrderService.GetOrder(5);

            // Assert
            Assert.Null(result);
            mockOrderRepository
                .Verify(x => x.GetOrder(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetOrderReturnsNothingGivenNegativeInvalidId()
        {
            // Arrange
            var mockOrderList = new List<Order>
            {
                new Order {
                    Id = 1,
                    Name = "One"
                },
                new Order {
                    Id = 2,
                    Name = "Two"
                },
                new Order {
                    Id = 3,
                    Name = "Three"
                },
            };

            IQueryable<Order> mockQueryableList =
                mockOrderList.AsQueryable();

            var mockOrderRepository = new Mock<IOrderRepository>();

            mockOrderRepository
                .Setup(x => x.GetOrder(It.IsAny<int?>()))
                .ReturnsAsync((int? id) => 
                    mockQueryableList
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .FirstOrDefault(m => m.Id == id)
                );

            var mockOrderService =
                new OrderService(null, mockOrderRepository.Object, null);

            // Act
            var result = await mockOrderService.GetOrder(-5);

            // Assert
            Assert.Null(result);
            mockOrderRepository
                .Verify(x => x.GetOrder(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void GetOrdersReturnsAllOrders()
        {
            // Arrange
            var mockOrderList = new List<Order>
            {
                new Order {
                    Id = 1,
                    Name = "One"
                },
                new Order {
                    Id = 2,
                    Name = "Two"
                },
                new Order {
                    Id = 3,
                    Name = "Three"
                },
            };

            IQueryable<Order> mockQueryableList = 
                mockOrderList.AsQueryable();

            var mockOrderRepository = new Mock<IOrderRepository>();

            mockOrderRepository
                .Setup(x => x.GetOrders())
                .ReturnsAsync(() => 
                    mockQueryableList
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .ToList()
                );

            var orderService = new OrderService(null, mockOrderRepository.Object, null);

            // Act
            var result = await orderService.GetOrders();

            // Assert
            mockOrderRepository.Verify(x => x.GetOrders(), Times.Once);
            Assert.Equal(3, result.Count);
            Assert.Equal("One", result.FirstOrDefault(o => o.Id == 1).Name);
            Assert.Equal("Two", result.FirstOrDefault(o => o.Id == 2).Name);
            Assert.Equal("Three", result.FirstOrDefault(o => o.Id == 3).Name);
        }

        [Fact]
        public void SaveOrderSavesProductToDbGivenGoodArg()
        {
            // Arrange
            var mockOrderList = new List<Order>();

            var testCartLine = new CartLine();
            testCartLine.OrderLineId = 1;
            testCartLine.Product = new Product{
                Id = 1
            };
            testCartLine.Quantity = 10;

            var testObject = new OrderViewModel
            {
                OrderId = 1,
                Name = "Name",
                Address = "Address",
                City = "City",
                Zip = "Zip",
                Country = "Country",
                Lines = new List<CartLine>()
            };

            testObject.Lines.Add(testCartLine);

            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository
                .Setup(x => x.Save(It.IsAny<Order>()))
                .Callback((Order order) => mockOrderList.Add(order));

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.UpdateProductQuantities());
                // .Callback(() => Console.WriteLine("This does nothing, but seems to be the easiest way to mock a method which only needs to be called, not to do anything."));

            var mockCart = new Cart();

            var mockOrderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

            // Act
            mockOrderService.SaveOrder(testObject);

            // Assert
            mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
            mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Once);
            Assert.Single(mockOrderList);
        }

        [Fact]
        public void SaveOrdersSavesSeveralProductsToDbGivenGoodArgs()
        {
            // Arrange
            var mockOrderList = new List<Order>();

            var testCartLine1 = new CartLine();
            testCartLine1.OrderLineId = 1;
            testCartLine1.Product = new Product{
                Id = 1
            };
            testCartLine1.Quantity = 10;

            var testObject1 = new OrderViewModel
            {
                OrderId = 1,
                Name = "Name One",
                Address = "Address One",
                City = "City",
                Zip = "Zip",
                Country = "Country",
                Lines = new List<CartLine>()
            };

            testObject1.Lines.Add(testCartLine1);

            var testObject2 = new OrderViewModel
            {
                OrderId = 2,
                Name = "Name Two",
                Address = "Address Two",
                City = "City",
                Zip = "Zip",
                Country = "Country",
                Lines = new List<CartLine>()
            };

            var testCartLine2 = new CartLine();
            testCartLine2.OrderLineId = 2;
            testCartLine2.Product = new Product{
                Id = 2
            };
            testCartLine2.Quantity = 20;

            testObject2.Lines.Add(testCartLine1);

            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository
                .Setup(x => x.Save(It.IsAny<Order>()))
                .Callback((Order order) => mockOrderList.Add(order));

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.UpdateProductQuantities());
                // .Callback(() => Console.WriteLine("This does nothing, but seems to be the easiest way to mock a method which only needs to be called, not to do anything."));

            var mockCart = new Cart();

            var mockOrderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

            // Act
            mockOrderService.SaveOrder(testObject1);
            mockOrderService.SaveOrder(testObject2);

            // Assert
            mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.AtLeastOnce);
            mockProductService.Verify(x => x.UpdateProductQuantities(), Times.AtLeastOnce);
            Assert.Equal(2, mockOrderList.Count);
            Assert.Equal("Address One", mockOrderList.First(o => o.Name == "Name One").Address);
            Assert.Equal("Address Two", mockOrderList.First(o => o.Name == "Name Two").Address);
        }

        [Fact]
        public void SaveOrderThrowsGivenNullArgument()
        {
            // Arrange
            var mockOrderList = new List<Order>();

            var testCartLine = new CartLine();
            testCartLine.OrderLineId = 1;
            testCartLine.Product = new Product{
                Id = 1
            };
            testCartLine.Quantity = 10;

            var testObject = new OrderViewModel
            {
                OrderId = 1,
                Name = "Name",
                Address = "Address",
                City = "City",
                Zip = "Zip",
                Country = "Country",
                Lines = new List<CartLine>()
            };

            testObject.Lines.Add(testCartLine);

            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository
                .Setup(x => x.Save(It.IsAny<Order>()))
                .Callback((Order order) => mockOrderList.Add(order));

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.UpdateProductQuantities());
                // .Callback(() => Console.WriteLine("This does nothing, but seems to be the easiest way to mock a method which only needs to be called, not to do anything."));

            var mockCart = new Cart();

            var mockOrderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

            // Act
            Action testAction = () => mockOrderService.SaveOrder(null);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
            mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
            mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Never);
        }

        [Fact]
        public void SaveOrderThrowsGivenArgWithNullLinesField()
        {
            // Arrange
            var mockOrderList = new List<Order>();

            var testObject = new OrderViewModel
            {
                OrderId = 1,
                Name = "Name",
                Address = "Address",
                City = "City",
                Zip = "Zip",
                Country = "Country",
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository
                .Setup(x => x.Save(It.IsAny<Order>()))
                .Callback((Order order) => mockOrderList.Add(order));

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.UpdateProductQuantities());
                // .Callback(() => Console.WriteLine("This does nothing, but seems to be the easiest way to mock a method which only needs to be called, not to do anything."));

            var mockCart = new Cart();

            var mockOrderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

            // Act
            Action testAction = () => mockOrderService.SaveOrder(testObject);

            // Assert
            Assert.Throws<NullReferenceException>(testAction);
            mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
            mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Never);
        }

        [Fact]
        public void SaveOrderToleratesMissingAndNullFieldsNotLines()
        {
            // Arrange
            var mockOrderList = new List<Order>();

            var testCartLine = new CartLine();
            testCartLine.OrderLineId = 1;
            testCartLine.Product = new Product{
                Id = 1
            };
            testCartLine.Quantity = 10;

            var testObject = new OrderViewModel
            {
                Name = null,
                City = null,
                Country = null,
                Lines = new List<CartLine>()
            };

            testObject.Lines.Add(testCartLine);

            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository
                .Setup(x => x.Save(It.IsAny<Order>()))
                .Callback((Order order) => mockOrderList.Add(order));

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.UpdateProductQuantities());
                //.Callback(() => Console.WriteLine("This does nothing, but seems to be the easiest way to mock a method which only needs to be called, not to do anything."));

            var mockCart = new Cart();

            var mockOrderService = new OrderService(mockCart, mockOrderRepository.Object, mockProductService.Object);

            // Act
            mockOrderService.SaveOrder(testObject);

            // Assert
            Assert.Single(mockOrderList);
            mockOrderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
            mockProductService.Verify(x => x.UpdateProductQuantities(), Times.Once);
        }
    }
}