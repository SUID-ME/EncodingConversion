using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingConversion.Logic
{
    public class RecodingLogic : IRecoding
    {
        private RewriteFile _rewriteFile;
        private RecursiveTraversal _traversal;

        public RecodingLogic() {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            _rewriteFile = new RewriteFile(win1251);
            _traversal = new RecursiveTraversal(_rewriteFile);
        }

        public void Run(string rootDir)
        {
            _traversal.RunTraversal(rootDir);
        }
    }
}
