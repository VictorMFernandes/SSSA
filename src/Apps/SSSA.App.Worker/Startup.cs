using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSSA.App.Worker.Workers;
using SSSA.Core.Api.Configuration;
using SSSA.Etl.Api.Configuration;

namespace SSSA.App.Worker
{
    public static class Startup
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Program));
            services.AddApiConfiguration();
            services.AddEtlConfiguration(configuration);
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");

            services.AddHostedService<DirectoryWatcherWorker>();
        }
    }
}
