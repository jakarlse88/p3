using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void IndexReturnsAViewOfProducts()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();

            var mockListProductViewModels = new List<ProductViewModel>
            {
                new ProductViewModel
                {
                    Id = 1,
                    Name = "one name",
                    Description = "one description",
                    Details = "one details",
                    Stock = "1",
                    Price = "10"
                },
                new ProductViewModel
                {
                    Id = 2,
                    Name = "two name",
                    Description = "two description",
                    Details = "two details",
                    Stock = "2",
                    Price = "20"
                },
                new ProductViewModel
                {
                    Id = 3,
                    Name = "three name",
                    Description = "three description",
                    Details = "three details",
                    Stock = "3",
                    Price = "30"
                }
            };

            mockProductService
                .Setup(x => x.GetAllProductsViewModel())
                .Returns(mockListProductViewModels);

            var productController = new ProductController(mockProductService.Object, null);

            // Act
            var result = productController.Index();

            // Assert
            mockProductService.Verify(x => x.GetAllProductsViewModel(), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void AdminWorksAsExpected() 
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();

            var mockListProductViewModels = new List<ProductViewModel>
            {
                new ProductViewModel
                {
                    Id = 1,
                    Name = "one name",
                    Description = "one description",
                    Details = "one details",
                    Stock = "1",
                    Price = "10"
                },
                new ProductViewModel
                {
                    Id = 2,
                    Name = "two name",
                    Description = "two description",
                    Details = "two details",
                    Stock = "2",
                    Price = "20"
                },
                new ProductViewModel
                {
                    Id = 3,
                    Name = "three name",
                    Description = "three description",
                    Details = "three details",
                    Stock = "3",
                    Price = "30"
                }
            };

            mockProductService
                .Setup(x => x.GetAllProductsViewModel())
                .Returns(mockListProductViewModels);

            var productController = new ProductController(mockProductService.Object, null);

            // Act
            var result = productController.Admin();

            // Assert       
            mockProductService.Verify(x => x.GetAllProductsViewModel(), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());     
        }

        [Fact]
        public void CreateRedirectsToAdminActionGivenValidModelState()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();

            var testProductViewModel = new ProductViewModel
            {
                Id = 1,
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Stock = "1",
                Price = "10"
            };

            mockProductService
                .Setup(x => x.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns((ProductViewModel product) => new List<string>());

            mockProductService
                .Setup(x => x.SaveProduct(It.IsAny<ProductViewModel>()));

            var productController = new ProductController(mockProductService.Object, null);

            // Act
            var result = productController.Create(testProductViewModel);

            // Assert
            mockProductService
                .Verify(x => x.SaveProduct(It.IsAny<ProductViewModel>()), Times.Once);
            
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", actionResult.ActionName);   
        }

        [Fact]
        public void CreateReturnsViewProductGivenInvalidModelState()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "MissingName" });

            var testProductViewModel = new ProductViewModel
            {
                Id = 1,
                Name = null,
                Description = "one description",
                Details = "one details",
                Stock = "1",
                Price = "10"
            };

            var productController = new ProductController(mockProductService.Object, null);

            // Act
            var result = productController.Create(testProductViewModel);

            // Assert
            mockProductService
                .Verify(x => x.CheckProductModelErrors(It.IsAny<ProductViewModel>()), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);
        }

        [Fact]
        public void DeleteProductRedirectsToAdminActionGivenValidArg()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            mockProductService
                .Setup(x => x.DeleteProduct(It.IsAny<int>()));

            var productController = new ProductController(mockProductService.Object, null);
            
            // Act
            var result = productController.DeleteProduct(1);

            // Assert
            mockProductService.Verify(x => x.DeleteProduct(It.IsAny<int>()), Times.Once);
            
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", actionResult.ActionName);
        }
    }
}
