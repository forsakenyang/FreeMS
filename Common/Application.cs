using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace FreeMS
{
    public static class Application
    {
        public static readonly Random Random = new Random();
        public const int DefaultBufferSize = 8192;
        public const short MapleVersion = 79;
        public const string PatchVersion = "";
        public const string CommandIndicator = "!";
        public const string PlayerCommandIndicator = "@";

        public static string LaunchPath
        {
            get
            {
                return Directory.GetCurrentDirectory() + @"\";
            }
        }

        public static string ExecutablePath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\";
            }
        }

        public static string ToCamel(this string value)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        public static bool IsAlphaNumeric(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (char c in value)
            {
                if (!char.IsLetter(c) && !char.IsNumber(c))
                {
                    return false;
                }
            }

            return true;
        }

        public static string ClearFormatters(this string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
