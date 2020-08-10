using Microsoft.Extensions.Localization;
using Moq.AutoMock;
using SSSA.Etl.Domain.Load;
using SSSA.Etl.Domain.Load.ReportBuilderStrategies;
using SSSA.Etl.Domain.Load.ReportLoaderStrategies;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SSSA.Etl.Domain.Tests.Load
{
    public class LoaderTests
    {
        private readonly AutoMocker _mocker;
        private readonly Loader _loader;
        private readonly string _validDestination;

        public LoaderTests()
        {
            _mocker = new AutoMocker();
            _mocker.Use(typeof(CultureInfo), CultureInfo.CreateSpecificCulture("en-US"));
            _loader = _mocker.CreateInstance<Loader>();
            _validDestination = "Test Data/out/";
        }

        [Fact(DisplayName = "Loader fail load when not configured")]
        [Trait("Unit", "Loader")]
        public async Task Loader_NotConfigured_Fail()
        {
            // Arrange
            _mocker.GetMock<IStringLocalizer<Loader>>()
                .Setup(x => x[Loader.NotConfiguredErrorMessage])
                .Returns(new LocalizedString(Loader.NotConfiguredErrorMessage, Loader.NotConfiguredErrorMessage));

            // Act
            var loadingResult = await _loader.Load(new object[] { "valid", "data" }, "valid destination");

            // Assert
            Assert.False(loadingResult.Succeeded);
        }

        [Fact(DisplayName = "Loader load data successfully")]
        [Trait("Unit", "Loader")]
        public async Task Loader_SucceedLoading()
        {
            // Arrange
            var reportFileName = "report.txt";
            _loader.Configure(
                    new ToFileLoaderStrategy(reportFileName),
                    new JoinByStringBuilderStrategy("ç"));

            // Act
            var transformationResult = await _loader.Load(new string[] { "this", "is", "valid", "data" }, _validDestination);

            // Assert
            Assert.True(transformationResult.Succeeded);
            Assert.True(File.Exists(transformationResult.Result));
            File.Delete(transformationResult.Result);
        }
    }
}
