using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SSSA.Core.Api.Communication.Errors;
using SSSA.Core.Api.Communication.Mediator;
using SSSA.Core.Api.Exceptions;

namespace SSSA.Core.Api.Configuration
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.InjectCoreDependencies();

            return services;
        }

        public static TAppSettings AddAppSettings<TAppSettings>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where TAppSettings : AppSettingsBase
        {
            var sp = services.BuildServiceProvider();
            var localizer = sp.GetService<IStringLocalizer>();

            var appSettingsSection = configuration.GetSection(sectionName);
            services.Configure<TAppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<TAppSettings>();

            if (appSettings?.IsValid != true)
            {
                throw new AppSettingsException(localizer["The appsettings.json section could not be found or is invalid."], sectionName);
            }

            return appSettings;
        }

        private static void InjectCoreDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IMediatorHandler, MediatorHandler>();
            services.AddSingleton<INotificationHandler<ErrorNotification>, ErrorHandler>();
        }
    }
}
