using EncodingConversion.Logic.Settings;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EncodingConversion.Logic
{
    internal class RecursiveTraversal(RewriteFile rewrite, ObservableCollection<ExtensionInfo> choosen)
    {
        private RewriteFile _rewriteFile = rewrite;
        private ObservableCollection<ExtensionInfo> _choosenExtension = choosen;

        private object _syncLock = new();

        public void RunTraversal(string rootDir)
        {
            RecursionLogic(rootDir, RecodeByLogic);
        }

        public void LocateExtensions(string rootDir)
        {
            RecursionLogic(rootDir, LocateExtLogic);
        }

        private void RecursionLogic(string curretDir, Action<string> action)
        {
            string[] allfiles = Directory.GetFiles(curretDir);
            foreach (string file in allfiles)
            {
                action(file);
            }

            Array.Clear(allfiles);
            string[] allDirs = Directory.GetDirectories(curretDir);
            foreach (string dir in allDirs)
            {
                RecursionLogic(dir, action);
            }
        }

        private void RecodeByLogic(string file)
        {
            string extension = Path.GetExtension(file);
            if (CheckSupportedExt(extension))
            {
                if (_rewriteFile.Rewrite(file) == false)
                {
                    Debug.WriteLine($"File '{file} is not recoding'");
                }
            }
        }

        private bool CheckSupportedExt(string extension)
        {
            lock (_syncLock)
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

        private void LocateExtLogic(string file)
        {
            string extension = Path.GetExtension(file);
            if (_choosenExtension.Any(x => x.Symbols == extension) == false)
            {
                _choosenExtension.Add(new ExtensionInfo(extension, false));
            }
        }
    }
}