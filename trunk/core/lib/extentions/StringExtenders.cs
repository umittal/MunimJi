using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace munimji.core.lib.extentions {
    public static class StringExtenders {
        public static bool IsNotEmptyOrWhiteSpace(this string value)
        {
            return !(String.IsNullOrEmpty(value) && String.IsNullOrWhiteSpace(value));
        }
        public static bool In(this string lhs, IEnumerable<string> values) {
            return values.Contains(lhs);
        }

        public static bool In(this char lhs, IEnumerable<char> values) {
            return values.Contains(lhs);
        }

        public static bool ContainsAny(this string lhs, IEnumerable<char> values) {
            return values.Any(value => lhs.Contains(value));
        }

        public static bool IsNotEmpty(this string value) {
            return !(String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value));
        }

        public static string ToCamelCase(this string phrase)
        {
            return ConvertCase(phrase,StringCases.CamelCase);
        }

        public static string ToTitleCase(this string phrase)
        {
            return ConvertCase(phrase, StringCases.TitleCase);
        }

        public static string ToLowerCase(this string phrase, bool withUnderscore)
        {
            var enumeration = withUnderscore 
                ? StringCases.LowerCase | StringCases.WithUnderscore 
                : StringCases.LowerCase;
            return ConvertCase(phrase, enumeration);

        }

        public static string ToUpperCase(this string phrase, bool withUnderScore)
        {
            var enumeration = withUnderScore
                                  ? StringCases.UpperCase | StringCases.WithUnderscore
                                  : StringCases.UpperCase;
               return ConvertCase(phrase, enumeration);
        }


        /// <summary>
        /// Converts the phrase to specified convention.
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="stringCases">The cases.</param>
        /// <returns>string</returns>
        private static string ConvertCase(string phrase, StringCases stringCases)
        {
            var splittedPhrase = Regex.Split(phrase, "(?=[A-Z][^A-Z])", RegexOptions.Compiled)
                .Where(x=>x.IsNotEmptyOrWhiteSpace()).ToList();

            char[] chars;
            if ((stringCases & StringCases.CamelCase)  == StringCases.CamelCase)
            {
                chars = splittedPhrase[0].ToCharArray();
                chars[0] = Char.ToLower(chars[0]);
                splittedPhrase[0] = new string(chars);
            }
            else if ((stringCases & StringCases.TitleCase) == StringCases.TitleCase)
            {
                chars = splittedPhrase[0].ToCharArray();
                chars[0] = Char.ToUpper(chars[0]);
                splittedPhrase[0] = new string(chars);
            }
            else if ((stringCases & StringCases.LowerCase) == StringCases.LowerCase)
            {
                for (var i = 0; i < splittedPhrase.Count; i++)
                {
                    splittedPhrase[i] = splittedPhrase[i].ToLower();
                }
            }
            else if ((stringCases & StringCases.UpperCase) == StringCases.UpperCase)
            {
                for (var i = 0; i < splittedPhrase.Count; i++)
                {
                    splittedPhrase[i] = splittedPhrase[i].ToUpper();
                }
            }
            var delimiter = "";
            if((stringCases & StringCases.WithUnderscore)==StringCases.WithUnderscore)
            {
                delimiter = "_";
            }
            return String.Join(delimiter, splittedPhrase);
        }

        [Flags]
        private enum StringCases
        {
            TitleCase = 1 << 0,
            CamelCase = 1 << 1,
            LowerCase = 1 << 2,
            UpperCase = 1 << 3,
            WithUnderscore = 1 << 4
        }
    }   

   
}