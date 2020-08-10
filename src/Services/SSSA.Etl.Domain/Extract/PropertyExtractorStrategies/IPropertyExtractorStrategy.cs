namespace SSSA.Etl.Domain.ExtractionStrategies.Property
{
    public interface IPropertyExtractorStrategy
    {
        string[] ExtractProperties(string record);
    }
}
