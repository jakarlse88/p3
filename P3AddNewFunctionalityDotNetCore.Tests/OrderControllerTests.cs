using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class OrderControllerTests 
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<IStringLocalizer<OrderController>> _mockLocalizer;
        private readonly List<OrderViewModel> _testOrderViewModelList;
        private readonly Product[] _testProductsArr;

        public OrderControllerTests()
        {
            _testOrderViewModelList = new List<OrderViewModel>();

            _testProductsArr = new Product[]
            {
                new Product { Name = "product one" },
                new Product { Name = "product two" },
                new Product { Name = "product three" }
            };
            
            _mockOrderService = new Mock<IOrderService>();
            
            _mockOrderService
                .Setup(x => x.SaveOrder(It.IsAny<OrderViewModel>()))
                .Callback((OrderViewModel order) => _testOrderViewModelList.Add(order));

            var cartEmptyKey = "Sorry, your cart is empty!";
            var localizedCartEmptyString = new LocalizedString(cartEmptyKey, cartEmptyKey);
            _mockLocalizer = new Mock<IStringLocalizer<OrderController>>();
            _mockLocalizer
                .Setup(x => x["CartEmpty"])
                .Returns(localizedCartEmptyString);
        }

        [Fact]
        public void TestIndex()
        {
            // Arrange
            var orderController = new OrderController(null, null, null);

            // Act
            var result = orderController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<OrderViewModel>(viewResult.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void TestIndexWithPopulatedCart()
        {
            // Arrange
            var cart = new Cart();

            var testOrder = new OrderViewModel
            {
                OrderId = 1,
                Name = "Order one"
            };

            var testProduct = _testProductsArr[0];

            cart.AddItem(testProduct, 1);

            var orderController = new OrderController(cart, _mockOrderService.Object, null);

            // Act
            var result = orderController.Index(testOrder);

            // Assert
            _mockOrderService
                .Verify(x => x.SaveOrder(It.IsAny<OrderViewModel>()), Times.Once);

            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Single(_testOrderViewModelList);
        }

        [Fact]
        public void TestIndexWithEmptyCart()
        {
            // Arrange
            var cart = new Cart();

            var testOrder = new OrderViewModel();

            var orderController = new OrderController(cart, null, _mockLocalizer.Object);

            // Act
            var result = orderController.Index(testOrder);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(orderController.ModelState.IsValid);
        }

        [Fact]
        public void TestCompleted()
        {
            // Arrange
            var cart = new Cart();

            for (int i = 0; i < _testProductsArr.Length; i++)
            {
                cart.AddItem(_testProductsArr[i], i+1);
            }

            var orderController = new OrderController(cart, null, null);

            // Act
            var result = orderController.Completed();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Empty(cart.Lines);
        }
    }
}