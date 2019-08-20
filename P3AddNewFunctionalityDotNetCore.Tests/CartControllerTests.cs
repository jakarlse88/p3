using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CartControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly IEnumerable<Product> _testProductList;

        public CartControllerTests()
        {
            _testProductList = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "one name"
                },
                new Product
                {
                    Id = 2,
                    Name = "two name"
                },
                new Product
                {
                    Id = 3,
                    Name = "three name"
                },
            };

            _mockProductService = new Mock<IProductService>();

            _mockProductService
                .Setup(x => x.GetAllProducts())
                .Returns(_testProductList.ToList());

            _mockProductService
                .Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns((int id) => _testProductList.FirstOrDefault(p => p.Id == id));

        }

        [Fact]
        public void TestIndex()
        {
            // Arrange
            var cart = new Cart();

            var cartController = new CartController(cart, null);

            // Act
            var result = cartController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Cart>(viewResult.ViewData.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void TestAddToCartValidProductId()
        {
            // Arrange
            var cart = new Cart();

            var cartController = new CartController(cart, _mockProductService.Object);

            // Act
            var result = cartController.AddToCart(2);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Single(cart.Lines);
            Assert.Contains(cart.Lines, l => l.OrderLineId == 0);
        }

        [Fact]
        public void TestAddToCartInvalidProductId()
        {
            // Arrange
            var cart = new Cart();

            var cartController = new CartController(cart, _mockProductService.Object);

            // Act
            var result = cartController.AddToCart(4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal("Product", actionResult.ControllerName);
            Assert.Empty(cart.Lines);
        }

        [Fact]
        public void TestAddToCartNegativeProductId()
        {
            // Arrange
            var cart = new Cart();

            var cartController = new CartController(cart, _mockProductService.Object);

            // Act
            var result = cartController.AddToCart(-4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal("Product", actionResult.ControllerName);
            Assert.Empty(cart.Lines);
        }

        [Fact]
        public void TestRemoveFromCartValidProductId()
        {
            // Arrange
            var cart = new Cart();

            foreach (var p in _testProductList)
            {
                cart.AddItem(p, 1);
            }

            var cartController = new CartController(cart, _mockProductService.Object);

            // Act
            var result = cartController.RemoveFromCart(2);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal(2, cart.Lines.ToList().Count);
            Assert.DoesNotContain(cart.Lines, p => p.OrderLineId == 2);
        }

        [Fact]
        public void TestRemoveFromCartInvalidProductId()
        {
            // Arrange
            var cart = new Cart();

            foreach (var p in _testProductList)
            {
                cart.AddItem(p, 1);
            }

            var cartController = new CartController(cart, _mockProductService.Object);

            // Act
            var result = cartController.RemoveFromCart(4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal(3, cart.Lines.ToList().Count);
        }

        [Fact]
        public void TestRemoveFromCartNegativeProductId()
        {
            // Arrange
            var cart = new Cart();

            foreach (var p in _testProductList)
            {
                cart.AddItem(p, 1);
            }

            var cartController = new CartController(cart, _mockProductService.Object);

            // Act
            var result = cartController.RemoveFromCart(-4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal(3, cart.Lines.ToList().Count);
        }
    }
}