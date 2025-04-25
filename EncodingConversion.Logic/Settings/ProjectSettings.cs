using ReactiveUI;
using SeamSearchLaserScan.Logic.ProjectSettings;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace EncodingConversion.Logic.Settings
{
    public class ProjectSettings : ISettingsData
    {
        public ObservableCollection<ExtensionInfo> ExtensionInfoData { get; set; } = [];
        public bool IsNeedResetFolder
        {
            get; set;
        } = true;
        public bool IsNeedCheckSourceEncoding
        {
            get; set;
        } = true;
        public bool IsNeedAutoSelectSourceEncoding
        {
            get; set;
        } = false;
    }
}
