using System.Collections.Generic;

namespace SSSA.Etl.Domain.Load.ReportBuilderStrategies
{
    public class JoinByStringBuilderStrategy : IReportBuilderStrategy
    {
        private readonly string _separator;

        public JoinByStringBuilderStrategy(string separator)
        {
            _separator = separator;
        }

        public string Build(IEnumerable<object> data) => string.Join(_separator, data);
    }
}
