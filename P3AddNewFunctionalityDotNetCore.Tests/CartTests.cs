using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CartTests
    {
        public class TheAddItemMethod : IDisposable
        {
            private readonly Cart _cart;

            public TheAddItemMethod()
            {
                _cart = new Cart();
            }

            public void Dispose()
            {
                _cart.Clear();
            }

            [Fact]
            public void AddsSingleProductToCart()
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

                int testQuantity = 1;

                // Act
                _cart.AddItem(testProduct, testQuantity);

                // Assert
                Assert.NotEmpty(_cart.Lines);
                Assert.Contains<CartLine>(_cart.Lines, l => l.Product.Name == "TestObject");
            }

            [Fact]
            public void IncrementsAlreadyPresentProductInCart()
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

                int initialQuantity = 1;
                int incrementQuantity = 2;
                int expectedTotalQuantity = initialQuantity + incrementQuantity;

                // Act
                _cart.AddItem(testProduct, initialQuantity);
                _cart.AddItem(testProduct, incrementQuantity);

                // Assert
                CartLine actual = _cart.Lines.FirstOrDefault(l => l.Product.Name == "TestObject");

                Assert.Single(_cart.Lines);
                Assert.Equal(expectedTotalQuantity, actual.Quantity);
            }

            [Fact]
            public void DecrementsQuantityGivenNegativeQuantityExceedingStock()
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

                int testQuantity = -5;

                // Act
                _cart.AddItem(testProduct, testQuantity);

                // Assert
                CartLine actual = _cart.Lines.FirstOrDefault(l => l.Product.Name == "TestObject");
                
                Assert.Single(_cart.Lines);
                Assert.Equal(testQuantity,actual.Quantity);
            }

            [Fact]
            public void ToleratesNullProductArgument()
            {
                // Arrange
                Product testProduct = null;

                int testQuantity = 1;

                // Act
                _cart.AddItem(testProduct, testQuantity);

                // Assert
                Assert.Single(_cart.Lines);
            }
        }
    }
}
