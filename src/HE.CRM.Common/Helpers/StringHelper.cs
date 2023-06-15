using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HE.CRM.Common.Helpers
{
    public static class StringHelper
    {
        public static string CleanCrmRecordName(string input)
        {
            string output = Regex.Replace(input, @"[\r\n]+|[\n]+\s+", " ");
            return output.Trim();
        }

        public static string CleanSharePointFolderName(string input)
        {
            string output = Regex.Replace(input, @"[\r\n]+|[\n]+\s+", " ");
            output = ReplaceSpecialsCharacters(output);
            return output.Trim();
        }

        public static string ReplaceSpecialsCharacters(string input)
        {
            Dictionary<char, string> replaceMap = new Dictionary<char, string>()
            {
                { '/', "-" },
                { '\\', "-" },
                { '|', "-" },
                { ':', "-" },
                { '*', "" },
                { '~', "" },
                { '"', "" },
                { '#', "" },
                { '%', "" },
                { '{', "" },
                { '}', "" },
                { '>', "" },
                { '<', "" },
                { '.', "" },
                { '?', "" },
                { '&', "-" },
                
            };

            var stringBuilder = new StringBuilder();
            foreach (var character in input)
            {
                if (replaceMap.TryGetValue(character, out var value))
                {
                    stringBuilder.Append(value);
                }
                else
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
