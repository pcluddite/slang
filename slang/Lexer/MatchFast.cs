﻿/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
namespace Slang.Lexer
{
    internal static unsafe class MatchFast
    {
        public unsafe static int MatchNumber(char* buff, int nLen)
        {
            int index = FindConsecutiveDigits(buff, 0, nLen);
            if (index == 0)
                return 0; // nothing was found

            if (index >= nLen)
                return index;

            if (buff[index] == '.')
                index = FindConsecutiveDigits(buff, ++index, nLen);

            if (index >= nLen)
                return index;

            if (buff[index] == 'e' || buff[index] == 'E') {
                if (buff[++index] == '-' || buff[index] == '+')
                    ++index;
                index = FindConsecutiveDigits(buff, index, nLen);
            }

            return index;
        }

        private static unsafe int FindConsecutiveDigits(char* buff, int start, int nLen)
        {
            int index = start;
            for (; index < nLen; ++index) {
                if (!char.IsDigit(buff[index])) {
                    return index;
                }
            }
            return index;
        }

        public static int MatchHex(string buff)
        {
            int end = 0;
            if (buff.CharAt(end) != '0')
                return -1;
            if (buff.CharAt(++end) != 'x' && buff.CharAt(end) != 'X')
                return -1;

            end = FindConsecutiveHex(buff, ++end);

            return end;
        }

        private static unsafe int FindConsecutiveHex(string buff, int start)
        {
            fixed (char* lpseg = buff) {
                int len = buff.Length;
                int index = start;
                for (; index < len; ++index) {
                    if (!IsHexDigit(lpseg[index])) {
                        return index;
                    }
                }
                return index;
            }
        }
        private static bool IsHexDigit(char c)
        {
            return char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
        }


        public static unsafe int FindAcceptableFuncChars(char* lpBuff, int nLen)
        {
            int index  = 0;
            for (; index < nLen; ++index) {
                if (!char.IsLetterOrDigit(lpBuff[index]) && lpBuff[index] != '_') {
                    return index;
                }
            }
            return index;
        }

        internal static unsafe bool StartsWithFast(string s, string pattern, bool ignoreCase)
        {
            if (s == null || pattern == null)
                return false;
            if (pattern.Length > s.Length)
                return false;
            
            fixed(char* lpstr = s) fixed(char* lppat = pattern) {
                for (int i = 0, len = pattern.Length; i < len; ++i) {
                    char a, b;
                    if (ignoreCase) {
                        a = char.ToLower(lpstr[i]);
                        b = char.ToLower(lppat[i]);
                    }
                    else {
                        a = lpstr[i];
                        b = lppat[i];
                    }
                    if (a != b)
                        return false;
                }
            }
            return true;
        }
    }
}
