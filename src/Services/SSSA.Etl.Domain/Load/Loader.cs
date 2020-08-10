using Microsoft.Extensions.Localization;
using SSSA.Etl.Domain.Load.ReportBuilderStrategies;
using SSSA.Etl.Domain.Load.ReportLoaderStrategies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSSA.Etl.Domain.Load
{
    public class Loader : ILoader
    {
        public const string NotConfiguredErrorMessage = "The loader must be configured before use";

        private readonly IStringLocalizer<Loader> _localizer;
        private IReportLoaderStrategy _reportLoaderStrategy;
        private IReportBuilderStrategy _reportBuilderStrategy;
        private bool _configured;

        public Loader(IStringLocalizer<Loader> localizer)
        {
            _localizer = localizer;
            _configured = false;
        }

        public void Configure(
            IReportLoaderStrategy reportLoaderStrategy,
            IReportBuilderStrategy reportBuilderStrategy)
        {
            _reportLoaderStrategy = reportLoaderStrategy;
            _reportBuilderStrategy = reportBuilderStrategy;

            _configured = true;
        }

        public async Task<LoadResult> LoadAsync(IEnumerable<object> data, string destination)
        {
            if (!_configured)
            {
                return LoadResult.WithError(_localizer[NotConfiguredErrorMessage]);
            }

            var content = _reportBuilderStrategy.Build(data);
            return await _reportLoaderStrategy.LoadAsync(content, destination);
        }
    }
}
