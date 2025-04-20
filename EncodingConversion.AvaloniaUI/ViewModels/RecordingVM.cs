using Avalonia.Platform.Storage;
using DynamicData;
using DynamicData.Binding;
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

        private static string
            _recodingDirText = "Начата перекодировка в корневой папке ",
            _recodingDoneText = "Перекодировка завершена. Можете продолжить, выбрав новую папку",
            _dirPickErrorText = "Папка не выбрана",
            _dirPickSuggestion = "Выберите корневую папку",
            _dirPickedDir = "Выбрана папка: ";

        private string _outputText = string.Empty;
        private readonly ObservableCollection<ExtensionInfo> _extensionInfos;
        #endregion Fields

        #region Constructor
        public RecordingVM()
        {
            _isCanExecuteRecording = this.WhenAnyValue(x => x.RootDir)
                .Select(rootDir => !(string.IsNullOrEmpty(rootDir)) &&
                    Directory.Exists(rootDir))
                .ToProperty(this, x => x.IsCanExecuteRecoding);

            _extensionInfos = new ObservableCollection<ExtensionInfo>()
            {
                GetDefaultExtensions()
            };

            _recodingLogic = new RecodingLogic(_extensionInfos);


            RecodingCommand = ReactiveCommand.CreateFromTask(RunRecodingMethodAsync, this.WhenAnyValue(x => x.IsCanExecuteRecoding));
            PickRootDirCommand = ReactiveCommand.CreateFromTask(PickRootDirAsync);
            AddExtensionCommand = ReactiveCommand.Create(AddNewExtension);

            OutPutText = _dirPickSuggestion;
        }
        #endregion Constructor

        #region Properties
        public Interaction<Unit, IReadOnlyList<IStorageFolder>> ShowFilePiker { get; } = new();
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

        public bool IsCanExecuteRecoding => _isCanExecuteRecording.Value;
        public ReactiveCommand<Unit, Unit> RecodingCommand { get; }
        public ReactiveCommand<Unit, Unit> PickRootDirCommand { get; }
        public ReactiveCommand<Unit, Unit> AddExtensionCommand {  get; }
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
                _recodingLogic.Run(RootDir);
            });

            OutPutText = _recodingDoneText;
            RootDir = string.Empty;
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