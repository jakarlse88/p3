using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CartTests
    {
        public class TheClearMethod
        {
            private readonly Cart _cart;

            public TheClearMethod()
            {
                _cart = new Cart();
            }

            [Fact]
            public void CorrectlyClearsCart()
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
        }

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
                Assert.Equal(testQuantity, actual.Quantity);
            }

            [Fact]
            public void ToleratesNullProductArgument()
            {
                // Arrange
                Product testProduct = null;

                int testQuantity = 1;

                // Actsynara
                _cart.AddItem(testProduct, testQuantity);

                // Assert
                Assert.Single(_cart.Lines);
            }
        }

        public class TheRemoveLineMethod 
        {
            private readonly Cart _cart;

            public TheRemoveLineMethod()
            {
                _cart = new Cart();
            }

            [Fact]
            public void CorrectlyRemovesCartLine()
            {
                // Arrange
                Product[] testProducts = new Product[]
                {
                    new Product { Id = 1 },
                    new Product { Id = 2 },
                    new Product { Id = 3 }
                };

                int testProductIndex = 2;

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
        }

        public class TheGetTotalValueMethod : IDisposable
        {
            private readonly Cart _cart;

            public TheGetTotalValueMethod()
            {
                _cart = new Cart();
            }

            public void Dispose()
            {
                _cart.Clear();
            }

            [Fact]
            public void ReturnsCorrectTotalForEmptyCart()
            {
                // Arrange
                double expected = 0;

                // Act
                double result = _cart.GetTotalValue();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ReturnsCorrectTotalGivenSingleItemInCart()
            {
                // Arrange
                Product testProduct = new Product
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
            public void ReturnsCorrectTotalGivenMultipleItemsInCart()
            {
                // Arrange
                Product[] testProducts = new Product[]
                {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
                };

                foreach (var p in testProducts)
                {
                    _cart.AddItem(p, 1);
                }

                double expected = 60;

                // Act
                double result = _cart.GetTotalValue();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ReturnsCorrectTotalGivenMultipleItemsInCartWithVaryingQuantities()
            {
                // Arrange
                Product[] testProducts = new Product[]
                {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
                };

                for (int i = 0; i < testProducts.Length; i++)
                {
                    _cart.AddItem(testProducts[i], i+1);
                }

                double expected = 140;

                // Act
                double result = _cart.GetTotalValue();

                // Assert
                Assert.Equal(expected, result);
            }
        }

        public class TheGetAverageValueMethod : IDisposable
        {
            private readonly Cart _cart;

            public TheGetAverageValueMethod()
            {
                _cart = new Cart();
            }

            public void Dispose()
            {
                _cart.Clear();
            }

            [Fact]
            public void ReturnsCorrectAverageForEmptyCart()
            {
                // Arrange
                double expected = 0;

                // Act
                double result = _cart.GetAverageValue();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ReturnsCorrectTotalGivenSingleItemInCart()
            {
                // Arrange
                Product testProduct = new Product
                {
                    Id = 1,
                    Price = 10
                };

                int testQuantity = 1;
                double expected = 10;
                _cart.AddItem(testProduct, testQuantity);

                // Act
                double result = _cart.GetAverageValue();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ReturnsCorrectTotalGivenMultipleItemsInCart()
            {
                // Arrange
                Product[] testProducts = new Product[]
                {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
                };

                foreach (var p in testProducts)
                {
                    _cart.AddItem(p, 1);
                }

                double expected = 20;

                // Act
                double result = _cart.GetAverageValue();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ReturnsCorrectTotalGivenMultipleItemsInCartWithVaryingQuantities()
            {
                // Arrange
                Product[] testProducts = new Product[]
                {
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 },
                    new Product { Id = 3, Price = 30 }
                };

                for (int i = 0; i < testProducts.Length; i++)
                {
                    _cart.AddItem(testProducts[i], i + 1);
                }

                double expected = 20;

                // Act
                double result = _cart.GetAverageValue();

                // Assert
                Assert.Equal(expected, result);
            }
        }
    }
}
