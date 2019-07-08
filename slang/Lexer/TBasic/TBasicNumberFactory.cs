/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Slang.Components;
using Slang.Runtime;

namespace Slang.Lexer.TBasic
{
    public class TBasicNumberFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, out Token token)
        {
            StringSegment value = stream.Value;
            int offset = value.Offset + (int)stream.Position, count;
            int nLen = value.Length - (int)stream.Position;

            token = default;

            unsafe {
                fixed (char* lpBuff = value.FullString) {
                    count = MatchNumber(&lpBuff[offset], nLen);
                }
            }

            if (count == 0)
                return 0;

            token = new Token(this, value.Subsegment(offset, count), TokenType.Number);
            stream.Seek(count, SeekOrigin.Current);
            return count;
        }

        private unsafe static int MatchNumber(char* buff, int nLen)
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
    }
}
