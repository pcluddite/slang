// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Components;
using System.Linq;

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

        internal static IEnumerable<TOutput> TB_ConvertAll<TInput, TOutput>(this IEnumerable<TInput> enumerable, Converter<TInput, TOutput> conversion)
        {
            foreach (TInput item in enumerable)
                yield return conversion(item);
        }

        internal static IEnumerable<string> TB_ToStrings(this IEnumerable<IEnumerable<char>> enumerable)
        {
            foreach (var item in enumerable)
                yield return item.ToString();
        }

        [Obsolete("", true)]
        internal static IEnumerable<T> TB_Range<T>(this IEnumerable<T> source, int start, int count)
        {
            using (var e = source.GetEnumerator()) {
                while (start >= 0 && e.MoveNext()) start--;
                if (start <= 0) {
                    while (count > 0 && e.MoveNext()) {
                        yield return e.Current;
                        --count;
                    }
                }
            }
        }

        internal static IEnumerable<char> TB_Segment(this string source, int start, int count)
        {
            return new StringSegment(source, start, count);
        }

        internal static bool TB_StartsWith(this IEnumerable<char> source, IEnumerable<char> other, int count, bool ignoreCase = true)
        {
            using (IEnumerator<char> esrc = source.GetEnumerator(), eoth = other.GetEnumerator()) {
                while (esrc.MoveNext() && eoth.MoveNext()) {
                    char a = esrc.Current, b = eoth.Current;
                    if (ignoreCase) {
                        a = char.ToLower(a);
                        b = char.ToLower(b);
                    }
                    if (a != b) {
                        return false;
                    }
                    if (count-- < 0) {
                        return false;
                    }
                }
            }
            return true;
        }

        internal static IEnumerable<char> TB_Trim(this IEnumerable<char> source)
        {
            var result = source.SkipWhile(c => char.IsWhiteSpace(c));
            int whitespace = 0;
            int size = 0;
            char last = ' ';
            foreach(char c in result) {
                if (char.IsWhiteSpace(last)) {
                    if (char.IsWhiteSpace(c)) {
                        ++whitespace;
                    }
                    else {
                        whitespace = 0;
                    }
                }
                last = c;
                ++size;
            }
            return result.Take(size - whitespace);
        }
    }
}
