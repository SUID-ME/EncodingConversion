using System.DirectoryServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EncodingConversion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private EncodeConverter _encodeConverter;
        private RewriteFile _rewriteFile;
        private RecursiveTraversal _traversal;
        private string _rootDir;

        public MainWindow()
        {
            InitializeComponent();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            _rewriteFile = new RewriteFile(win1251);
             _traversal = new RecursiveTraversal(_rewriteFile);
        }

        public void Convert_Click(object sender, EventArgs e)
        {
            Output.Text = "Начата перекодировка в корневой папке " + _rootDir;
            _traversal.RunTraversal(_rootDir);
            Output.Text = "Перекодировка завершена";
        }

        private void FolderChoose_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFolderDialog dialog = new Microsoft.Win32.OpenFolderDialog();
            dialog.Multiselect = false;
            dialog.Title = "Show a folder";

            bool? res = dialog.ShowDialog();
            if (res == true)
            {
                _rootDir = dialog.FolderName;
                Output.Text = _rootDir;
            } else
            {
                Output.Text = "Ошибка при выборе папки";
            }
        }
    }
}