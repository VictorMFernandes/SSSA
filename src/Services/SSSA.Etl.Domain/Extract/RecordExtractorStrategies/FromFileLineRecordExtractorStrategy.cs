using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SSSA.Etl.Domain.Extract.RecordExtractorStrategies
{
    public class FromFileLineRecordExtractorStrategy : IRecordExtractorStrategy
    {
        public FromFileLineRecordExtractorStrategy()
        {
        }

        public async IAsyncEnumerable<string> ExtractRecordsAsync(string filePath)
        {
            var record = string.Empty;

            using var file = new StreamReader(filePath);
            while ((record = await file.ReadLineAsync()) != null)
            {
                yield return record;
            }
        }
    }
}
