using Avalonia.Platform.Storage;
using EncodingConversion.Logic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingConversion.AvaloniaUI.ViewModels
{
    internal class RecordingVM : ViewModelBase
    {
        #region Fields
        private IRecoding recodingLogic;
        private string _rootDir;

        private static string
            _recodingDirText = "Начата перекодировка в корневой папке ",
            _recodingDoneText = "Перекодировка завершена. Можете продолжить, выбрав новую папку",
            _dirPickErrorText = "Папка не выбрана",
            _dirPickSuggestion = "Выберите корневую папку",
            _filePickTitle = "Show a folder";

        private string _outputText = string.Empty;
        #endregion Fields

        #region Constructor
        public RecordingVM()
        {
            recodingLogic = new RecodingLogic();
            //RecodingCommand = ReactiveCommand.Create(RunRecodingMethod, this.WhenAnyValue(x => x.IsCanExecuteRecoding));
            RecodingCommand = ReactiveCommand.Create(RunRecodingMethod);
            PickRootDirCommand = ReactiveCommand.CreateFromTask(PickRootDir);

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

        public bool IsCanExecuteRecoding
        {
            get { return (string.IsNullOrEmpty(RootDir) == false); }
        }

        public ReactiveCommand<Unit, Unit> RecodingCommand { get; }
        public ReactiveCommand<Unit, Unit> PickRootDirCommand { get; }
        #endregion Properties

        #region Methods
        private void RunRecodingMethod()
        {
#warning Make it Async!
            if (string.IsNullOrEmpty(RootDir))
            {
                return;
            }

            //Output.Text = _recodingDirText + _rootDir;
            //recodingLogic.Run(RootDir);
            OutPutText = _recodingDoneText;
            RootDir = string.Empty;
        }

        public async Task PickRootDir()
        {
            var folders = await ShowFilePiker.Handle(Unit.Default).FirstAsync();
            if (folders?.Count > 0)
            {
                RootDir = folders.First().Path.ToString();
            }
        }
        #endregion Methods
    }
}
