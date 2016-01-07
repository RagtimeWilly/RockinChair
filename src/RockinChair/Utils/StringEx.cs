using System;
using System.Net.Http;
using System.Text;

namespace RockinChair.Utils
{
    internal static class StringEx
    {
        public static string ToBase64(this string s)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(s));
        }

        public static StringContent ToStringContent(this string s)
        {
            return new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
        }
    }
}
