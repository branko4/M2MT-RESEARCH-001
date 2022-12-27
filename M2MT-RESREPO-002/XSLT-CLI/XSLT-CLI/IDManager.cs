using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XSLT_CLI
{
    public class IDManager
    {
        //private static Dictionary<string, object> reservedIds { get; set; } = new Dictionary<string, object>();
        private static readonly SHA1 sha1 = SHA1.Create();

        public string GetID(string name)
        {
            Encoding enc = Encoding.GetEncoding(20127);

            string s = name;
            byte[] array = sha1.ComputeHash(enc.GetBytes(s));
            array[6] = (byte)((array[6] & 0xFu) | 0x50u);
            array[8] = (byte)((array[8] & 0x3Fu) | 0x80u);

            Console.WriteLine($"string:{s}, sha1:{array}");

            return new Guid(array.Take(16).ToArray()).ToString();
        }
    }
}
