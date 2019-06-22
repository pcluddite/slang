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
            int startIdx = value.Offset;
            int nLen = value.Length;
            int count;

            token = default;
            unsafe {
                fixed(char* lpBuff = value.FullString) {
                    count = MatchVariable(&lpBuff[startIdx], nLen);
                }
            }
            token = new Variable(value.Subsegment(0, count));
            stream.Seek(count, SeekOrigin.Current);
            return count;
        }

        private static unsafe int MatchVariable(char* lpBuff, int nLen)
        {
            int index = 0;
            if (lpBuff[index] == '$' || lpBuff[index] == '@') { // it's a macro
                if (++index >= nLen)
                    return 0;
                index = MatchFast.FindAcceptableFuncChars(lpBuff, index, nLen);
            }
            return index;
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
