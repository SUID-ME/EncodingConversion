using System.IO;
using System.Text;
using System.Windows;

namespace EncodingConversion.Logic
{
    internal class RewriteFile
    {
        private Encoding _encFrom;

        internal RewriteFile(Encoding encoderFrom)
        {
            _encFrom = encoderFrom;
        }

        public bool Rewrite(string filepath)
        {
            string? line;
            string decodeFilePath = filepath + ".decode";
            try
            {
                StreamWriter sw = new StreamWriter(decodeFilePath);
                StreamReader sr = new StreamReader(filepath, _encFrom);
                line = sr.ReadLine();
                while (line != null)
                {
                    sw.WriteLine(line);

                    line = sr.ReadLine();
                }

                sr.Close();
                sw.Close();
                File.Delete(filepath);
                File.Move(decodeFilePath, filepath);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Exception: " + e.Message, "Error!");
                return false;
            }


            return true;
        }
    }
}