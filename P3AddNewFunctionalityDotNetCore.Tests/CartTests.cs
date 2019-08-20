using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CartTests
    {
        private readonly Product _testProduct;
        private readonly Product[] _testProductsArr;

        public CartTests()
        {
            _testProduct = new Product
            {
                Name = "test product",
                Price = 10D,
                Quantity = 4,
                Description = "test description",
                Details = "test details"
            };

            _testProductsArr = new Product[]
            {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
            };
        }

        [Fact]
        public void TestClear()
        {
            // Arrange
            var _cart = new Cart();

            foreach (var p in _testProductsArr)
            {
                _cart.AddItem(p, 1);
            }

            // Act
            _cart.Clear();

            // Assert
            Assert.NotNull(_cart.Lines);
            Assert.Empty(_cart.Lines);
        }

        [Fact]
        public void TestAddItemSingleProduct()
        {
            // Arrange
            var _cart = new Cart();

            const int testQuantity = 1;

            // Act
            _cart.AddItem(_testProduct, testQuantity);

            // Assert
            Assert.NotEmpty(_cart.Lines);
            Assert.Contains(_cart.Lines, l => l.Product.Name == "test product");
        }

        [Fact]
        public void TestAddItemProductAlreadyInCart()
        {
            // Arrange
            var _cart = new Cart();

            const int initialQuantity = 1;
            const int incrementQuantity = 2;
            const int expectedTotalQuantity = initialQuantity + incrementQuantity;

            // Act
            _cart.AddItem(_testProduct, initialQuantity);
            _cart.AddItem(_testProduct, incrementQuantity);

            // Assert
            var actual = _cart.Lines.FirstOrDefault(l => l.Product.Name == "test product");

            Assert.Single(_cart.Lines);
            Assert.Equal(expectedTotalQuantity, actual.Quantity);
        }

        [Fact]
        public void TestAddItemProductAlreadyInCartNegativeQuantity()
        {
            // Arrange
            var _cart = new Cart();

            const int testQuantity = -5;

            // Act
            _cart.AddItem(_testProduct, testQuantity);

            // Assert
            CartLine actual = _cart.Lines.FirstOrDefault(l => l.Product.Name == "test product");

            Assert.Single(_cart.Lines);
            Assert.Equal(testQuantity, actual.Quantity);
        }

        [Fact]
        public void TestAddItemNullProduct()
        {
            // Arrange
            var _cart = new Cart();

            Product testProduct = null;

            const int testQuantity = 1;

            // Actsynara
            _cart.AddItem(testProduct, testQuantity);

            // Assert
            Assert.Single(_cart.Lines);
        }

        [Fact]
        public void TestRemoveLine()
        {
            // Arrange
            var _cart = new Cart();

            const int testQuantity = 1;
            const int testProductIndex = 2;

            foreach (var p in _testProductsArr)
            {
                _cart.AddItem(p, testQuantity);
            }

            // Act
            _cart.RemoveLine(_testProductsArr[testProductIndex]);

            // Assert
            Assert.NotNull(_cart.Lines);
            Assert.NotEmpty(_cart.Lines);
            Assert.DoesNotContain(_cart.Lines, l => l.Product?.Id == _testProductsArr[testProductIndex].Id);
        }

        [Fact]
        public void TestReturnTotalValueEmptyCart()
        {
            // Arrange
            var _cart = new Cart();

            const double expected = 0;

            // Act
            double result = _cart.GetTotalValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestGetTotalValueCartSingleItem()
        {
            // Arrange
            var _cart = new Cart();

            int testQuantity = 1;
            double expected = 10;
            _cart.AddItem(_testProduct, testQuantity);

            // Act
            double result = _cart.GetTotalValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestGetTotalValueCartMultipleItems()
        {
            // Arrange
            var _cart = new Cart();

            foreach (var p in _testProductsArr)
            {
                _cart.AddItem(p, 1);
            }

            const double expected = 60;

            // Act
            double result = _cart.GetTotalValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestGetTotalValueCartMultipleItemsVaryingQuantities()
        {
            // Arrange
            var _cart = new Cart();

            for (int i = 0; i < _testProductsArr.Length; i++)
            {
                _cart.AddItem(_testProductsArr[i], i + 1);
            }

            const double expected = 140;

            // Act
            double result = _cart.GetTotalValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestGetAverageValueEmptyCart()
        {
            // Arrange
            var _cart = new Cart();

            const double expected = 0;

            // Act
            double result = _cart.GetAverageValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestGetAverageValueCartSingleItem()
        {
            // Arrange
            var _cart = new Cart();

            var testProduct = new Product
            {
                Id = 1,
                Price = 10
            };

            const int testQuantity = 1;
            const double expected = 10;
            _cart.AddItem(testProduct, testQuantity);

            // Act
            double result = _cart.GetAverageValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestGetAverageValueCartMultipleItems()
        {
            // Arrange
            var _cart = new Cart();

            foreach (var p in _testProductsArr)
            {
                _cart.AddItem(p, 1);
            }

            const double expected = 20;

            // Act
            double result = _cart.GetAverageValue();

            // Assert
            Assert.Equal(expected, result);
    }

        [Fact]
        public void TestGetAverageValueCartMultipleItemsVaryingQuantities()
        {
            // Arrange
            var _cart = new Cart();

            for (int i = 0; i < _testProductsArr.Length; i++)
            {
                _cart.AddItem(_testProductsArr[i], i + 1);
            }

            const double expected = 20;

            // Act
            double result = _cart.GetAverageValue();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
