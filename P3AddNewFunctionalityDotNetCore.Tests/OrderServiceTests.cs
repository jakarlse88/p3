using Microsoft.EntityFrameworkCore;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Assert.Equal(3, result.Count);
            Assert.Equal("One", result.FirstOrDefault(o => o.Id == 1).Name);
            Assert.Equal("Two", result.FirstOrDefault(o => o.Id == 2).Name);
            Assert.Equal("Three", result.FirstOrDefault(o => o.Id == 3).Name);
        }
    }
}