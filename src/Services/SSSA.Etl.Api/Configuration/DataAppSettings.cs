using SSSA.Core.Api.Configuration;

namespace SSSA.Etl.Api.Configuration
{
    public class DataAppSettings : AppSettingsBase
    {
        public const string SectionName = "DataSettings";
        public string Source { get; set; }
        public string Destination { get; set; }
        private string _cultureInfo;
        public string CultureInfo
        {
            get => _cultureInfo ?? "en-US";
            set => _cultureInfo = value;
        }

        public override bool IsValid => !string.IsNullOrEmpty(Source) && !string.IsNullOrEmpty(Destination);

        public DataAppSettings()
        {
        }
    }
}