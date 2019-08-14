using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        public async void LoginWorksAsExpectedGivenValidArgs()
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

            var mockUserManager = new Mock<UserManager<IdentityUser>>(
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

            mockUserManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(testUser);

            var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);

            mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            mockSignInManager
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var accountController = 
                new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Act
            var result = await accountController.Login(loginModel);

            // Assert
            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", actionResult.Url);

            mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);

            mockSignInManager
                .Verify(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ), Times.Once);
        }

        [Fact]
        public async void LoginWorksAsExpectedRedirectsDefaultGivenValidArgsNullReturnUrl()
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
                ReturnUrl = null
            };

            var mockUserManager = new Mock<UserManager<IdentityUser>>(
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

            mockUserManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(testUser);

            var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);

            mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            mockSignInManager
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var accountController = 
                new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Act
            var result = await accountController.Login(loginModel);

            // Assert
            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/Admin/Index", actionResult.Url);

            mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);

            mockSignInManager
                .Verify(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ), Times.Once);
        }

        [Fact]
        public async void LoginFailsGivenUserNull()
        {
            // Arrange
            var testUser = new IdentityUser
            {
                Id = "1",
                NormalizedUserName = "NormalizedName"
            };

            var testUserList = new List<IdentityUser>{
                new IdentityUser {
                    UserName = "one"
                },
                new IdentityUser {
                    UserName = "two"
                },
                new IdentityUser {
                    UserName = "three"
                }
            };

            var loginModel = new LoginModel 
            { 
                Name = "name", 
                Password = "password", 
                ReturnUrl = null
            };

            var mockUserManager = new Mock<UserManager<IdentityUser>>(
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

            mockUserManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => testUserList.FirstOrDefault(u => u.UserName == loginModel.Name));

            var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);

            mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            mockSignInManager
                .Setup(x => x.PasswordSignInAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var accountController = 
                new AccountController(mockUserManager.Object, mockSignInManager.Object);

            // Act
            var result = await accountController.Login(loginModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginModel>(viewResult.Model);
            Assert.False(accountController.ModelState.IsValid);
        }

        [Fact]
        public async void LogoutWorksAsExpectedRedirectsToReturnUrl()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<IdentityUser>>(
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

            var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object
                );

            mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            var accountController = new AccountController(null, mockSignInManager.Object);

            // Act
            var result = await accountController.Logout("/test");

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", redirectResult.Url);

            mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async void LogoutWorkAsExpectedDefaultArg()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<IdentityUser>>(
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

            var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object
                );

            mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            var accountController = new AccountController(null, mockSignInManager.Object);

            // Act
            var result = await accountController.Logout();

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);

            mockSignInManager
                .Verify(x => x.SignOutAsync(), Times.Once);
        }
    } 
}