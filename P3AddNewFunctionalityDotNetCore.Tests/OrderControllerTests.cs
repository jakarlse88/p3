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
        [Fact]
        public void IndexReturnsViewWithNewOrderViewModel()
        {
            // Arrange
            var orderController = new OrderController(null, null, null);

            // Act
            var result = orderController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<OrderViewModel>(result.ViewData.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void IndexWithArgSavesOrderRedirectsToActionCompleted()
        {
            // Arrange
            var cart = new Cart();

            var testList = new List<OrderViewModel>();
            
            var testOrder = new OrderViewModel
            {
                OrderId = 1,
                Name = "Order one"
            };

            var testProduct = new Product{
                Id = 1,
                Name = "Product One"
            };

            cart.AddItem(testProduct, 1);

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(x => x.SaveOrder(It.IsAny<OrderViewModel>()))
                .Callback((OrderViewModel order) => testList.Add(order));

            var orderController = new OrderController(cart, mockOrderService.Object, null);

            // Act
            var result = orderController.Index(testOrder);

            // Assert
            mockOrderService
                .Verify(x => x.SaveOrder(It.IsAny<OrderViewModel>()), Times.Once);

            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Single(testList);
        }

        [Fact]
        public void IndexWithArgDoesNothingEmptyCart()
        {
            // Arrange
            var cart = new Cart();

            var testOrder = new OrderViewModel();

            var mockLocalizer = new Mock<IStringLocalizer<OrderController>>();
            var cartEmptyKey = "Sorry, your cart is empty!";
            var localizedCartEmptyString = new LocalizedString(cartEmptyKey, cartEmptyKey);
            mockLocalizer
                .Setup(x => x["CartEmpty"])
                .Returns(localizedCartEmptyString);

            var orderController = new OrderController(cart, null, mockLocalizer.Object);

            // Act
            var result = orderController.Index(testOrder);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(orderController.ModelState.IsValid);
        }

        [Fact]
        public void CompletedClearsCartAndReturnsView()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(new Product {Name = "Produt One" }, 1);
            cart.AddItem(new Product {Name = "Produt Two" }, 2);
            cart.AddItem(new Product {Name = "Produt Three" }, 3);

            var orderController = new OrderController(cart, null, null);

            // Act
            var result = orderController.Completed();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Empty(cart.Lines);
        }
    }
}