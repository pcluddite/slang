/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System.Collections.Generic;
using System.IO;
using System.Text;
using Slang.Components;
using Slang.Runtime;

namespace Slang.Lexer.Tokens
{
    public class IdentifierFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, Scope scope, out IToken token)
        {
            StringSegment value = stream.Value;
            int offset = value.Offset + (int)stream.Position, count;
            int nLen = value.Length - (int)stream.Position;
            token = default;
            char begin = (char)stream.Peek();
            if (!char.IsLetter(begin) && begin != '_')
                return 0;
            
            unsafe {
                fixed (char* lpStr = value.FullString) {
                    count = MatchFast.FindAcceptableFuncChars(&lpStr[offset], nLen);
                }
            }

            if (count == 0)
                return 0;

            token = new Identifier(value.Subsegment(offset, count));
            stream.Seek(count, SeekOrigin.Current);
            return count;
        }
    }

    public struct Identifier : IToken
    {
        private readonly StringSegment value;

        public IEnumerable<char> Text => value;
        public object Native => value.ToString();

        public Identifier(IEnumerable<char> text)
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

        internal Identifier(StringSegment value)
        {
            this.value = value;
        }
    }
}
