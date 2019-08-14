using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class LanguageControllerTests 
    {
        [Fact]
        public void ChangeUiLanguageChangesLanguageRedirectsGivenValidArgs()
        {
            // Arrange
            var mockLanguageService = new Mock<ILanguageService>();
            mockLanguageService
                .Setup(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()));

            var languageController = new LanguageController(mockLanguageService.Object);
       
            var testModel = new LanguageViewModel 
            {
                Language = "English"
            };

            string testReturnUrl = "/test";

            // Act
            var result = languageController.ChangeUiLanguage(testModel, testReturnUrl);

            // Assert
            mockLanguageService
                .Verify(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Once);

            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", actionResult.Url);
        }

        [Fact]
        public void ChangeUiLanguageOnlyRedirectsGivenInvalidArg()
        {
            // Arrange
            var mockLanguageService = new Mock<ILanguageService>();
            mockLanguageService
                .Setup(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()));

            var languageController = new LanguageController(mockLanguageService.Object);
       
            var testModel = new LanguageViewModel 
            {
                Language = null
            };

            string testReturnUrl = "/test";

            // Act
            var result = languageController.ChangeUiLanguage(testModel, testReturnUrl);

            // Assert
            mockLanguageService
                .Verify(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never);

            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", actionResult.Url);
        }

        [Fact]
        public void ChangeUiLanguageThrowsGivenNullModelArg()
        {
            // Arrange
            var mockLanguageService = new Mock<ILanguageService>();
            mockLanguageService
                .Setup(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()));

            var languageController = new LanguageController(mockLanguageService.Object);
       
            LanguageViewModel testModel = null;

            string testReturnUrl = "/test";

            // Act
            Action testAction = () => languageController.ChangeUiLanguage(testModel, testReturnUrl);

            // Assert
            mockLanguageService
                .Verify(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never);

            Assert.Throws<NullReferenceException>(testAction);
        }
    }
}