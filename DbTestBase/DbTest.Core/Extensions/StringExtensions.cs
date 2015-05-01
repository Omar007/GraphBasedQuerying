using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DbTest.Core
{
    public static class StringExtensions
    {
        public static IEnumerable<byte> HexStringToByteArray(this string hex)
        {
            hex = hex.StartsWith("0x") ? hex.Substring(2) : hex;

            Debug.Assert(hex.Length % 2 == 0); //Binary key cannot be uneven.

            using (var sr = new StringReader(hex))
            {
                while (sr.Peek() != -1)
                {
                    yield return Convert.ToByte(new String(new Char[] { (char)sr.Read(), (char)sr.Read() }), 16);
                }
            }
        }
    }
}
