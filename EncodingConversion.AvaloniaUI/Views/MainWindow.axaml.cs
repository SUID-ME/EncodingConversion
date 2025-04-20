using Avalonia.Controls;

namespace EncodingConversion.AvaloniaUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
        {
            App.ProjectSettingsLoader.WriteSettings();
        }
    }
}