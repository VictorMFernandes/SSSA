using System;
using System.IO;
using System.Threading.Tasks;

namespace SSSA.Etl.Domain.Load.ReportLoaderStrategies
{
    public class ToFileLoaderStrategy : IReportLoaderStrategy
    {
        private readonly string _fileName;

        public ToFileLoaderStrategy(string fileName)
        {
            _fileName = fileName;
        }

        public async Task<LoadResult> LoadAsync(string content, string destination)
        {
            try
            {
                Directory.CreateDirectory(destination);
                var filePath = Path.Combine(destination, $"{DateTime.Now:yyyyMMddHHmmss}{_fileName}");
                using var sw = new StreamWriter(filePath, false);
                await sw.WriteAsync(content);
                return new LoadResult(filePath);
            }
            catch (Exception ex)
            {
                return LoadResult.WithError(ex.Message);
            }
        }
    }
}
