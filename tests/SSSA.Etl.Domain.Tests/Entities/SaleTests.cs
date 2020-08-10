using Bogus;
using SSSA.Etl.Domain.Entities;
using Xunit;

namespace SSSA.Etl.Domain.Tests.Entities
{
    public class SaleTests
    {
        private readonly Faker _faker;
        public SaleTests()
        {
            _faker = new Faker();
        }

        [Theory(DisplayName = "Sale total calculated correctly")]
        [InlineData(5, 2, 10, 5, 60)]
        [InlineData(5, 1, 10, 13, 135)]
        [InlineData(0, 2, 5, 5, 25)]
        [InlineData(5, 2, 0, 5, 10)]
        [Trait("Unit", "Sale")]
        public void Sale_TotalCalculatedCorrectly(
            decimal firstItemPrice,
            int firstQuantity,
            decimal secondItemPrice,
            int secondQuantity,
            decimal expectedTotal)
        {
            // Arrange
            var firstItem = new Item(_faker.Random.Byte().ToString(), firstItemPrice);
            var secondItem = new Item(_faker.Random.Byte().ToString(), secondItemPrice);
            var firstSaleItem = new SaleItem(firstItem, firstQuantity);
            var secondSaleItem = new SaleItem(secondItem, secondQuantity);
            var sale = new Sale(_faker.Random.Byte().ToString(), _faker.Name.FullName(), new SaleItem[] { firstSaleItem, secondSaleItem });

            // Act
            var total = sale.Total;

            // Assert
            Assert.Equal(expectedTotal, total);
        }
    }
}
