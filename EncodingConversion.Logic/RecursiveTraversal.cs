using System.Diagnostics;
using System.IO;

namespace EncodingConversion.Logic
{
    internal class RecursiveTraversal(RewriteFile rewrite, List<ExtensionInfo> choosen)
    {
        private RewriteFile _rewriteFile = rewrite;
        private List<ExtensionInfo> _choosenExtension = choosen;

        private object _syncLock = new();

        public void RunTraversal(string rootDir)
        {
            _recursion_logic(rootDir);
        }

        public void UpdateExtensionList(List<ExtensionInfo> extensions)
        {
            lock(_syncLock)
            {
                _choosenExtension = extensions;
            }

        }

        private void _recursion_logic(string curretDir)
        {
            string[] allfiles = Directory.GetFiles(curretDir);
            foreach (string file in allfiles)
            {
                string extension = Path.GetExtension(file);
                if (_checkSupportExt(extension))
                {
                    if (_rewriteFile.Rewrite(file) == false)
                    {
                        Debug.WriteLine($"File '{file} is not recoding'");
                    }
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
            lock(_syncLock)
            {
                foreach (var ext in _choosenExtension)
                {
                    if (ext.IsEnable == true && ext.Symbols == extension)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}