using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq.AutoMock;
using SSSA.Etl.Domain.Entities;
using SSSA.Etl.Domain.Extraction;
using SSSA.Etl.Domain.Transform;
using SSSA.Etl.Domain.Transform.TransformationStrategies;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace SSSA.Etl.Domain.Tests.Transform
{
    public class TransformerTests
    {
        private readonly AutoMocker _mocker;
        private readonly Transformer _transformer;
        private readonly Faker _faker;
        private readonly Faker<Salesman> _salesmanFaker;
        private readonly Faker<Client> _clientFaker;
        private readonly List<Sale> _sales;

        public TransformerTests()
        {
            _mocker = new AutoMocker();
            _mocker.Use(typeof(CultureInfo), CultureInfo.CreateSpecificCulture("en-US"));
            _transformer = _mocker.CreateInstance<Transformer>();
            _faker = new Faker();
            _salesmanFaker = new Faker<Salesman>()
                .RuleFor(x => x.Cpf, f => f.Person.Cpf())
                .RuleFor(x => x.Name, f => f.Person.FullName)
                .RuleFor(x => x.Salary, f => f.Finance.Amount(1000, 5000));
            _clientFaker = new Faker<Client>()
                .RuleFor(x => x.Cnpj, f => f.Company.Cnpj())
                .RuleFor(x => x.Name, f => f.Company.CompanyName())
                .RuleFor(x => x.BusinessArea, f => f.Company.CatchPhrase());
            _sales = new List<Sale>
            {
                new Sale(_faker.Random.Byte().ToString(), _faker.Person.FullName, new List<SaleItem>
                {
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                }),
                new Sale(_faker.Random.Byte().ToString(), _faker.Person.FullName, new List<SaleItem>
                {
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                }),
                new Sale(_faker.Random.Byte().ToString(), _faker.Person.FullName, new List<SaleItem>
                {
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                    new SaleItem(new Item(_faker.Random.Byte().ToString(), _faker.Finance.Amount(1, 100)), _faker.Random.Byte()),
                }),
            };
        }

        [Fact(DisplayName = "Transformer fail transform when not configured")]
        [Trait("Unit", "Transformer")]
        public void Transformer_NotConfigured_Fail()
        {
            // Arrange
            var validExtractionResult = new ExtractionResult(null, null, null);

            _mocker.GetMock<IStringLocalizer<Transformer>>()
                .Setup(x => x[Transformer.NotConfiguredErrorMessage])
                .Returns(new LocalizedString(Transformer.NotConfiguredErrorMessage, Transformer.NotConfiguredErrorMessage));

            // Act
            var transformationResult = _transformer.Transform(validExtractionResult);

            // Assert
            Assert.False(transformationResult.Succeeded);
        }

        [Fact(DisplayName = "Transformer transforms data successfully")]
        [Trait("Unit", "Transformer")]
        public void Transformer_SucceedTransforming()
        {
            // Arrange
            var validExtractionResult = new ExtractionResult(_salesmanFaker.Generate(4), _clientFaker.Generate(3), _sales);
            _transformer.Configure(new ExpensivestSaleWorstSalesmanTransformationStrategy());

            // Act
            var transformationResult = _transformer.Transform(validExtractionResult);

            // Assert
            Assert.True(transformationResult.Succeeded);
        }

        [Fact(DisplayName = "Transformer fails transforming invalid data")]
        [Trait("Unit", "Transformer")]
        public void Transformer_InvalidInputData_Fail()
        {
            // Arrange
            var invalidExtractionResult = new ExtractionResult(null, null, null);
            _transformer.Configure(new ExpensivestSaleWorstSalesmanTransformationStrategy());

            // Act
            var transformationResult = _transformer.Transform(invalidExtractionResult);

            // Assert
            Assert.False(transformationResult.Succeeded);
        }
    }
}
