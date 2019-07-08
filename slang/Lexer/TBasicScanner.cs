/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;
using Slang.Components;
using Slang.Errors;
using Slang.Runtime;

namespace Slang.Lexer
{
    /// <summary>
    /// The default implementation of Scanner. This can be extended and modified for custom implementations.
    /// </summary>
    internal partial class TBasicScanner : IScanner
    {
        private readonly StringStream stream;
        private readonly List<ITokenFactory> factories = new List<ITokenFactory>();
        public Scope Scope { get; private set; }

        public TBasicScanner(StringStream stream, Scope scope)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));
            Contract.EndContractBlock();
            this.stream = stream;
            Scope = scope;
        }

        public TBasicScanner(string str, Scope scope)
            : this(new StringStream(str), scope)
        {
        }

        public int Position { get => (int)stream.Position; set => stream.Position = value; }

        public int Length => (int)stream.Length;

        public bool EndOfStream => stream.Peek() == -1;

        public virtual Token[] Tokenize()
        {
            if (EndOfStream)
                throw new EndOfStreamException();
            List<Token> tokens = new List<Token>();
            Token? next;
            while ((next = Next()) != null)
                tokens.Add(next.Value);
            return tokens.ToArray();
        }

        public virtual Token? Next()
        {
            int c;
            while ((c = stream.Read()) != -1 && c != '\n' && char.IsWhiteSpace((char)c)) ;

            if (EndOfStream)
                return null;

            int maxRead = -1;
            Token? found = null;
            int pos = (int)stream.Position;
            for (int idx = 0; idx < factories.Count; ++idx) { // maximal munch
                int read = factories[idx].MatchToken(stream, out Token token);
                if (read == 0)
                    continue;
                stream.Position = pos;
                if (read > maxRead) {
                    maxRead = read;
                    found = token;
                }
                else if (read == maxRead) {
                    found = new Token(found.Value.value, found.Value.Type | token.Type);
                }
            }
            if (found == null)
                throw ThrowHelper.UnknownToken(stream.ReadWord());
            return found;
        }

        public virtual IScanner Scan(StringStream stream, Scope scope)
        {
            return new TBasicScanner(stream, scope);
        }

        public virtual void Skip(int count)
        {
            stream.Seek(count, SeekOrigin.Current);
        }

        protected int CurrentOffset
        {
            get {
                StringStream stream = this.stream;
                StringSegment segment = stream.Value;
                return segment.Offset + (int)stream.Position;
            }
        }

        protected virtual int NextNumber(out Token token)
        {
            StringSegment value = stream.Value;
            int offset = CurrentOffset, count;
            int nLen = value.Length - (int)stream.Position;
            token = default;
            unsafe {
                fixed (char* lpBuff = value.FullString) {
                    count = MatchFast.MatchNumber(&lpBuff[offset], nLen);
                }
            }

            if (count == 0)
                return 0;
            token = new Token(value.Subsegment(offset, count), TokenType.Number);
            return count;
        }

        protected virtual int NextString(out Token token)
        {
            int open = stream.Read();
            token = default;

            if (open != '\'' && open != '\"')
                return 0;

            int c, read = 0;
            StringBuilder sb = new StringBuilder();
            for (; (c = stream.Read()) != -1 && c != open; ++read) {
                if (open == '"' && read == '\\') {
                    switch (c = stream.Read()) {
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
                sb.Append((char)c);
            }
            if (c != open)
                throw ThrowHelper.UnterminatedString();
            token = new Token(sb.ToString(0, sb.Length - 2), TokenType.String);
            return read;
        }

        protected int NextVariable(out Token token)
        {
            StringSegment value = stream.Value;
            int offset = CurrentOffset, count;
            int nLen = value.Length - (int)stream.Position;

            token = default;

            if (!(stream.Peek() == '$' || stream.Peek() == '@'))
                return 0;

            stream.Read();

            if (stream.Peek() == -1)
                return 0;

            unsafe {
                fixed (char* lpBuff = value.FullString) {
                    count = MatchFast.FindAcceptableFuncChars(&lpBuff[offset + 1], nLen);
                }
            }

            token = new Token(value.Subsegment(offset, count), TokenType.Variable);
            return count;
        }

        protected int NextIdentifier(out Token token)
        {
            StringSegment value = stream.Value;
            int offset = CurrentOffset, count;
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

            token = new Token(value.Subsegment(offset, count), TokenType.Identifier);
            return count;
        }
    }
}
