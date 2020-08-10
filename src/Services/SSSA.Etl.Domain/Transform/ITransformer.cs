using SSSA.Etl.Domain.Extraction;
using SSSA.Etl.Domain.Transform.TransformationStrategies;

namespace SSSA.Etl.Domain.Transform
{
    public interface ITransformer
    {
        void Configure(ITransformationStrategy transformationStrategy);
        TransformationResult Transform(ExtractionResult extractionResult);
    }
}
