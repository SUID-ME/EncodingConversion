namespace EncodingConversion.AvaloniaUI.ViewModels
{
    internal partial class MainWindowViewModel : ViewModelBase
    {
        public RecordingVM RecordingVM { get; set; } = new RecordingVM();
        public MainWindowViewModel() { }
    }
}
