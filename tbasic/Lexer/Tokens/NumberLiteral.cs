/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tbasic.Components;

namespace Tbasic.Lexer.Tokens
{
    public class NumberLiteralFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, out IToken token)
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

            token = new NumberLiteral(value.Subsegment(offset, count));
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
            int len = nLen;
            int index = start;
            for (; index < len; ++index) {
                if (!char.IsDigit(buff[index])) {
                    return index;
                }
            }
            return index;
        }
    }

    public struct NumberLiteral : IToken
    {
        private readonly StringSegment value;

        public IEnumerable<char> Text => value;
        public object Native => double.Parse(value.ToString());

        public NumberLiteral(IEnumerable<char> text)
        {
            string value = text as string;
            if (text == null) {
                StringBuilder sb = new StringBuilder();
                foreach (char c in text)
                    sb.Append(c);
                this.value = new StringSegment(sb.ToString(), 0);
            }
            else {
                this.value = new StringSegment(value);
            }
        }

        internal NumberLiteral(StringSegment value)
        {
            this.value = value;
        }
    }
}
