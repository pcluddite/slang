﻿/** +++====+++
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
    public class VariableFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, out Token token)
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

            token = new Token(value.Subsegment(offset, count), TokenType.VARIABLE);
            stream.Seek(count, SeekOrigin.Current);
            return count;
        }
    }
}
