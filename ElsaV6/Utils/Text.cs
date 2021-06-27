using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ElsaV6.Utils
{
    public class Text
    {
        public static string ToLowerAlphaNum(string text)
        {
            return Regex.Replace(text.ToLower(), @"[^A-Za-z0-9]", "");
        }
    }
}
