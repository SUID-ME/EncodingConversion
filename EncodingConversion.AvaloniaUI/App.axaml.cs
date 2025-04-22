using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using EncodingConversion.AvaloniaUI.ViewModels;
using EncodingConversion.AvaloniaUI.Views;
using EncodingConversion.Logic.Settings;
using ReactiveUI;
using SeamSearchLaserScan.Logic.ProjectSettings;
using Splat;
using System.Linq;

namespace EncodingConversion.AvaloniaUI
{
    public partial class App : Application
    {
        private static object _settingsSync = new object();
        private static ProjectSettingsLoader<ProjectSettings> _settingsLoader;

        public static ProjectSettings ProjectSettings
        {
            get
            {
                lock (_settingsSync)
                {
                    return _settingsLoader.Settings;
                }
            }
        }

        public static ProjectSettingsLoader<ProjectSettings> ProjectSettingsLoader
        {
            get
            {
                lock (_settingsSync)
                {
                    return _settingsLoader;
                }
            }
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
            Locator.CurrentMutable.Register<IActivationForViewFetcher>(() => new AvaloniaActivationForViewFetcher());

            _settingsLoader = new ProjectSettingsLoader<ProjectSettings>(new ProjectSettings());

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}