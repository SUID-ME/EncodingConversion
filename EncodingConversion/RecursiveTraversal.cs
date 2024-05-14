using System.IO;

namespace EncodingConversion
{
    public class RecursiveTraversal
    {
        private RewriteFile _rewriteFile;

        private List<string> _support_extensions = new List<string>() { ".cs" };

        public RecursiveTraversal(RewriteFile rewrite)
        {
            _rewriteFile = rewrite;
        }

        public void RunTraversal(string rootDir)
        {
            _recursion_logic(rootDir);
        }

        private void _recursion_logic(string curretDir)
        {
            string[] allfiles = Directory.GetFiles(curretDir);
            foreach (string file in allfiles)
            {
                string extension = System.IO.Path.GetExtension(file);
                if (_checkSupportExt(extension))
                {
                    _rewriteFile.Rewrite(file);
                }
            }

            Array.Clear(allfiles);
            string[] allDirs = Directory.GetDirectories(curretDir);
            foreach (string dir in allDirs)
            {
                _recursion_logic(dir);
            }
        }

        private bool _checkSupportExt(string extension)
        {
            foreach (string ext in _support_extensions)
            {
                if (ext == extension)
                {
                    return true;
                }
            }
            return false;
        }
    }
}