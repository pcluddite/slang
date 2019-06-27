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
    public static class TokenType
    {
        public const uint NONE             = 0x00000000;
        public const uint NUMBER           = 0xF0000001;
        public const uint STRING           = 0xF0000002;
        public const uint IDENTIFIER       = 0xF0000004;
        public const uint KEYWORD          = 0xF0000008;
        public const uint VARIABLE         = 0xF0000010;
        public const uint OPERATOR         = 0xF0000020;
        public const uint BINARY_OPERATOR  = 0xF0000060; // OPERATOR | 0xF0000040
        public const uint UNARY_OPERATOR   = 0xF00000A0; // OPERATOR | 0xF0000080
        public const uint TERNARY_OPERATOR = 0xF0000120; // OPERATOR | 0xF0000100
        public const uint CHARACTER        = 0xF0000200;
        public const uint INT8             = 0xF0000401;
        public const uint INT16            = 0xF0000801;
        public const uint INT32            = 0xF0001001;
        public const uint INT64            = 0xF0002001;
        public const uint UNSIGNED_NUMBER  = 0xF0004001;
        public const uint UINT8            = 0xF0004401; // INT8 | UNSIGNED_NUMBER
        public const uint UINT16           = 0xF0004801; // INT16 | UNSIGNED_NUMBER
        public const uint UINT32           = 0xF0005001; // INT32 | UNSIGNED_NUMBER
        public const uint UINT64           = 0xF0006001; // INT64 | UNSIGNED_NUMBER
        public const uint DATE             = 0xF0008001;
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
        public bool Ambiguous { get; private set; }
        public uint Type { get; private set; }
        public ITokenFactory Factory { get; private set; }

        public Token(string str)
            : this(str, TokenType.NONE)
        {
        }

        public Token(string str, uint type)
            : this(str, type, null)
        {
        }

        public Token(string str, Token[] subtokens)
            : this(str, TokenType.NONE, subtokens)
        {
        }

        public Token(string str, uint type, Token[] subtokens)
            : this(null, str, type, subtokens)
        {
        }

        public Token(ITokenFactory factory, string str)
            : this(factory, str, TokenType.NONE)
        {
        }

        public Token(ITokenFactory factory, string str, uint type)
            : this(factory, str, type, null)
        {
        }

        public Token(ItokenFactory factory, string str, Token[] subtokens)
            : this(factory, str, TokenType.NONE, subtokens)
        {
        }

        public Token(ITokenFactory factory, string str, uint type, Token[] subtokens)
            : this(factory, new StringSegment(str), type, subtokens)
        {
        }

        internal Token(StringSegment str)
            : this(str, TokenType.NONE)
        {
        }

        internal Token(StringSegment str, uint type)
            : this(str, type, null)
        {
        }

        internal Token(StringSegment str, Token[] subtokens)
            : this(str, TokenType.NONE, subtokens)
        {
        }

        internal Token(StringSegment str, uint type, Token[] subtokens)
            : this(null, str, type, subtokens)
        {
        }

        internal Token(ITokenFactory factory, StringSegment str)
            : this(factory, str, TokenType.NONE)
        {
        }

        internal Token(ITokenFactory factory, StringSegment str, uint type)
            : this(factory, str, type, null)
        {
        }

        internal Token(ITokenFactory factory, StringSegment str, Token[] subtokens)
            : this(factory, str, TokenType.NONE, subtokens)
        {
        }

        internal Token(ITokenFactory factory StringSegment str, uint type, Token[] subtokens)
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
