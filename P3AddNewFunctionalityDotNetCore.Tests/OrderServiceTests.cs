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
            var _mockOrderList = new List<Order>
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

            IQueryable<Order> _mockQueryableList =
                _mockOrderList.AsQueryable();

            var _mockOrderRepository = new Mock<IOrderRepository>();

            _mockOrderRepository
                .Setup(x => x.GetOrder(It.IsAny<int?>()))
                .ReturnsAsync((int? id) => 
                    _mockQueryableList
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .FirstOrDefault(m => m.Id == id)
                );

            var _mockOrderService =
                new OrderService(null, _mockOrderRepository.Object, null);

            // Act
            var result = await _mockOrderService.GetOrder(1);

            // Assert
            Assert.Equal("One", result.Name);
        }

        [Fact]
        public async void GetOrderReturnsNothingGivenInvalidId()
        {
            // Arrange
            var _mockOrderList = new List<Order>
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

            IQueryable<Order> _mockQueryableList =
                _mockOrderList.AsQueryable();

            var _mockOrderRepository = new Mock<IOrderRepository>();

            _mockOrderRepository
                .Setup(x => x.GetOrder(It.IsAny<int?>()))
                .ReturnsAsync((int? id) => 
                    _mockQueryableList
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .FirstOrDefault(m => m.Id == id)
                );

            var _mockOrderService =
                new OrderService(null, _mockOrderRepository.Object, null);

            // Act
            var result = await _mockOrderService.GetOrder(5);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrderReturnsNothingGivenNegativeInvalidId()
        {
            // Arrange
            var _mockOrderList = new List<Order>
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

            IQueryable<Order> _mockQueryableList =
                _mockOrderList.AsQueryable();

            var _mockOrderRepository = new Mock<IOrderRepository>();

            _mockOrderRepository
                .Setup(x => x.GetOrder(It.IsAny<int?>()))
                .ReturnsAsync((int? id) => 
                    _mockQueryableList
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .FirstOrDefault(m => m.Id == id)
                );

            var _mockOrderService =
                new OrderService(null, _mockOrderRepository.Object, null);

            // Act
            var result = await _mockOrderService.GetOrder(-5);

            // Assert
            Assert.Null(result);
        }
    }
}