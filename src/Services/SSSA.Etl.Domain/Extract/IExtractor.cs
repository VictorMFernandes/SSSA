using SSSA.Etl.Domain.Extract.EntityIdentifierStrategies;
using SSSA.Etl.Domain.Extract.RecordExtractorStrategies;
using SSSA.Etl.Domain.Extract.SaleItemExtractorStrategies;
using SSSA.Etl.Domain.Extraction;
using SSSA.Etl.Domain.ExtractionStrategies.Property;
using System.Threading.Tasks;

namespace SSSA.Etl.Domain.Extract
{
    public interface IExtractor
    {
        void Configure(
            IRecordExtractorStrategy recordExtractorStrategy,
            IPropertyExtractorStrategy propertyExtractorStrategy,
            IEntityIdentifierStrategy entityIdentifierStrategy,
            ISaleItemExtractorStrategy saleItemExtractorStrategy);
        Task<ExtractionResult> ExtractAsync(string source);
    }
}
