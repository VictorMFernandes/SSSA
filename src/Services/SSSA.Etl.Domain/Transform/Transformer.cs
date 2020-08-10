using Microsoft.Extensions.Localization;
using SSSA.Etl.Domain.Extraction;
using SSSA.Etl.Domain.Transform.TransformationStrategies;

namespace SSSA.Etl.Domain.Transform
{
    public class Transformer : ITransformer
    {
        public const string NotConfiguredErrorMessage = "The transformer must be configured before use";

        private readonly IStringLocalizer<Transformer> _localizer;
        private ITransformationStrategy _transformationStrategy;
        private bool _configured;

        public Transformer(IStringLocalizer<Transformer> localizer)
        {
            _localizer = localizer;
            _configured = false;
        }

        public void Configure(ITransformationStrategy transformationStrategy)
        {
            _transformationStrategy = transformationStrategy;
            _configured = true;
        }

        public TransformationResult Transform(ExtractionResult extractionResult)
        {
            if (!_configured)
            {
                return new TransformationResult(_localizer[NotConfiguredErrorMessage]);
            }

            return _transformationStrategy.Transform(extractionResult);
        }
    }
}
