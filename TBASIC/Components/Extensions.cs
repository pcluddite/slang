// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic
{
    internal static class Extensions
    {
        private const StringComparison ComparisonType = StringComparison.OrdinalIgnoreCase;

        public static bool EqualsIgnoreCase(this string initial, string other)
        {
            return initial.Equals(other, ComparisonType);
        }

        public static bool EndsWithIgnoreCase(this string initial, string other)
        {
            return initial.EndsWith(other, ComparisonType);
        }

        public static int IndexOfIgnoreCase(this string initial, string other, int start)
        {
            return initial.IndexOf(other, start, ComparisonType);
        }

        public static int SkipWhiteSpace(this string str, int start = 0)
        {
            for(int index = start; index < str.Length; ++index) {
                if (!char.IsWhiteSpace(str[index])) {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Removes the first instance of a char onward. If the char isn't found, returns the original string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string RemoveFromChar(this string str, char c)
        {
            int char_index = str.IndexOf(c);
            if (char_index > -1) {
                return str.Remove(char_index);
            }
            else {
                return str;
            }
        }

        /// <summary>
        /// Removes the first instance of a char and everything before it. If the char isn't found, returns an empty string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string RemoveToChar(this string str, char c)
        {
            int char_index = str.IndexOf(c);
            if (char_index < 0) {
                return string.Empty;
            }
            else {
                return str.Substring(0, char_index);
            }
        }
    }
}
