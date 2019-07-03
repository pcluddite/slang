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

namespace Slang.Lexer.Tokens
{
    public class IdentifierFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, out Token token)
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

            token = new Token(this, value.Subsegment(offset, count), TokenType.Identifier);
            stream.Seek(count, SeekOrigin.Current);
            return count;
        }
    }
}
