/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using Slang.Components;
using Slang.Errors;
using Slang.Lexer;
using Slang.Runtime;
using Slang.Lexer.Scanners.TBasic;

namespace Slang.Lexer.Scanners
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

        public Token[] Tokenize()
        {
            if (EndOfStream)
                throw new EndOfStreamException();
            List<Token> tokens = new List<Token>();
            Token? next;
            while ((next = Next()) != null)
                tokens.Add(next.Value);
            return tokens.ToArray();
        }

        public Token? Next()
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

        public void RegisterToken<T>() where T : ITokenFactory
        {
            ConstructorInfo ctor = typeof(T).TypeInitializer;
            factories.Add((ITokenFactory)ctor.Invoke(new object[0]));
        }

        public IScanner Scan(StringStream stream, Scope scope)
        {
            return new DefaultScanner(stream, scope);
        }

        public void Skip(int count)
        {
            stream.Seek(count, SeekOrigin.Current);
        }
    }
}
