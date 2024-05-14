using System.Text;

namespace EncodingConversion
{
    public class EncodeConverter
    {
        private Encoding _from;
        private Encoding _to;

        public EncodeConverter(Encoding encFrom, Encoding encTo)
        {
            _from = encFrom;
            _to = encTo;
        }

        public string Convert(string forConvertion)
        {
            byte[] encToBytes = _from.GetBytes(forConvertion);
            byte[] encFromBytes = Encoding.Convert(_to, _from, encToBytes);
            return _from.GetString(encFromBytes);
        }
    }
}