// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Runtime;

namespace Tbasic.Parsing
{
    /// <summary>
    /// A set of methods for parsing character groups such as strings and characters grouped by parentheses
    /// </summary>
    public static class GroupParser
    {
        /// <summary>
        /// Indexes a string without parsing it (i.e. retreives the index of the terminating quote)
        /// </summary>
        /// <param name="fullstr">the full string containing the string to be checked</param>
        /// <param name="index">the index in the full string of the first quote of the string to be indexed</param>
        /// <returns>the index of the terminating quote in the full string</returns>
        public static int IndexString(string fullstr, int index)
        {
            return IndexString(new StringSegment(fullstr), index);
        }

        internal static int IndexString(StringSegment fullstr, int index)
        {
            char quote = fullstr[index++]; // The first character should be the quote

            for (; index < fullstr.Length; index++) {
                char cur = fullstr[index];
                switch (cur) {
                    case '\n':
                    case '\r':
                        throw ThrowHelper.UnterminatedString();
                    case '\\':
                        index++;
                        cur = fullstr[index];
                        if (index >= fullstr.Length) {
                            throw ThrowHelper.UnterminatedEscapeSequence();
                        }
                        switch (cur) {
                            case 'b':
                            case 't':
                            case 'n':
                            case 'f':
                            case 'r':
                            case '\"':
                            case '\\':
                            case '\'': break; // you're golden
                            case 'u':
                                index += 4;
                                if (index >= fullstr.Length) {
                                    throw ThrowHelper.UnterminatedUnicodeEscape();
                                }
                                break;
                            default:
                                throw ThrowHelper.UnknownEscapeSequence(cur);
                        }
                        break;
                    default:
                        if (cur == quote) { // We be dun
                            return index;
                        }
                        break;
                }
            }
            throw ThrowHelper.UnterminatedString();
        }

        /// <summary>
        /// Indexes and builds a string, parsing any escape sequences properly
        /// </summary>
        /// <param name="fullstr">the full string containing the string to be parsed</param>
        /// <param name="index">the index in the full string of the first quote of the string to be indexed</param>
        /// <param name="s_parsed">the parsed string (without quotes and escape sequences replaced)</param>
        /// <returns>the index of the terminating quote  in the full string</returns>
        public static int ReadString(string fullstr, int index, out string s_parsed)
        {
            return ReadString(new StringSegment(fullstr), index, out s_parsed);
        }

        internal static int ReadString(StringSegment fullstr, int index, out string s_parsed)
        {
            char quote = fullstr[index++]; // The first character should be the quote

            StringBuilder sb = new StringBuilder();
            for (; index < fullstr.Length; index++) {
                char cur = fullstr[index];
                switch (cur) {
                    case '\n':
                    case '\r':
                        throw ThrowHelper.UnterminatedString();
                    case '\\':
                        index++;
                        if (index >= fullstr.Length) {
                            throw ThrowHelper.UnterminatedEscapeSequence();
                        }
                        cur = fullstr[index];
                        switch (cur) {
                            case 'b': sb.Append('\b'); break;
                            case 't': sb.Append('\t'); break;
                            case 'n': sb.Append('\n'); break;
                            case 'f': sb.Append('\f'); break;
                            case 'r': sb.Append('\r'); break;
                            case '\\': sb.Append('\\'); break;
                            case '"': sb.Append('"'); break;
                            case '\'': sb.Append('\''); break;
                            case 'u':
                                index += 4;
                                if (index >= fullstr.Length) {
                                    throw ThrowHelper.UnterminatedUnicodeEscape();
                                }
                                sb.Append((char)ushort.Parse(fullstr.Substring(index - 3, 4), NumberStyles.HexNumber));
                                break;
                            default:
                                throw ThrowHelper.UnknownEscapeSequence(cur);
                        }
                        break;
                    default:
                        if (cur == quote) { // We be dun
                            s_parsed = sb.ToString();
                            return index;
                        }
                        else {
                            sb.Append(cur);
                        }
                        break;
                }
            }
            throw ThrowHelper.UnterminatedString();
        }

        /// <summary>
        /// Indexes a group (set off by parentheses or brackets) without evaluating its operators
        /// </summary>
        /// <param name="fullstr">the full string containing the group to be indexed</param>
        /// <param name="index">the index in the full string of the character of the group to be indexed</param>
        /// <returns>the index of the terminating grouping character in the full string</returns>
        public static int IndexGroup(string fullstr, int index)
        {
            return IndexGroup(new StringSegment(fullstr), index);
        }
        
        internal static int IndexGroup(StringSegment fullstr, int index)
        {
            char c_open = fullstr[index]; // The first character should be the grouping character (i.e. '(' or '[')
            char c_close = c_open == '(' ? ')' : ']';

            int expected = 1; // We are expecting one closing character
            int c_index = index + 1; // We used the first character
            for (; c_index < fullstr.Length; c_index++) {
                char cur = fullstr[c_index];
                switch (cur) {
                    case ' ': // ignore spaces
                        continue;
                    case '\'':
                    case '\"': {
                            c_index = IndexString(fullstr, c_index);
                        }
                        break;
                    case '(':
                    case '[':
                        if (cur == c_open) {
                            expected++;
                        }
                        else {
                            c_index = IndexGroup(fullstr, c_index);
                        }
                        break;
                    default:
                        if (cur == c_close) {
                            expected--;
                        }
                        break;
                }
                if (expected == 0) {
                    return c_index;
                }
            }
            throw ThrowHelper.UnterminatedGroup();
        }

        internal static int ReadGroup(StringSegment fullstr, int index, TBasic exec, out IList<object> args)
        {
            return ReadGroup(fullstr, index, ',', exec, out args);
        }

        internal static int ReadGroup(StringSegment fullstr, int index, char separator, TBasic exec, out IList<object> args)
        {
            List<object> result = new List<object>();
            ExpressionEvaluator eval = new ExpressionEvaluator(exec);
            char c_open = fullstr[index];
            char c_close = c_open == '(' ? ')' : ']';
            int expected = 0;
            int last = index;

            for (; index < fullstr.Length; index++) {
                char cur = fullstr[index];
                switch (cur) {
                    case '\'':
                    case '\"': {
                            index = IndexString(fullstr, index);
                        }
                        break;
                    case '(':
                    case '[':
                        if (cur == c_open) {
                            expected++;
                        }
                        else {
                            index = IndexGroup(fullstr, index);
                        }
                        break;
                    default:
                        if (cur == c_close) {
                            expected--;
                        }
                        break;
                }

                if ((expected == 1 && cur == separator) // The separators in between other parentheses are not ours.
                    || expected == 0) {
                    StringSegment _param = fullstr.Subsegment(last + 1, index - last - 1).Trim();
                    if (!StringSegment.Equals(_param, "")) {
                        result.Add(eval.Evaluate(_param));
                    }
                    last = index;
                    if (expected == 0) { // fin
                        args = result;
                        return last;
                    }
                }
            }
            throw ThrowHelper.UnterminatedGroup();
        }
    }
}
