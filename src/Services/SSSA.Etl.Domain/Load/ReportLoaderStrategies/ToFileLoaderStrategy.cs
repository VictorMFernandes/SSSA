using System;
using System.IO;
using System.Threading.Tasks;

namespace SSSA.Etl.Domain.Load.ReportLoaderStrategies
{
    public class ToFileLoaderStrategy : IReportLoaderStrategy
    {
        public async Task<LoadResult> LoadAsync(string content, string destination, string reportName)
        {
            try
            {
                Directory.CreateDirectory(destination);
                var filePath = Path.Combine(destination, $"{DateTime.Now:yyyyMMddHHmmss}{reportName}");

                await File.AppendAllTextAsync(filePath, content);
                return new LoadResult(filePath);
            }
            catch (Exception ex)
            {
                return LoadResult.WithError(ex.Message);
            }
        }
    }
}
