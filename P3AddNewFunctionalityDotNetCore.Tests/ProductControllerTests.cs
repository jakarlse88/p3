using System.Collections.Generic;
using System.Linq;
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
        private readonly IEnumerable<ProductViewModel> _testProductViewModelsList;
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductViewModel _testProductViewModel;

        public ProductControllerTests()
        {
            _testProductViewModelsList = new List<ProductViewModel>
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

            _testProductViewModel = _testProductViewModelsList.First();

            _mockProductService = new Mock<IProductService>();
            
            _mockProductService
                .Setup(x => x.GetAllProductsViewModel())
                .Returns(_testProductViewModelsList.ToList());

            _mockProductService
                .Setup(x => x.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns((ProductViewModel product) => new List<string>());

            _mockProductService
                .Setup(x => x.SaveProduct(It.IsAny<ProductViewModel>()));

            _mockProductService
                .Setup(x => x.DeleteProduct(It.IsAny<int>()));
        }

        [Fact]
        public void IndexReturnsAViewOfProducts()
        {
            // Arrange
            var productController = new ProductController(_mockProductService.Object, null);

            // Act
            var result = productController.Index();

            // Assert
            _mockProductService.Verify(x => x.GetAllProductsViewModel(), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void AdminWorksAsExpected() 
        {
            // Arrange
            var productController = new ProductController(_mockProductService.Object, null);

            // Act
            var result = productController.Admin();

            // Assert       
            _mockProductService.Verify(x => x.GetAllProductsViewModel(), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());     
        }

        [Fact]
        public void CreateRedirectsToAdminActionGivenValidModelState()
        {
            // Arrange
            var productController = new ProductController(_mockProductService.Object, null);

            // Act
            var result = productController.Create(_testProductViewModel);

            // Assert
            _mockProductService
                .Verify(x => x.SaveProduct(It.IsAny<ProductViewModel>()), Times.Once);
            
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", actionResult.ActionName);   
        }

        [Fact]
        public void CreateReturnsViewProductGivenInvalidModelState()
        {
            // Arrange
            _mockProductService
                .Setup(x => x.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "MissingName" });

            var productController = new ProductController(_mockProductService.Object, null);

            // Act
            var result = productController.Create(_testProductViewModel);

            // Assert
            _mockProductService
                .Verify(x => x.CheckProductModelErrors(It.IsAny<ProductViewModel>()), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);
        }

        [Fact]
        public void DeleteProductRedirectsToAdminActionGivenValidArg()
        {
            // Arrange
            var productController = new ProductController(_mockProductService.Object, null);
            
            // Act
            var result = productController.DeleteProduct(1);

            // Assert
            _mockProductService.Verify(x => x.DeleteProduct(It.IsAny<int>()), Times.Once);
            
            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", actionResult.ActionName);
        }
    }
}
