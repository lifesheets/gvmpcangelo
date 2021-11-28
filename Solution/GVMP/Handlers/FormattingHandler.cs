using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GVMP
{
    public static class FormattingHandler
    {
        public static string GetRGBStr(this Faction faction)
        {
            return "rgb(" + faction.RGB.Red + ", " + faction.RGB.Green + ", " + faction.RGB.Blue + ")";
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string ToDots(this int val)
        {
            var nfi = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };
            string res = val.ToString("#,##0", nfi);

            return res;
        }
    }
}
