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
    /// <summary>
    /// Static class of token types. Token IDs act as flags in case a token matches multiple types.
    /// Arbitrary tokens can be created but note that all token IDs greater than 4026531840 (0xF0000000)
    /// are expected to be reserved
    /// </summary>
    [Flags]
    public enum TokenType : uint
    {
        None            = 0x00000000,
        Character       = 0x00000001,
        Date            = 0x00000002,
        Identifier      = 0x00000004,
        Keyword         = 0x00000008,
        Number          = 0x00000010,
        Operator        = 0x00000020,
        String          = 0x00000040,
        UnsignedNumber  = 0x00000080 | Number,

        Int8            = 0x00000100 | Number,
        Int16           = 0x00000200 | Number,
        Int32           = 0x00000400 | Number,
        Int64           = 0x00000800 | Number,

        BinaryOperator  = 0x00001000 | Operator,
        UnaryOperator   = 0x00002000 | Operator,
        TernaryOperator = 0x00004000 | Operator,

        UInt8           = Int8 | UnsignedNumber,
        UInt16          = Int16 | UnsignedNumber,
        UInt32          = Int32 | UnsignedNumber,
        UInt64          = Int64 | UnsignedNumber
    }

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
        public TokenType Type { get; private set; }
        public ITokenFactory Factory { get; private set; }

        public bool Ambiguous => !Enum.IsDefined(typeof(TokenType), Type);

        public Token(string str)
            : this(str, TokenType.None)
        {
        }

        public Token(string str, TokenType type)
            : this(str, type, null)
        {
        }

        public Token(string str, Token[] subtokens)
            : this(str, TokenType.None, subtokens)
        {
        }

        public Token(string str, TokenType type, Token[] subtokens)
            : this(null, str, type, subtokens)
        {
        }

        public Token(ITokenFactory factory, string str)
            : this(factory, str, TokenType.None)
        {
        }

        public Token(ITokenFactory factory, string str, TokenType type)
            : this(factory, str, type, null)
        {
        }

        public Token(ITokenFactory factory, string str, Token[] subtokens)
            : this(factory, str, TokenType.None, subtokens)
        {
        }

        public Token(ITokenFactory factory, string str, TokenType type, Token[] subtokens)
            : this(factory, new StringSegment(str), type, subtokens)
        {
        }

        internal Token(StringSegment str)
            : this(str, TokenType.None)
        {
        }

        internal Token(StringSegment str, TokenType type)
            : this(str, type, null)
        {
        }

        internal Token(StringSegment str, Token[] subtokens)
            : this(str, TokenType.None, subtokens)
        {
        }

        internal Token(StringSegment str, TokenType type, Token[] subtokens)
            : this(null, str, type, subtokens)
        {
        }

        internal Token(ITokenFactory factory, StringSegment str)
            : this(factory, str, TokenType.None)
        {
        }

        internal Token(ITokenFactory factory, StringSegment str, TokenType type)
            : this(factory, str, type, null)
        {
        }

        internal Token(ITokenFactory factory, StringSegment str, Token[] subtokens)
            : this(factory, str, TokenType.None, subtokens)
        {
        }

        internal Token(ITokenFactory factory, StringSegment str, TokenType type, Token[] subtokens)
        {
            Factory = factory;
            text = str;
            Type = type;
            tokens = subtokens;
        }

        public bool Equals(Token other)
        {
            if (other.Type != Type)
                return false;
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
            return ((object)tokens ?? 0).GetHashCode() ^ HasSubtokens.GetHashCode() ^ ((object)text ?? "").GetHashCode() | Type.GetHashCode();
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
