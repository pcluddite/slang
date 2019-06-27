/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using Slang.Components;
using System;
using System.Collections.Generic;

namespace Slang.Lexer.Tokens
{
    public struct Token : IEquatable<Token>
    {
        private readonly Token[] tokens;
        private readonly StringSegment text;

        public Token[] Subtokens
        {
            get {
                if (tokens == null)
                    throw new NotImplementedException();
                return tokens;
            }
        }

        public bool HasSubtokens => tokens != null;
        public IEnumerable<char> Text => text;
        public bool Ambiguous { get; private set; }

        public bool Equals(Token other)
        {
            if (text != other.text)
                return false;
            if (tokens != other.tokens) {
                if (tokens == null || other.tokens == null)
                    return false;
                if (tokens.Length != other.tokens.Length)
                    return false;
                for (int idx = 0; idx < tokens.Length; ++idx) {
                    if (tokens[idx] != other.tokens[idx])
                        return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return ((object)tokens ?? 0).GetHashCode() ^ HasSubtokens.GetHashCode() ^ ((object)text ?? "").GetHashCode() | Ambiguous.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Token? token = obj as Token?;
            if (token == null)
                return false;
            return Equals(token.Value);
        }

        public static bool operator==(Token a, Token b)
        {
            return a.Equals(b);
        }

        public static bool operator!=(Token a, Token b)
        {
            return !a.Equals(b);
        }
    }
}
