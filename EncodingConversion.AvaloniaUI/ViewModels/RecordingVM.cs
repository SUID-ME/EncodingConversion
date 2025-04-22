using Avalonia.Platform.Storage;
using DynamicData;
using EncodingConversion.Logic;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EncodingConversion.AvaloniaUI.ViewModels
{
    internal class RecordingVM : ViewModelBase
    {
        #region Fields
        private IRecoding _recodingLogic;
        private string _rootDir = string.Empty;

        private readonly ObservableAsPropertyHelper<bool> _isCanExecuteRecording;
        private readonly ObservableAsPropertyHelper<bool> _isCanExecuteRemoveExt;

        private static string
            _recodingDirText = "Начата перекодировка в корневой папке ",
            _recodingDoneText = "Перекодировка завершена. Можете продолжить, выбрав новую папку",
            _dirPickErrorText = "Папка не выбрана",
            _dirPickSuggestion = "Выберите корневую папку",
            _dirPickedDir = "Выбрана папка: ";

        private string _outputText = string.Empty;
        private readonly ObservableCollection<ExtensionInfo> _extensionInfos;
        private ExtensionInfo _selectedExt;
        #endregion Fields

        #region Constructor
        public RecordingVM()
        {
            _isCanExecuteRecording = this.WhenAnyValue(x => x.RootDir)
                .Select(rootDir => !(string.IsNullOrEmpty(rootDir)) &&
                    Directory.Exists(rootDir))
                .ToProperty(this, x => x.IsCanExecuteRecoding);
            _isCanExecuteRemoveExt = this.WhenAnyValue(x => x.SelectedExtension).
                Select(extInfo => (extInfo != null))
                .ToProperty(this, x => x.IsCanExecuteRemoveExt);

            _extensionInfos = GetSettingsInfo();

            _recodingLogic = new RecodingLogic(_extensionInfos);


            RecodingCommand = ReactiveCommand.CreateFromTask(RunRecodingMethodAsync, this.WhenAnyValue(x => x.IsCanExecuteRecoding));
            LocateExtensionsCommand = ReactiveCommand.CreateFromTask(RunLocateExtensionsAsync, this.WhenAnyValue(x => x.IsCanExecuteRecoding));
            ClearExtensionsCommand = ReactiveCommand.Create(ClearExtensions);
            ClearDisabladExtensionsCommand = ReactiveCommand.Create(ClearDisableExtensions);
            PickRootDirCommand = ReactiveCommand.CreateFromTask(PickRootDirAsync);
            AddExtensionCommand = ReactiveCommand.Create(AddNewExtension);
            RemoveSelectedExtCommand = ReactiveCommand.Create(RemoveExtension, this.WhenAnyValue(x => x.IsCanExecuteRemoveExt));
            EnableOrDisableAllExtensionsCommand = ReactiveCommand.Create(EnableOrDisableAllList);

            OutPutText = _dirPickSuggestion;
        }
        #endregion Constructor

        #region Properties
        public Interaction<Unit, IReadOnlyList<IStorageFolder>> ShowFilePiker { get; } = new();
        public bool IsCheckedAllEnable { get; set; }
        public string OutPutText
        {
            get { return _outputText; }
            set { this.RaiseAndSetIfChanged(ref _outputText, value); }
        }

        public string RootDir
        {
            get { return _rootDir; }
            set { this.RaiseAndSetIfChanged(ref _rootDir, value); }
        }

        public ObservableCollection<ExtensionInfo> ExtensionInfos => _extensionInfos;
        public ExtensionInfo SelectedExtension
        {
            get { return _selectedExt; }
            set { this.RaiseAndSetIfChanged(ref _selectedExt, value); }
        }

        public bool IsCanExecuteRecoding => _isCanExecuteRecording.Value;
        public bool IsCanExecuteRemoveExt => _isCanExecuteRemoveExt.Value;
        public ReactiveCommand<Unit, Unit> RecodingCommand { get; }
        public ReactiveCommand<Unit, Unit> PickRootDirCommand { get; }
        public ReactiveCommand<Unit, Unit> AddExtensionCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveSelectedExtCommand { get; }
        public ReactiveCommand<Unit, Unit> LocateExtensionsCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearExtensionsCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearDisabladExtensionsCommand { get; }
        public ReactiveCommand<Unit, Unit> EnableOrDisableAllExtensionsCommand { get; }
        #endregion Properties

        #region Methods
        public async Task RunRecodingMethodAsync()
        {
            if (string.IsNullOrEmpty(RootDir) || Directory.Exists(RootDir) == false)
            {
                OutPutText = _dirPickErrorText;
                return;
            }

            OutPutText = _recodingDirText + _rootDir;
            await Task.Run(() =>
            {
                _recodingLogic.RunRecoding(RootDir);
            });

            OutPutText = _recodingDoneText;
            RootDir = string.Empty;
        }

        public async Task RunLocateExtensionsAsync()
        {
            if (string.IsNullOrEmpty(RootDir) || Directory.Exists(RootDir) == false)
            {
                OutPutText = _dirPickErrorText;
                return;
            }


            OutPutText = "Начат поиск расширений в папке: " + _rootDir;
            await Task.Run(() =>
            {
                _recodingLogic.LocateExtensions(RootDir);
            });
            OutPutText = "Поиск расширений завершен";
        }

        public async Task PickRootDirAsync()
        {
            var folders = await ShowFilePiker.Handle(Unit.Default).FirstAsync();
            if (folders?.Count > 0)
            {
                RootDir = folders.First().Path.LocalPath;
                OutPutText = _dirPickedDir + RootDir;
            }
        }

        public void AddNewExtension()
        {
            ExtensionInfos.Add(new("."));
        }

        public void RemoveExtension()
        {
            if (SelectedExtension != null)
            {
                ExtensionInfos.Remove(SelectedExtension);
            }
        }

        public void ClearExtensions()
        {
            _extensionInfos.Clear();
        }

        public void ClearDisableExtensions()
        {
            List<ExtensionInfo> clearList = [];

            foreach (var extensionInfo in ExtensionInfos)
            {
                if (extensionInfo.IsEnable == false)
                {
                    clearList.Add(extensionInfo);
                }
            }

            _extensionInfos.Remove(clearList);
        }

        private ObservableCollection<ExtensionInfo> GetSettingsInfo()
        {
            var ret = App.ProjectSettings.ExtensionInfoData;
            if (ret != null && ret.Count == 0)
            {
                ret.AddRange(GetDefaultExtensions());
            }

            return ret;
        }

        private void EnableOrDisableAllList()
        {
            foreach (var extensionInfo in ExtensionInfos)
            {
                if (extensionInfo.IsEnable != IsCheckedAllEnable)
                {
                    extensionInfo.IsEnable = IsCheckedAllEnable;
                }

            }
        }

        private List<ExtensionInfo> GetDefaultExtensions()
        {
            return new List<ExtensionInfo>()
            {
                new ExtensionInfo(".cs"),
                new ExtensionInfo(".cpp"),
                new ExtensionInfo(".h")
            };
        }
        #endregion Methods
    }
}