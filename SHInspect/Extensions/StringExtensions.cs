using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SHInspect.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Replace(Environment.NewLine, " ").Replace('\r', ' ').Replace('\n', ' ');
        }
        public static string SpaceAtCamelCase(string value)
        {
            string strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);
            string strTargetString = value;
            string strReplace = @" $1$2";

            return myRegex.Replace(strTargetString, strReplace);
        }
    }
}
