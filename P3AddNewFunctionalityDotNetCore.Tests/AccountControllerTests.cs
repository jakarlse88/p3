using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public void LoginReturnsViewWithCorrectReturnUrl()
        {
            // Arrange
            var accountController = new AccountController(null, null);

            // Act
            var result = accountController.Login("/test");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginModel>(viewResult.Model);
            Assert.Equal("/test", model.ReturnUrl);
        }

        [Fact]
        public void TestName()
        {
            // Arrange
            var testUser = new IdentityUser
            {
                Id = "1",
                NormalizedUserName = "NormalizedName"
            };

            var loginModel = new LoginModel 
            { 
                Name = "Name", 
                Password = "Password", 
                ReturnUrl = "/test" 
            };

            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            mockUserManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(testUser);

            var mockSignInManager = new Mock<SignInManager<IdentityUser>>();
            mockSignInManager
                .Setup(x => x.SignOutAsync()); // Maybe this is fine? If it doesn|t need to actually do anything.

            mockSignInManager
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
                // This is where we|re at

        }
    } 
}