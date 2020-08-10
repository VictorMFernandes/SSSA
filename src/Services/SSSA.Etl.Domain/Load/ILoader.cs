using SSSA.Etl.Domain.Load.ReportBuilderStrategies;
using SSSA.Etl.Domain.Load.ReportLoaderStrategies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSSA.Etl.Domain.Load
{
    public interface ILoader
    {
        void Configure(
            IReportLoaderStrategy reportLoaderStrategy,
            IReportBuilderStrategy reportBuilderStrategy);
        Task<LoadResult> LoadAsync(IEnumerable<object> data, string destination);
    }
}