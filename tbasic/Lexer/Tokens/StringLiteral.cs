/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Tbasic.Errors;

namespace Tbasic.Lexer.Tokens
{
    public class StringLiteral : IToken
    {
        private List<char> value;
        public IEnumerable<char> Text
        {
            get {
                if (value == null)
                    throw new NullReferenceException();
                return value;
            }
        }

        public int Match(StreamReader reader)
        {
            int open = reader.Peek();
            if (open != '\'' && open != '\"')
                return 0;
            int c;
            value = new List<char>();
            for(int read = 0; (c = reader.Read()) != -1; ++read) {
                if (open == '"' && read == '\\') { 
                    switch(c = reader.Read()) {
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
                            if (reader.Read(buff, 0, buff.Length) != buff.Length)
                                throw ThrowHelper.UnterminatedUnicodeEscape();
                            c = (char)ushort.Parse(new string(buff), NumberStyles.HexNumber);
                            break;
                    }
                }
                value.Add((char)c);
                if (c == open)
                    return read;
            }
            throw ThrowHelper.UnterminatedString();
        }

        public bool Pack()
        {
            value?.TrimExcess();
            return true;
        }
    }
}
