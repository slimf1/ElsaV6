using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DataAccess
{
    public class Utils
    {
        public static string ToLowerAlphaNum(string text)
        {
            return Regex.Replace(text.ToLower(), @"[^A-Za-z0-9]", "");
        }

        public static string ReadFile(string path)
        {
            string fileContent;
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    fileContent = streamReader.ReadToEnd();
                }
            }

            return fileContent;
        }
    }
}
