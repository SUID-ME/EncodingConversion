using EncodingConversion.Logic;
using System.Text;
using System.Windows;

namespace EncodingConversion.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        #endregion Fields

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            ConvertButton.IsEnabled = false;

            recodingLogic = new RecodingLogic([]);
            Output.Text = _dirPickSuggestion;
        }
        #endregion Constructor

        #region UICallBacks
        /// <summary>
        /// Метод запускающий перекодировку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Convert_Click(object sender, EventArgs e)
        {
#warning Make it Async!
            if (string.IsNullOrEmpty(_rootDir))
            {
                return;
            }

            //Output.Text = _recodingDirText + _rootDir;
            recodingLogic.RunRecoding(_rootDir);
            Output.Text = _recodingDoneText;
            _rootDir = String.Empty;
            ConvertButton.IsEnabled = false;
        }

        /// <summary>
        /// Метод, для выбора корневой папки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderChoose_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFolderDialog dialog = new Microsoft.Win32.OpenFolderDialog();
            dialog.Multiselect = false;
            dialog.Title = _filePickTitle;

            bool? res = dialog.ShowDialog();
            if (res == true)
            {
                _rootDir = dialog.FolderName;
                Output.Text = _rootDir;
                ConvertButton.IsEnabled = true;
            }
            else
            {
                Output.Text = _dirPickErrorText;
            }
        }
        #endregion UICallBacks
    }
}