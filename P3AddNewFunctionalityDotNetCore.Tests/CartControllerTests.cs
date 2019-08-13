using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests 
{
    public class CartControllerTests
    {
        [Fact]
        public void IndexReturnsViewWithCartModel()
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
        public void AddToCartWorksAsExpectedWithValidId()
        {
            // Arrange
            var cart = new Cart();

            var testAllProductsList = new List<Product>
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

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns((int id) => testAllProductsList.FirstOrDefault(p => p.Id == id));

            var cartController = new CartController(cart, mockProductService.Object);

            // Act
            var result = cartController.AddToCart(2);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Single(cart.Lines);
            Assert.Contains(cart.Lines, l => l.OrderLineId == 0);
        }

        [Fact]
        public void AddToCartRedirectsToProductControllerIndexGivenInvalidId()
        {
            // Arrange
            var cart = new Cart();

            var testAllProductsList = new List<Product>
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

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns((int id) => testAllProductsList.FirstOrDefault(p => p.Id == id));

            var cartController = new CartController(cart, mockProductService.Object);

            // Act
            var result = cartController.AddToCart(4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal("Product", actionResult.ControllerName);
            Assert.Empty(cart.Lines);
        }

        [Fact]
        public void AddToCartRedirectsToProductControllerIndexGivenInvalidIdNegative()
        {
            // Arrange
            var cart = new Cart();

            var testAllProductsList = new List<Product>
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

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.GetProductById(It.IsAny<int>()))
                .Returns((int id) => testAllProductsList.FirstOrDefault(p => p.Id == id));

            var cartController = new CartController(cart, mockProductService.Object);

            // Act
            var result = cartController.AddToCart(-4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal("Product", actionResult.ControllerName);
            Assert.Empty(cart.Lines);
        }

        [Fact]
        public void RemoveFromCartWorksAsExpectedGivenValidId()
        {
            // Arrange
            var cart = new Cart();

            var testAllProductsList = new List<Product>
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

            foreach (var p in testAllProductsList)
            {
                cart.AddItem(p, 1);
            }

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.GetAllProducts())
                .Returns(testAllProductsList);

            var cartController = new CartController(cart, mockProductService.Object);

            // Act
            var result = cartController.RemoveFromCart(2);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal(2, cart.Lines.ToList().Count);
            Assert.DoesNotContain(cart.Lines, p => p.OrderLineId == 2);
        }

        [Fact]
        public void RedirectToActionResultRedirectsOnlyGivenInvalidId()
        {
            // Arrange
            var cart = new Cart();

            var testAllProductsList = new List<Product>
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

            foreach (var p in testAllProductsList)
            {
                cart.AddItem(p, 1);
            }

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.GetAllProducts())
                .Returns(testAllProductsList);

            var cartController = new CartController(cart, mockProductService.Object);

            // Act
            var result = cartController.RemoveFromCart(4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal(3, cart.Lines.ToList().Count);
        }

        [Fact]
        public void RedirectToActionResultRedirectsOnlyGivenInvalidIdNegative()
        {
            // Arrange
            var cart = new Cart();

            var testAllProductsList = new List<Product>
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

            foreach (var p in testAllProductsList)
            {
                cart.AddItem(p, 1);
            }

            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.GetAllProducts())
                .Returns(testAllProductsList);

            var cartController = new CartController(cart, mockProductService.Object);

            // Act
            var result = cartController.RemoveFromCart(-4);

            // Assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal(3, cart.Lines.ToList().Count);
        }
    }
}