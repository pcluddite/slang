/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Slang.Components;
using Slang.Errors;
using Slang.Runtime;

namespace Slang.Lexer.Tokens
{
    public class StringLiteralFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, out Token token)
        {
            int open = stream.Peek();
            token = default;

            if (open != '\'' && open != '\"')
                return 0;
			stream.Read();

            int c, read = 0;
            StringBuilder sb = new StringBuilder();
            for(; (c = stream.Read()) != -1 && c != open; ++read) {
                if (open == '"' && read == '\\') {
                    switch(c = stream.Read()) {
                        case -1:
                            throw ThrowHelper.UnterminatedEscapeSequence();
                        case 'a': c = '\a'; break;
                        case 'b': c = '\b'; break;
                        case 'f': c = '\f'; break;
                        case 'n': c = '\n'; break;
                        case 'r': c = '\r'; break;
                        case 't': c = '\t'; break;
                        case 'v': c = '\v'; break;
                        case '\'': c = '\''; break;
                        case '\"': c = '\"'; break;
                        case '\\': c = '\\'; break;
                        case '\0': c = '\0'; break;
                        case 'u':
                            char[] buff = new char[4];
                            if (stream.Read(buff, 0, buff.Length) != buff.Length)
                                throw ThrowHelper.UnterminatedUnicodeEscape();
                            c = (char)ushort.Parse(new string(buff), NumberStyles.HexNumber);
                            break;
                    }
                }
                value.Add((char)c);
            }
			if (c != open)
	            throw ThrowHelper.UnterminatedString();
			token = new Token(value.Substring(0, value.Count - 2), TokenType.STRING);
			return read;
        }
    }
}
