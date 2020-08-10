using SSSA.Etl.Domain.Extraction;
using System.Collections.Generic;

namespace SSSA.Etl.Domain.Transform.TransformationStrategies
{
    public interface ITransformationStrategy
    {
        TransformationResult Transform(ExtractionResult extractionResult);
    }
}
