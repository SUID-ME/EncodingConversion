using Avalonia.Platform.Storage;
using EncodingConversion.Logic;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EncodingConversion.AvaloniaUI.ViewModels
{
    internal partial class MainWindowViewModel : ViewModelBase
    {
        public RecordingVM RecordingVM { get; set; } = new RecordingVM();
        public MainWindowViewModel() { }
    }
}
