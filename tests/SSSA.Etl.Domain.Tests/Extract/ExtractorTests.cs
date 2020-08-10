using Microsoft.Extensions.Localization;
using Moq.AutoMock;
using SSSA.Etl.Domain.Extract;
using SSSA.Etl.Domain.Extract.EntityIdentifierStrategies;
using SSSA.Etl.Domain.Extract.PropertyExtractorStrategies;
using SSSA.Etl.Domain.Extract.RecordExtractorStrategies;
using SSSA.Etl.Domain.Extract.SaleItemExtractorStrategies;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace SSSA.Etl.Domain.Tests.Extract
{
    public class ExtractorTests
    {
        private readonly AutoMocker _mocker;
        private readonly Extractor _extractor;
        private readonly string _validSource;

        public ExtractorTests()
        {
            _mocker = new AutoMocker();
            _mocker.Use(typeof(CultureInfo), CultureInfo.CreateSpecificCulture("en-US"));
            _extractor = _mocker.CreateInstance<Extractor>();
            _validSource = "Test Data/in/rawData.txt";
        }

        [Fact(DisplayName = "Extractor fail extraction when not configured")]
        [Trait("Unit", "Extractor")]
        public async Task Extractor_NotConfigured_Fail()
        {
            // Arrange
            _mocker.GetMock<IStringLocalizer<Extractor>>()
                .Setup(x => x[Extractor.NotConfiguredErrorMessage])
                .Returns(new LocalizedString(Extractor.NotConfiguredErrorMessage, Extractor.NotConfiguredErrorMessage));

            // Act
            var extractionResult = await _extractor.ExtractAsync(_validSource);

            // Assert
            Assert.False(extractionResult.Succeeded);
        }

        [Fact(DisplayName = "Extractor extracts from source successfully")]
        [Trait("Unit", "Extractor")]
        public async Task Extractor_SucceedExtracting()
        {
            // Arrange
            _extractor.Configure(
                    new FromFileLineRecordExtractorStrategy(),
                    new SplitByStringPropertyExtractorStrategy("ç"),
                    new PropertyIndexEntityIdentifierStrategy(0),
                    new SplitByStringSaleItemExtractorStrategy(",", "-", "[", "]"));

            // Act
            var extractionResult = await _extractor.ExtractAsync(_validSource);

            // Assert
            Assert.True(extractionResult.Succeeded);
        }
    }
}
