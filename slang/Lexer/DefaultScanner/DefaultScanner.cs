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
using Slang.Lexer.Tokens;
using Slang.Runtime;

namespace Slang.Lexer
{
    /// <summary>
    /// The default implementation of Scanner. This can be extended and modified for custom implementations.
    /// </summary>
    internal partial class DefaultScanner : IScanner
    {
        private readonly StringStream stream;
        private readonly List<ITokenFactory> tokens = new List<ITokenFactory>();
        public Scope Scope { get; private set; }

        public DefaultScanner(StringStream stream, Scope scope)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));
            Contract.EndContractBlock();
            this.stream = stream;
            Scope = scope;
        }

        public DefaultScanner(string str, Scope scope)
            : this(new StringStream(str), scope)
        {
        }

        public int Position { get => (int)stream.Position; set => stream.Position = value; }

        public int Length => (int)stream.Length;

        public bool EndOfStream => stream.Peek() == -1;

        public IToken[] Tokenize()
        {
            if (EndOfStream)
                throw new EndOfStreamException();
            List<IToken> tokens = new List<IToken>();
            IToken next;
            while ((next = Next()) != null)
                tokens.Add(next);
            return tokens.ToArray();
        }

        public IToken Next()
        {
            int c;
            while ((c = stream.Read()) != -1 && c != '\n' && char.IsWhiteSpace((char)c)) ;

            if (EndOfStream)
                return null;

            int maxRead = 0;
            IToken found = null;
            int pos = (int)stream.Position;
            for (int idx = 0; idx < tokens.Count; ++idx) { // maximal munch
                int read = tokens[idx].MatchToken(stream, out IToken token);
                if (read != 0)
                    stream.Position = pos;
                if (read > maxRead) {
                    maxRead = read;
                    found = token;
                }
            }
            if (found == null)
                throw ThrowHelper.UnknownToken(stream.ReadWord());
            return found;
        }

        public void RegisterToken<T>() where T : ITokenFactory
        {
            ConstructorInfo ctor = typeof(T).TypeInitializer;
            tokens.Add((ITokenFactory)ctor.Invoke(new object[0]));
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
