using SSSA.Etl.Domain.ExtractionStrategies.Property;

namespace SSSA.Etl.Domain.Extract.PropertyExtractorStrategies
{
    public class SplitByStringPropertyExtractorStrategy : IPropertyExtractorStrategy
    {
        private readonly string _separator;

        public SplitByStringPropertyExtractorStrategy(string separator)
        {
            _separator = separator;
        }

        public string[] ExtractProperties(string record) => record.Split(_separator);
    }
}
