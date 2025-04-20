using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingConversion.Logic
{
    public class RecodingLogic : IRecoding
    {
        private RewriteFile _rewriteFile;
        private RecursiveTraversal _traversal;
        private ObservableCollection<ExtensionInfo> _choosenExt;

        public RecodingLogic(ObservableCollection<ExtensionInfo> extensions = null)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            _rewriteFile = new RewriteFile(win1251);

            if (extensions == null)
            {
                extensions = new ObservableCollection<ExtensionInfo>() {
                    new ExtensionInfo(".tst1"),
                    new ExtensionInfo(".tst2"),
                    new ExtensionInfo(".tst3"),
                };
            }

            _choosenExt = extensions;

            _traversal = new RecursiveTraversal(_rewriteFile, _choosenExt);

        }

        public void Run(string rootDir)
        {
            _traversal.RunTraversal(rootDir);
        }

        public ObservableCollection<ExtensionInfo> ExtensionInfos
        {
            get { return _choosenExt; }
            set { _choosenExt = value; }
        }
    }
}
