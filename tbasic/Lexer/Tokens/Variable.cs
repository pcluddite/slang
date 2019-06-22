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
    public class VariableFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, out IToken token)
        {
            StringSegment value = stream.Value;
            int offset = value.Offset + (int)stream.Position, count;
            int nLen = value.Length - (int)stream.Position;

            token = default;

            if (!(stream.Peek() == '$' || stream.Peek() == '@'))
                return 0;

            stream.Read();

            if (stream.Peek() == -1)
                return 0;

            unsafe {
                fixed(char* lpBuff = value.FullString) {
                    count = MatchFast.FindAcceptableFuncChars(&lpBuff[offset + 1], nLen);
                }
            }

            token = new Variable(value.Subsegment(offset, count));
            stream.Seek(count, SeekOrigin.Current);
            return count;
        }
    }

    public struct Variable : IToken
    {
        private readonly StringSegment value;

        public IEnumerable<char> Text => value;
        public object Native => value.ToString();

        public Variable(IEnumerable<char> text)
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

        internal Variable(StringSegment value)
        {
            this.value = value;
        }
    }
}
