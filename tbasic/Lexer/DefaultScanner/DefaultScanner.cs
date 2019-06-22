/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Lexer.Tokens;

namespace Tbasic.Lexer
{
    /// <summary>
    /// The default implementation of Scanner. This can be extended and modified for custom implementations.
    /// </summary>
    internal partial class DefaultScanner : IScanner
    {
        private readonly StringStream stream;
        private readonly List<ITokenFactory> tokens = new List<ITokenFactory>();

        public DefaultScanner(StringStream stream)
        {
            this.stream = stream;
        }

        public DefaultScanner(string str)
            : this(new StringStream(str))
        {
        }

        public int Position { get => (int)stream.Position; set => stream.Position = value; }

        public int Length => (int)stream.Length;

        public bool EndOfStream => stream.Peek() == -1;

        public IToken Next()
        {
            int c;
            while ((c = stream.Read()) != -1 && c != '\n' && char.IsWhiteSpace((char)c)) ;

            if (EndOfStream)
                throw new EndOfStreamException();

            int maxRead = 0;
            IToken found = null;
            for (int idx = 0; idx < tokens.Count; ++idx) { // maximal munch
                int read = tokens[idx].MatchToken(stream, out IToken token);
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

        public IScanner Scan(StringStream stream)
        {
            return new DefaultScanner(stream);
        }

        public void Skip(int count)
        {
            stream.Seek(count, SeekOrigin.Current);
        }
    }
}
