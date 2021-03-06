using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class LanguageControllerTests
    {
        private readonly Mock<ILanguageService> _mockLanguageService;

        public LanguageControllerTests()
        {
            _mockLanguageService = new Mock<ILanguageService>();

            _mockLanguageService
                .Setup(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()));
        }

        [Fact]
        public void TestChangeUiLanguageValidArgs()
        {
            // Arrange
            var languageController = new LanguageController(_mockLanguageService.Object);

            var testModel = new LanguageViewModel
            {
                Language = "English"
            };

            const string testReturnUrl = "/test";

            // Act
            var result = languageController.ChangeUiLanguage(testModel, testReturnUrl);

            // Assert
            _mockLanguageService
                .Verify(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Once);

            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", actionResult.Url);
        }

        [Fact]
        public void TestChangeUiLanguageInvalidModelArg()
        {
            // Arrange
            var languageController = new LanguageController(_mockLanguageService.Object);

            var testModel = new LanguageViewModel
            {
                Language = null
            };

            const string testReturnUrl = "/test";

            // Act
            var result = languageController.ChangeUiLanguage(testModel, testReturnUrl);

            // Assert
            _mockLanguageService
                .Verify(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never);

            var actionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/test", actionResult.Url);
        }

        [Fact]
        public void TestChangeUiLanguageNullModelArg()
        {
            // Arrange
            var languageController = new LanguageController(_mockLanguageService.Object);

            const string testReturnUrl = "/test";

            // Act
            void TestAction() => languageController.ChangeUiLanguage(null, testReturnUrl);

            // Assert
            _mockLanguageService
                .Verify(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never);

            Assert.Throws<NullReferenceException>(TestAction);
        }
        
        [Fact]
        public void TestChangeUiLanguageNullReturnUrlArg()
        {
            // Arrange
            var languageController = new LanguageController(_mockLanguageService.Object);

            var testModel = new LanguageViewModel
            {
                Language = "English"
            };
            
            // Act
            void TestAction() => languageController.ChangeUiLanguage(testModel, null);

            // Assert
            _mockLanguageService
                .Verify(x => x.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never);

            Assert.Throws<ArgumentException>(TestAction);
        }
    }
}