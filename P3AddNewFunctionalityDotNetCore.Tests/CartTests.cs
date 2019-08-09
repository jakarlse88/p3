using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CartTests : IDisposable
    {
        private readonly Cart _cart;

        public CartTests()
        {
            _cart = new Cart();
        }

        public void Dispose()
        {
            _cart.Clear();
        }

        [Fact]
        public void ClearCorrectlyClearsCart()
        {
            // Arrange
            Product[] testProducts = new Product[]
            {
                    new Product { Id = 1 },
                    new Product { Id = 2 },
                    new Product { Id = 3 }
            };


            foreach (var p in testProducts)
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
        public void AddItemCorrectlyAddsSingleProductToCart()
        {
            // Arrange
            var testProduct = new Product
            {
                Name = "TestObject",
                Price = 10D,
                Quantity = 4,
                Description = "TestDescription",
                Details = "TestDetails"
            };

            const int testQuantity = 1;

            // Act
            _cart.AddItem(testProduct, testQuantity);

            // Assert
            Assert.NotEmpty(_cart.Lines);
            Assert.Contains(_cart.Lines, l => l.Product.Name == "TestObject");
        }

        [Fact]
        public void AddItemIncrementsAlreadyPresentProductInCart()
        {
            // Arrange
            var testProduct = new Product
            {
                Name = "TestObject",
                Price = 10D,
                Quantity = 4,
                Description = "TestDescription",
                Details = "TestDetails"
            };

            const int initialQuantity = 1;
            const int incrementQuantity = 2;
            const int expectedTotalQuantity = initialQuantity + incrementQuantity;

            // Act
            _cart.AddItem(testProduct, initialQuantity);
            _cart.AddItem(testProduct, incrementQuantity);

            // Assert
            var actual = _cart.Lines.FirstOrDefault(l => l.Product.Name == "TestObject");

            Assert.Single(_cart.Lines);
            Assert.Equal(expectedTotalQuantity, actual.Quantity);
        }

        [Fact]
        public void AddItemDecrementsQuantityGivenNegativeQuantityExceedingStock()
        {
            // Arrange
            var testProduct = new Product
            {
                Name = "TestObject",
                Price = 10D,
                Quantity = 4,
                Description = "TestDescription",
                Details = "TestDetails"
            };

            const int testQuantity = -5;

            // Act
            _cart.AddItem(testProduct, testQuantity);

            // Assert
            CartLine actual = _cart.Lines.FirstOrDefault(l => l.Product.Name == "TestObject");

            Assert.Single(_cart.Lines);
            Assert.Equal(testQuantity, actual.Quantity);
        }

        [Fact]
        public void AddItemToleratesNullProductArgument()
        {
            // Arrange
            Product testProduct = null;

            const int testQuantity = 1;

            // Actsynara
            _cart.AddItem(testProduct, testQuantity);

            // Assert
            Assert.Single(_cart.Lines);
        }

        [Fact]
        public void RemoveLineCorrectlyRemovesCartLine()
        {
            // Arrange
            var testProducts = new Product[]
            {
                    new Product { Id = 1 },
                    new Product { Id = 2 },
                    new Product { Id = 3 }
            };

            const int testProductIndex = 2;

            foreach (var p in testProducts)
            {
                _cart.AddItem(p, 1);
            }

            // Act
            _cart.RemoveLine(testProducts[testProductIndex]);

            // Assert
            Assert.NotNull(_cart.Lines);
            Assert.NotEmpty(_cart.Lines);
            Assert.DoesNotContain(_cart.Lines, l => l.Product?.Id == testProducts[testProductIndex].Id);
        }

        [Fact]
        public void GetTotalValueReturnsCorrectTotalForEmptyCart()
        {
            // Arrange
            const double expected = 0;

            // Act
            double result = _cart.GetTotalValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetTotalValueReturnsCorrectTotalGivenSingleItemInCart()
        {
            // Arrange
            var testProduct = new Product
            {
                Id = 1,
                Price = 10
            };

            int testQuantity = 1;
            double expected = 10;
            _cart.AddItem(testProduct, testQuantity);

            // Act
            double result = _cart.GetTotalValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetTotalValueReturnsCorrectTotalGivenMultipleItemsInCart()
        {
            // Arrange
            var testProducts = new Product[]
            {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
            };

            foreach (var p in testProducts)
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
        public void GetTotalValueReturnsCorrectTotalGivenMultipleItemsInCartWithVaryingQuantities()
        {
            // Arrange
            var testProducts = new Product[]
            {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
            };

            for (int i = 0; i < testProducts.Length; i++)
            {
                _cart.AddItem(testProducts[i], i + 1);
            }

            const double expected = 140;

            // Act
            double result = _cart.GetTotalValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAverageValueReturnsCorrectAverageForEmptyCart()
        {
            // Arrange
            const double expected = 0;

            // Act
            double result = _cart.GetAverageValue();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAverageValueReturnsCorrectTotalGivenSingleItemInCart()
        {
            // Arrange
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
        public void GetAverageValueReturnsCorrectTotalGivenMultipleItemsInCart()
        {
            // Arrange
            var testProducts = new Product[]
            {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
            };

            foreach (var p in testProducts)
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
        public void GetAverageValueReturnsCorrectTotalGivenMultipleItemsInCartWithVaryingQuantities()
        {
            // Arrange
            var testProducts = new Product[]
            {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
            };

            for (int i = 0; i < testProducts.Length; i++)
            {
                _cart.AddItem(testProducts[i], i + 1);
            }

            const double expected = 20;

            // Act
            double result = _cart.GetAverageValue();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
