using SSSA.Helper.Extensions;
using System.Drawing;
using Xunit;

namespace SSSA.Helper.Tests.Extensions
{
    public class IEnumerableExtensionsTests
    {
        #region Higher

        [Theory(DisplayName = "Higher with primitive collection")]
        [InlineData("thirdParam", "secondParam", "firstParam")]
        [InlineData(3, 2, 1)]
        [InlineData(3, 1, 1)]
        [InlineData(null, null, null)]
        [Trait("Unit", "IEnumerableExtensions")]
        public void Higher_WithPrimitiveCollection_Succeed(object firstParam, object secondParam, object thirdParam)
        {
            // Arrange
            var collection = new object[] { firstParam, secondParam, thirdParam };

            // Act
            var result = collection.Higher(x => x);

            // Assert
            Assert.Equal(firstParam, result);
        }

        [Theory(DisplayName = "Higher with complex collection")]
        [InlineData(50, 15, 10)]
        [InlineData(50, 15, 15)]
        [InlineData(50, 10, null)]
        [InlineData(null, null, null)]
        [Trait("Unit", "IEnumerableExtensions")]
        public void Higher_WithComplexCollection_Succeed(int firstParam, int secondParam, int thirdParam)
        {
            // Arrange
            var higherXRectangle = new Rectangle(firstParam, 10, 20, 24);
            var collection = new Rectangle[] { higherXRectangle, new Rectangle(secondParam, 10, 20, 24), new Rectangle(thirdParam, 10, 20, 24) };

            // Act
            var result = collection.Higher(x => x.X);

            // Assert
            Assert.Equal(higherXRectangle, result);
        }

        #endregion

        #region Lower

        [Theory(DisplayName = "Lower with primitive collection")]
        [InlineData("thirdParam", "secondParam", "firstParam")]
        [InlineData(3, 2, 1)]
        [InlineData(3, 1, 1)]
        [InlineData(null, null, null)]
        [Trait("Unit", "IEnumerableExtensions")]
        public void Lower_WithPrimitiveCollection_Succeed(object firstParam, object secondParam, object thirdParam)
        {
            // Arrange
            var collection = new object[] { firstParam, secondParam, thirdParam };

            // Act
            var result = collection.Lower(x => x);

            // Assert
            Assert.Equal(thirdParam, result);
        }

        [Theory(DisplayName = "Lower with complex collection")]
        [InlineData(50, 15, 10)]
        [InlineData(50, 15, 15)]
        [InlineData(50, 10, null)]
        [InlineData(null, null, null)]
        [Trait("Unit", "IEnumerableExtensions")]
        public void Lower_WithComplexCollection_Succeed(int firstParam, int secondParam, int thirdParam)
        {
            // Arrange
            var lowerXRectangle = new Rectangle(thirdParam, 10, 20, 24);
            var collection = new Rectangle[] { lowerXRectangle, new Rectangle(secondParam, 10, 20, 24), new Rectangle(firstParam, 10, 20, 24) };

            // Act
            var result = collection.Lower(x => x.X);

            // Assert
            Assert.Equal(lowerXRectangle, result);
        }

        #endregion
    }
}
