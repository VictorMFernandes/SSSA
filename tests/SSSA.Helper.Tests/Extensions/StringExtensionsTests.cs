using SSSA.Helper.Extensions;
using Xunit;

namespace SSSA.Helper.Tests.Extensions
{
    public class StringExtensionsTests
    {
        #region RemoveTexts

        [Theory(DisplayName = "RemoveTexts")]
        [InlineData("TextToBeAltered", "TextAltered", "ToBe")]
        [InlineData("TextToBeAltered", "xoBAlrd", "T", "t", "e")]
        [InlineData("TextToBeAltered", "TextToBeAltered", "z", "y", "w", "")]
        [InlineData("TextToBeAltered", "ToBeAltered", "z", "y", "w", "Text")]
        [InlineData("", "", "A", "a", "B", "b")]
        [InlineData(null, null, "A", "a", "B", "b")]
        [InlineData(null, null, null)]
        [Trait("Unit", "StringExtensions")]
        public void RemoveTexts_Succeed(string text, string expectedResult, params string[] stringsToRemove)
        {
            // Arrange

            // Act
            var result = text.RemoveTexts(stringsToRemove);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        #endregion
    }
}
