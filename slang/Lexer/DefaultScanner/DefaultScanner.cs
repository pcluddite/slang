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
        private readonly List<ITokenFactory> factories = new List<ITokenFactory>();
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
            IEnumerable<IToken> next;
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

            int maxRead = -1;
            List<IToken> found = new List<IToken>(1);
            int pos = (int)stream.Position;
            for (int idx = 0; idx < factories.Count; ++idx) { // maximal munch
                int read = factories[idx].MatchToken(stream, out IToken token);
                if (read == 0)
                    continue;
                stream.Position = pos;
                if (read > maxRead) {
                    maxRead = read;
                    found.Clear();
                    found.Add(token);
                }
                else if (read == maxRead) {
                    found.Add(token);
                }
            }
            if (found.Count == 0)
                throw ThrowHelper.UnknownToken(stream.ReadWord());
            if (found.Count == 1)
                return found[0];
            return new AmbiguousToken(found);
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
