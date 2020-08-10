using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSSA.Core.Api.Configuration;
using SSSA.Etl.Api.Commands;
using SSSA.Etl.Domain.Extract;
using SSSA.Etl.Domain.Load;
using SSSA.Etl.Domain.Transform;
using System.Globalization;

namespace SSSA.Etl.Api.Configuration
{
    public static class EtlConfiguration
    {
        public static void AddEtlConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSettings = services.AddAppSettings<DataAppSettings>(configuration, DataAppSettings.SectionName);
            services.AddSingleton(x => CultureInfo.CreateSpecificCulture(dataSettings.CultureInfo));
            services.InjectEtlDependencies();
        }

        private static void InjectEtlDependencies(this IServiceCollection services)
        {
            // Commands
            services.AddSingleton<IRequestHandler<CreateSalesReportCommand, bool>, EtlCommandHandler>();

            services.AddSingleton<IExtractor, Extractor>();
            services.AddSingleton<ITransformer, Transformer>();
            services.AddSingleton<ILoader, Loader>();
        }
    }
}
