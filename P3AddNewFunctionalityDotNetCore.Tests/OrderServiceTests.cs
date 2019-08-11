using System;
using System.Collections.Generic;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async void GetOrderReturnsCorrectOrder()
        {
            // Arrange
            var _mockContext = new P3Referential(null);

            var _mockOrderList = new List<Product>
            {
                new Product {
                    Id = 1,
                    Name = "One"
                },
                new Product {
                    Id = 2,
                    Name = "Two"
                },
                new Product {
                    Id = 3,
                    Name = "Three"
                },
            };

            IQueryable<Product> _mockQueryableList = 
                _mockOrderList.AsQueryable();

            var _mockSet = new Mock<DbSet<Product>>();
            _mockSet
                .As<IQueryable<Product>>()
                .Setup(x => x.Provider)
                .Returns(_mockQueryableList.Provider);

            _mockSet
                .As<IQueryable<Product>>()
                .Setup(x => x.Expression)
                .Returns(_mockQueryableList.Expression);

            _mockSet
                .As<IQueryable<Product>>()
                .Setup(x => x.ElementType)
                .Returns(_mockQueryableList.ElementType);

            _mockSet
                .As<IQueryable<Product>>()
                .Setup(x => x.GetEnumerator())
                .Returns(_mockQueryableList.GetEnumerator);

            _mockContext.Product = _mockSet.Object;

            var _mockOrderRepository = new Mock<IOrderRepository>();

            _mockOrderRepository
                .Setup(x => x.GetOrder(It.IsAny<int?>()))
                .Callback((int? id) =>
                {
                    var orderEntity = _mockContext.Order
                        .Include(x => x.OrderLine)
                        .ThenInclude(product => product.Product)
                        .FirstOrDefault(m => m.Id == id);
                });

            var _mockOrderService = 
                new OrderService(null, _mockOrderRepository.Object, null);

            // Act
            var result = await _mockOrderService.GetOrder(1);

            // Assert
            Assert.Equal("One", result.Name);
        }
    }
}