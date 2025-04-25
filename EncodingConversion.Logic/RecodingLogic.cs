using EncodingConversion.Logic.Settings;
using System.Collections.ObjectModel;
using System.Runtime;
using System.Text;

namespace EncodingConversion.Logic
{
    public class RecodingLogic : IRecoding
    {
        private RewriteFile _rewriteFile;
        private RecursiveTraversal _traversal;
        private ObservableCollection<ExtensionInfo> _choosenExt;

        public RecodingLogic(ProjectSettings settings)
        {
            Settings = settings;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            _rewriteFile = new RewriteFile(win1251);
            _choosenExt = Settings.ExtensionInfoData;

            _traversal = new RecursiveTraversal(_rewriteFile, _choosenExt);

        }

        public void RunRecoding(string rootDir)
        {
            _traversal.RunTraversal(rootDir);
        }

        public void LocateExtensions(string rootDir)
        {
            _traversal.LocateExtensions(rootDir);
        }

        public ObservableCollection<ExtensionInfo> ExtensionInfos
        {
            get { return _choosenExt; }
            set { _choosenExt = value; }
        }

        public static ProjectSettings Settings;
    }
}
