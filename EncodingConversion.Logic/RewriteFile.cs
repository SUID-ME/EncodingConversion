using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace EncodingConversion.Logic
{
    internal class RewriteFile
    {
        private Encoding _encFrom;
        private Encoding _encTo = Encoding.UTF8;

        internal RewriteFile(Encoding encoderFrom)
        {
            _encFrom = encoderFrom;
        }

        public Encoding EncodingFrom
        {
            get { return _encFrom; }
            set { _encFrom = value; }
        }

        public bool Rewrite(string filepath)
        {
            Encoding encoding = _encFrom;

            if (CheckEncoding(filepath, ref encoding) == false)
            {
                return false;
            }

            string? line;
            string decodeFilePath = filepath + ".decode";
            try
            {
                StreamWriter sw = new StreamWriter(decodeFilePath);
                StreamReader sr = new StreamReader(filepath, encoding);
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

                Debug.WriteLine($"File '{filepath}' is recoded");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message, "Error!");
                return false;
            }


            return true;
        }

        private bool CheckEncoding(string filePath, ref Encoding encoding)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                Ude.CharsetDetector charsetDetector = new Ude.CharsetDetector();
                charsetDetector.Feed(fileStream);
                charsetDetector.DataEnd();

                if (charsetDetector.Charset != null)
                {
                    Debug.WriteLine($"Кодировка - {charsetDetector.Charset}, Уверенность в результате - {charsetDetector.Confidence}.");

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding enc = Encoding.GetEncoding(charsetDetector.Charset);

                    if (enc == _encTo)
                    {
                        Debug.WriteLine($"Файл '{filePath}' Уже в нужной кодировке");
                        return false;
                    }
                    else if (enc != _encFrom)
                    {
                        Debug.WriteLine($"Файл '{filePath}' не соответствует указанной начальной кодировке");
                        encoding = enc;
                        return true;
                    } else
                    {
                        return true;
                    }
                }
                else
                {
                    Debug.WriteLine("Проблема с определением кодировки.");
                    return false;
                }
            }
        }
    }
}