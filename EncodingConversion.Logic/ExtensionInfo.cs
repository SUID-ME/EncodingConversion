using ReactiveUI;
using SeamSearchLaserScan.Logic.ProjectSettings;
using System.Runtime.Serialization;

namespace EncodingConversion.Logic
{
    [DataContract]
    public class ExtensionInfo(string ext, bool isEnable = true) : ReactiveObject, ISettingsData
    {
        private string _extensionSymbols = ext;
        private bool _isEnable = isEnable;

        [DataMember]
        public string Symbols
        {
            get { return _extensionSymbols; }
            set { this.RaiseAndSetIfChanged(ref _extensionSymbols, value); }
        }

        [DataMember]
        public bool IsEnable
        {
            get { return _isEnable; }
            set { this.RaiseAndSetIfChanged(ref _isEnable, value); }
        }
    }
}