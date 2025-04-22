using SeamSearchLaserScan.Logic.ProjectSettings;
using System.Collections.ObjectModel;

namespace EncodingConversion.Logic.Settings
{
    public class ProjectSettings : ISettingsData
    {
        public ProjectSettings()
        {
            ExtensionInfoData = new()
            {

            };
        }

        public ObservableCollection<ExtensionInfo> ExtensionInfoData { get; set; }
    }
}
