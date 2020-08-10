using Bogus;
using SSSA.Etl.Domain.Entities;
using Xunit;

namespace SSSA.Etl.Domain.Tests.Entities
{
    public class SaleItemTests
    {
        private readonly Faker _faker;
        public SaleItemTests()
        {
            _faker = new Faker();
        }

        [Theory(DisplayName = "Sale item value calculated correctly")]
        [InlineData(5, 2, 10)]
        [InlineData(1000, 5, 5000)]
        [InlineData(100, 0, 0)]
        [InlineData(0, 0, 0)]
        [Trait("Unit", "Sale Item")]
        public void SaileItem_ValueCalculatedCorrectly(decimal itemPrice, int quantity, decimal expectedValue)
        {
            // Arrange
            var item = new Item(_faker.Random.Byte().ToString(), itemPrice);
            var saleItem = new SaleItem(item, quantity);

            // Act
            var value = saleItem.Value;

            // Assert
            Assert.Equal(expectedValue, value);
        }
    }
}
