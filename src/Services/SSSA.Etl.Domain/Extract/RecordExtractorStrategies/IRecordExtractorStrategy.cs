using System.Collections.Generic;

namespace SSSA.Etl.Domain.Extract.RecordExtractorStrategies
{
    public interface IRecordExtractorStrategy
    {
        IAsyncEnumerable<string> ExtractRecordsAsync(string filePath);
    }
}