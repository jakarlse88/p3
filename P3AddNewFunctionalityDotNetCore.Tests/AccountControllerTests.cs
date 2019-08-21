using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
        private readonly LoginModel _testLoginModel;

        public AccountControllerTests()
        {
            _testLoginModel = new LoginModel
            {
                Name = "name one",
                Password = "password",
                ReturnUrl = "/test"
            };

            var testUsersList = new List<IdentityUser>{
                new IdentityUser {
                    UserName = "name one"
                },
                new IdentityUser {
                    UserName = "name two"
                },
                new IdentityUser {
                    UserName = "name three"
                }
            };

            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<IdentityUser>>().Object,
                new IUserValidator<IdentityUser>[0],
                new IPasswordValidator<IdentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<IdentityUser>>>().Object
                );

            _mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                _mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);

            _mockUserManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => testUsersList
                    .FirstOrDefault(u => u.UserName == name));

            _mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        }

        [Fact]
        public void TestLoginReturnUrl()
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
        public async void TestLoginLoginModel()
        {
            // Arrange
            var accountController =
                new AccountController(_mockUserManager.Object, _mockSignInManager.Object);

            // Act
            var result = await accountController.Login(_testLoginModel);

            // Assert
            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", actionResult.Url);

            _mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);

            _mockSignInManager
                .Verify(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ), Times.Once);
        }

        [Fact]
        public async void TestLoginLoginModelNullReturnUrl()
        {
            // Arrange
            var testLoginModelLocal = new LoginModel
            {
                Name = _testLoginModel.Name,
                Password = _testLoginModel.Password,
                ReturnUrl = null
            };

            var accountController =
                new AccountController(_mockUserManager.Object, _mockSignInManager.Object);

            // Act
            var result = await accountController.Login(testLoginModelLocal);

            // Assert
            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/Admin/Index", actionResult.Url);

            _mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);

            _mockSignInManager
                .Verify(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ), Times.Once);
        }

        [Fact]
        public async void TestLoginLoginModelNullName()
        {
            // Arrange
            var testLoginModelLocal = new LoginModel
            {
                Name = null,
                Password = _testLoginModel.Password,
            };

            var accountController =
                new AccountController(_mockUserManager.Object, _mockSignInManager.Object);

            // Act
            var result = await accountController.Login(testLoginModelLocal);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<LoginModel>(viewResult.Model);
            Assert.False(accountController.ModelState.IsValid);
        }

        [Fact]
        public async void TestLogoutReturnUrl()
        {
            // Arrange
            var accountController = new AccountController(null, _mockSignInManager.Object);

            // Act
            var result = await accountController.Logout("/test");

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", redirectResult.Url);

            _mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async void TestLogoutDefaultArg()
        {
            // Arrange
            var accountController = new AccountController(null, _mockSignInManager.Object);

            // Act
            var result = await accountController.Logout();

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);

            _mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);
        }
    }
}