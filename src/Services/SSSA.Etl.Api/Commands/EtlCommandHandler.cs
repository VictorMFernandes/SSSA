using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SSSA.Core.Api.Communication.Commands;
using SSSA.Core.Api.Communication.Errors;
using SSSA.Core.Api.Communication.Mediator;
using SSSA.Etl.Domain.Extract;
using SSSA.Etl.Domain.Extract.EntityIdentifierStrategies;
using SSSA.Etl.Domain.Extract.PropertyExtractorStrategies;
using SSSA.Etl.Domain.Extract.RecordExtractorStrategies;
using SSSA.Etl.Domain.Extract.SaleItemExtractorStrategies;
using SSSA.Etl.Domain.Load;
using SSSA.Etl.Domain.Load.ReportBuilderStrategies;
using SSSA.Etl.Domain.Load.ReportLoaderStrategies;
using SSSA.Etl.Domain.Transform;
using SSSA.Etl.Domain.Transform.TransformationStrategies;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SSSA.Etl.Api.Commands
{
    public class EtlCommandHandler : CommandHandlerBase,
        IRequestHandler<CreateSalesReportCommand, bool>
    {
        private readonly ILogger<EtlCommandHandler> _logger;
        private readonly IStringLocalizer<EtlCommandHandler> _localizer;
        private readonly IExtractor _extractor;
        private readonly ITransformer _transformer;
        private readonly ILoader _loader;

        public EtlCommandHandler(
            ILogger<EtlCommandHandler> logger,
            IStringLocalizer<EtlCommandHandler> localizer,
            IMediatorHandler mediatorHandler,
            IExtractor extractor,
            ITransformer transformer,
            ILoader loader)
            : base(mediatorHandler)
        {
            _extractor = extractor;
            _transformer = transformer;
            _loader = loader;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<bool> Handle(CreateSalesReportCommand request, CancellationToken cancellationToken)
        {
            foreach (var inputFilePath in request.InputFilePaths)
            {
                _extractor.Configure(
                    new FromFileLineRecordExtractorStrategy(),
                    new SplitByStringPropertyExtractorStrategy("ç"),
                    new PropertyIndexEntityIdentifierStrategy(0),
                    new SplitByStringSaleItemExtractorStrategy(",", "-", "[", "]"));

                var extractionResult = await _extractor.ExtractAsync(inputFilePath);
                if (!extractionResult.Succeeded)
                {
                    await MediatorHandler.NotifyErrorAsync(new ErrorNotification(nameof(Extractor), extractionResult.ErrorMessage));
                    return false;
                }

                _transformer.Configure(new ExpensivestSaleWorstSalesmanTransformationStrategy());

                var transformationResult = _transformer.Transform(extractionResult);
                if (!transformationResult.Succeeded)
                {
                    await MediatorHandler.NotifyErrorAsync(new ErrorNotification(nameof(Transformer), transformationResult.ErrorMessage));
                    return false;
                }

                _loader.Configure(
                    new ToFileLoaderStrategy(),
                    new JoinByStringBuilderStrategy("ç"));

                var loadResult = await _loader.LoadAsync(transformationResult.Value, request.OutputFilePath, Path.GetFileName(inputFilePath));
                if (!loadResult.Succeeded)
                {
                    await MediatorHandler.NotifyErrorAsync(new ErrorNotification(nameof(Loader), loadResult.ErrorMessage));
                    return false;
                }
            }

            _logger.LogInformation(_localizer["Command {@command} executed successfully"], request);

            return true;
        }
    }
}
