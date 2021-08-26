using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Facebook
{
    public static class Base64Url
    {
        public static string Encode(byte[] b) => Convert.ToBase64String(b).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        public static string Encode(string s) => Encode(Encoding.UTF8.GetBytes(s));

        public static byte[] DecodeToArray(string s)
        {
            string incoming = s.Replace('_', '/').Replace('-', '+');
            switch (s.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            return Convert.FromBase64String(incoming);

        }
        public static string Decode(string s) => Encoding.UTF8.GetString(DecodeToArray(s));
    }

    public static class Utility
    {
        public static T DeserializeJson<T>(string json, T obj, JsonSerializerOptions options = default)
        => JsonSerializer.Deserialize<T>(json, options);

    }
}
