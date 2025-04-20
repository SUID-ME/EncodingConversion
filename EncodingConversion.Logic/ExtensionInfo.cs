using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace EncodingConversion.Logic
{
    public class ExtensionInfo(string ext, bool isEnable = true) : ReactiveObject
    {
        private string _extensionSymbols = ext;
        private bool _isEnable = isEnable;

        public string Symbols
        {
            get { return _extensionSymbols; }
            set {  this.RaiseAndSetIfChanged(ref _extensionSymbols, value); }
        }
        public bool IsEnable
        {
            get { return _isEnable; }
            set { this.RaiseAndSetIfChanged(ref _isEnable, value); }
        }
    }
}