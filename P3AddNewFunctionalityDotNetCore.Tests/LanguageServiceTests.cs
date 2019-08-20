using System;
using System.Collections.Generic;
using System.Text;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class LanguageServiceTests
    {
        [Theory]
        [InlineData("English", "en")]
        [InlineData("French", "fr")]
        [InlineData("Spanish", "es")]
        public void TestSetCultureValidArgs(string language, string expected)
        {
            // Arrange
            var languageService = new LanguageService();

            // Act
            var result = languageService.SetCulture(language);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("garbage", "en")]
        [InlineData(null, "en")]
        [InlineData("3ngl1sh", "en")]
        [InlineData("\nenglish\t", "en")]
        [InlineData("     english      ", "en")]
        public void TestSetcultureInvalidArgs(string language, string expected)
        {
            // Arrange
            var languageService = new LanguageService();

            // Act
            var result = languageService.SetCulture(language);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
