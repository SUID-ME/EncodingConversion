using SeamSearchLaserScan.Logic.ProjectSettings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
