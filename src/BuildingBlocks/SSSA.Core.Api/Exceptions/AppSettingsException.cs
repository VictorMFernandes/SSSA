using System;

namespace SSSA.Core.Api.Exceptions
{
    public class AppSettingsException : Exception
    {
        public string AppSettingsSectionName { get; }

        public AppSettingsException(string message, string appSettingsSectionName)
            : base(message)
        {
            AppSettingsSectionName = appSettingsSectionName;
        }
    }
}
