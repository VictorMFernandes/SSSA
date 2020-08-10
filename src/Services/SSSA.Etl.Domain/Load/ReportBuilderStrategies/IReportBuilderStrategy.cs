using System.Collections.Generic;

namespace SSSA.Etl.Domain.Load.ReportBuilderStrategies
{
    public interface IReportBuilderStrategy
    {
        string Build(IEnumerable<object> data);
    }
}
