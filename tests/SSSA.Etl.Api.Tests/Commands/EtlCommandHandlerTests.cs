using Moq;
using Moq.AutoMock;
using SSSA.Core.Api.Communication.Errors;
using SSSA.Core.Api.Communication.Mediator;
using SSSA.Etl.Api.Commands;
using SSSA.Etl.Domain.Extract;
using SSSA.Etl.Domain.Extract.EntityIdentifierStrategies;
using SSSA.Etl.Domain.Extract.RecordExtractorStrategies;
using SSSA.Etl.Domain.Extract.SaleItemExtractorStrategies;
using SSSA.Etl.Domain.Extraction;
using SSSA.Etl.Domain.ExtractionStrategies.Property;
using SSSA.Etl.Domain.Load;
using SSSA.Etl.Domain.Load.ReportBuilderStrategies;
using SSSA.Etl.Domain.Load.ReportLoaderStrategies;
using SSSA.Etl.Domain.Transform;
using SSSA.Etl.Domain.Transform.TransformationStrategies;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SSSA.Etl.Api.Tests.Commands
{
    public class EtlCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly EtlCommandHandler _etlCommandHandler;

        public EtlCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _mocker.Use(typeof(CultureInfo), CultureInfo.CreateSpecificCulture("en-US"));
            _etlCommandHandler = _mocker.CreateInstance<EtlCommandHandler>();
        }

        [Fact(DisplayName = "Create sales report successfully")]
        [Trait("Unit", "Etl Commands")]
        public async Task CreateSalesReportCommand_Succeeded()
        {
            // Arrange
            var request = new CreateSalesReportCommand("validOutput", new string[] { "validInput" });
            var succeededExtractionResult = new ExtractionResult(null, null, null);
            var succeededTransformationResult = new TransformationResult(new string[] { });
            var succeededLoadResult = new LoadResult("validResult");

            _mocker.GetMock<IExtractor>()
                .Setup(x => x.ExtractAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(succeededExtractionResult));

            _mocker.GetMock<ITransformer>()
                .Setup(x => x.Transform(It.IsAny<ExtractionResult>()))
                .Returns(succeededTransformationResult);

            _mocker.GetMock<ILoader>()
                .Setup(x => x.Load(It.IsAny<IEnumerable<object>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(succeededLoadResult));

            // Act
            var result = await _etlCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IRecordExtractorStrategy>(),
                    It.IsAny<IPropertyExtractorStrategy>(),
                    It.IsAny<IEntityIdentifierStrategy>(),
                    It.IsAny<ISaleItemExtractorStrategy>()), Times.Once);
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.ExtractAsync(
                    It.IsAny<string>()), Times.Once);

            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<ITransformationStrategy>()), Times.Once);
            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Transform(
                    It.IsAny<ExtractionResult>()), Times.Once);

            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IReportLoaderStrategy>(),
                    It.IsAny<IReportBuilderStrategy>()), Times.Once);
            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Load(
                    It.IsAny<IEnumerable<object>>(),
                    It.IsAny<string>()), Times.Once);

            _mocker.GetMock<IMediatorHandler>()
                   .Verify(
                r => r.NotifyErrorAsync(
                    It.IsAny<ErrorNotification>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "Create sales report extraction failed")]
        [Trait("Unit", "Etl Commands")]
        public async Task CreateSalesReportCommand_WithFailedExtraction_Failed()
        {
            // Arrange
            var request = new CreateSalesReportCommand("validOutput", new string[] { "validInput" });
            var failedExtractionResult = new ExtractionResult("Error message");
            var succeededTransformationResult = new TransformationResult(new string[] { });
            var succeededLoadResult = new LoadResult("validResult");

            _mocker.GetMock<IExtractor>()
                .Setup(x => x.ExtractAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(failedExtractionResult));

            _mocker.GetMock<ITransformer>()
                .Setup(x => x.Transform(It.IsAny<ExtractionResult>()))
                .Returns(succeededTransformationResult);

            _mocker.GetMock<ILoader>()
                .Setup(x => x.Load(It.IsAny<IEnumerable<object>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(succeededLoadResult));

            // Act
            var result = await _etlCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IRecordExtractorStrategy>(),
                    It.IsAny<IPropertyExtractorStrategy>(),
                    It.IsAny<IEntityIdentifierStrategy>(),
                    It.IsAny<ISaleItemExtractorStrategy>()), Times.Once);
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.ExtractAsync(
                    It.IsAny<string>()), Times.Once);

            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<ITransformationStrategy>()), Times.Never);
            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Transform(
                    It.IsAny<ExtractionResult>()), Times.Never);

            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IReportLoaderStrategy>(),
                    It.IsAny<IReportBuilderStrategy>()), Times.Never);
            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Load(
                    It.IsAny<IEnumerable<object>>(),
                    It.IsAny<string>()), Times.Never);

            _mocker.GetMock<IMediatorHandler>()
                   .Verify(
                r => r.NotifyErrorAsync(
                    It.Is<ErrorNotification>(x =>
                        x.Key == nameof(Extractor) &&
                        x.Value == failedExtractionResult.ErrorMessage)), Times.Once);
            Assert.False(result);
        }

        [Fact(DisplayName = "Create sales report transformation failed")]
        [Trait("Unit", "Etl Commands")]
        public async Task CreateSalesReportCommand_WithFailedTransformation_Failed()
        {
            // Arrange
            var request = new CreateSalesReportCommand("validOutput", new string[] { "validInput" });
            var succeededExtractionResult = new ExtractionResult(null, null, null);
            var failedTransformationResult = new TransformationResult("Error message");
            var succeededLoadResult = new LoadResult("validResult");

            _mocker.GetMock<IExtractor>()
                .Setup(x => x.ExtractAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(succeededExtractionResult));

            _mocker.GetMock<ITransformer>()
                .Setup(x => x.Transform(It.IsAny<ExtractionResult>()))
                .Returns(failedTransformationResult);

            _mocker.GetMock<ILoader>()
                .Setup(x => x.Load(It.IsAny<IEnumerable<object>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(succeededLoadResult));

            // Act
            var result = await _etlCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IRecordExtractorStrategy>(),
                    It.IsAny<IPropertyExtractorStrategy>(),
                    It.IsAny<IEntityIdentifierStrategy>(),
                    It.IsAny<ISaleItemExtractorStrategy>()), Times.Once);
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.ExtractAsync(
                    It.IsAny<string>()), Times.Once);

            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<ITransformationStrategy>()), Times.Once);
            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Transform(
                    It.IsAny<ExtractionResult>()), Times.Once);

            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IReportLoaderStrategy>(),
                    It.IsAny<IReportBuilderStrategy>()), Times.Never);
            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Load(
                    It.IsAny<IEnumerable<object>>(),
                    It.IsAny<string>()), Times.Never);

            _mocker.GetMock<IMediatorHandler>()
                   .Verify(
                r => r.NotifyErrorAsync(
                    It.Is<ErrorNotification>(x =>
                        x.Key == nameof(Transformer) &&
                        x.Value == failedTransformationResult.ErrorMessage)), Times.Once);
            Assert.False(result);
        }

        [Fact(DisplayName = "Create sales report load failed")]
        [Trait("Unit", "Etl Commands")]
        public async Task CreateSalesReportCommand_WithFailedLoad_Failed()
        {
            // Arrange
            var request = new CreateSalesReportCommand("validOutput", new string[] { "validInput" });
            var succeededExtractionResult = new ExtractionResult(null, null, null);
            var succeededTransformationResult = new TransformationResult(new string[] { });
            var failedLoadResult = LoadResult.WithError("Error message");

            _mocker.GetMock<IExtractor>()
                .Setup(x => x.ExtractAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(succeededExtractionResult));

            _mocker.GetMock<ITransformer>()
                .Setup(x => x.Transform(It.IsAny<ExtractionResult>()))
                .Returns(succeededTransformationResult);

            _mocker.GetMock<ILoader>()
                .Setup(x => x.Load(It.IsAny<IEnumerable<object>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(failedLoadResult));

            // Act
            var result = await _etlCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IRecordExtractorStrategy>(),
                    It.IsAny<IPropertyExtractorStrategy>(),
                    It.IsAny<IEntityIdentifierStrategy>(),
                    It.IsAny<ISaleItemExtractorStrategy>()), Times.Once);
            _mocker.GetMock<IExtractor>()
                   .Verify(
                r => r.ExtractAsync(
                    It.IsAny<string>()), Times.Once);

            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<ITransformationStrategy>()), Times.Once);
            _mocker.GetMock<ITransformer>()
                   .Verify(
                r => r.Transform(
                    It.IsAny<ExtractionResult>()), Times.Once);

            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Configure(
                    It.IsAny<IReportLoaderStrategy>(),
                    It.IsAny<IReportBuilderStrategy>()), Times.Once);
            _mocker.GetMock<ILoader>()
                   .Verify(
                r => r.Load(
                    It.IsAny<IEnumerable<object>>(),
                    It.IsAny<string>()), Times.Once);

            _mocker.GetMock<IMediatorHandler>()
                   .Verify(
                r => r.NotifyErrorAsync(
                    It.Is<ErrorNotification>(x =>
                        x.Key == nameof(Loader) &&
                        x.Value == failedLoadResult.ErrorMessage)), Times.Once);
            Assert.False(result);
        }
    }
}
