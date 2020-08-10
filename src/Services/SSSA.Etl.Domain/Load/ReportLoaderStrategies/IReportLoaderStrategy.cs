using System.Threading.Tasks;

namespace SSSA.Etl.Domain.Load.ReportLoaderStrategies
{
    public interface IReportLoaderStrategy
    {
        Task<LoadResult> LoadAsync(string content, string destination);
    }
}
