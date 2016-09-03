// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Tbasic.Runtime;
using Tbasic.Types;

namespace Tbasic.Parsing
{
    /// <summary>
    /// The default implementation of Scanner
    /// </summary>
    internal partial class DefaultScanner : IScanner
    {
        protected static readonly Regex rxNumeric = new Regex(@"^((?:[0-9]+)?(?:\.[0-9]+)?(?:[eE]-?[0-9]+)?)", RegexOptions.Compiled);
        protected static readonly Regex rxHex = new Regex(@"^(0x([0-9a-fA-F]+))", RegexOptions.Compiled);
        protected static readonly Regex rxId = new Regex(@"^((_|[a-zA-Z])\w+)", RegexOptions.Compiled); protected Tuple<int, string> TokenBuffer = null;

        /// <summary>
        /// The internal buffer for this scanner
        /// </summary>
        protected string InternalBuffer;

        /// <summary>
        /// Gets a value indicating whether the end of the stream has been reached
        /// </summary>
        public bool EndOfStream
        {
            get {
                return Position >= InternalBuffer.Length;
            }
        }

        /// <summary>
        /// Gets or sets the current position of the stream as an integer
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets the length of this stream as an integer
        /// </summary>
        public int Length
        {
            get {
                return InternalBuffer.Length;
            }
        }

        public virtual int Current
        {
            get { return CharAt(Position); }
        }

        protected virtual int CharAt(int pos)
        {
            if (EndOfStream)
                return -1;
            return InternalBuffer[pos];
        }

        /// <summary>
        /// Skips all leading whitespace
        /// </summary>
        public virtual void SkipWhiteSpace()
        {
            if (EndOfStream)
                return;
            Position = FindNonWhiteSpace();
        }

        protected virtual int FindNonWhiteSpace()
        {
            int pos = Position;
            while (pos < Length && char.IsWhiteSpace(InternalBuffer[pos])) {
                ++pos;
            }
            return pos;
        }

        /// <summary>
        /// Gets the next token from the buffer. If the next token is not buffered, then it will be.
        /// </summary>
        protected string BuffNextWord()
        {
            int start = FindNonWhiteSpace(),
                pos = start;
            if (TokenBuffer == null || TokenBuffer.Item1 != pos) {
                while (pos < Length && !char.IsWhiteSpace(InternalBuffer[pos])) {
                    ++pos;
                }
                if (pos - start > 0) {
                    TokenBuffer = new Tuple<int, string>(start, InternalBuffer.Substring(start, pos - start));
                }
                else {
                    return null;
                }
            }
            return TokenBuffer.Item2;
        }

        /// <summary>
        /// Advances the scanner to the end of the next token
        /// </summary>
        protected void AdvanceScanner(string matchedToken)
        {
            AdvanceScanner(matchedToken.Length);
        }

        /// <summary>
        /// Advances the scanner to the end of the next token
        /// </summary>
        protected void AdvanceScanner(int tokenLen)
        {
            Position = tokenLen + TokenBuffer.Item1;
        }

        /// <summary>
        /// Gets the next token in the buffer
        /// </summary>
        public virtual IEnumerable<char> Next()
        {
            return NextSegment();
        }

        /// <summary>
        /// Gets the next token in the buffer as a IEnumerable&lt;char&gt;
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<char> NextSegment()
        {
            string word = BuffNextWord();
            if (word != null) {
                AdvanceScanner(word);
            }
            return word;
        }

        public DefaultScanner(string buffer)
        {
            InternalBuffer = buffer;
        }

        public DefaultScanner()
        {
        }

        public bool NextNumber(out Number num)
        {
            Match m = rxNumeric.Match(BuffNextWord() ?? string.Empty);
            if (m.Success && Number.TryParse(m.Value, out num)) {
                AdvanceScanner(m.Value);
                return true;
            }
            else {
                num = default(Number);
                return false;
            }
        }

        public bool NextHexadecimal(out long hex)
        {
            Match m = rxHex.Match(BuffNextWord() ?? string.Empty);
            if (m.Success) {
                hex = Convert.ToInt64(m.Value, 16);
                AdvanceScanner(m.Value);
                return true;
            }
            else {
                hex = default(long);
                return false;
            }
        }

        public bool Next(string pattern, bool ignoreCase)
        {
            string token = BuffNextWord();
            if (!string.IsNullOrEmpty(token) && token.StartsWith(pattern, ignoreCase, CultureInfo.CurrentCulture)) {
                AdvanceScanner(pattern);
                return true;
            }
            else {
                return false;
            }
        }

        public bool NextStringOrToken(out IEnumerable<char> token)
        {
            token = null;
            int pos = FindNonWhiteSpace();
            if (pos >= Length)
                return false;
            string sztoken = null;
            if (IsQuote(CharAt(pos))) {
                int end = IndexString(InternalBuffer, pos) + 1;
                token = sztoken = InternalBuffer.Substring(pos, end - pos);
                Position = pos + sztoken.Length;
            }
            else {
                token = sztoken = BuffNextWord();
                AdvanceScanner(sztoken);
            }
            return (sztoken != null);
        }

        public bool NextString(out string parsed)
        {
            int pos = FindNonWhiteSpace();
            if (pos >= Length || !IsQuote(InternalBuffer[pos])) {
                parsed = null;
            }
            else {
                Position = ReadString(InternalBuffer, pos, out parsed) + 1;
            }
            return (parsed != null);
        }

        public bool SkipString()
        {
            int pos = FindNonWhiteSpace();
            if (pos >= Length || !IsQuote(InternalBuffer[pos])) {
                return false;
            }
            else {
                Position = IndexString(InternalBuffer, pos);
                return true;
            }
        }

        protected static bool IsGroupChar(int c)
        {
            return (c == '(' || c == '[');
        }

        protected static bool IsQuote(int c)
        {
            return (c == '\"' || c == '\'');
        }

        public bool NextGroup(out IList<IEnumerable<char>> args)
        {
            int start = FindNonWhiteSpace();
            int pos = GetGroup(start, out args);
            if (pos != start) {
                Position = pos;
                return true;
            }
            return false;
        }

        protected int GetGroup(int pos, out IList<IEnumerable<char>> args)
        {
            if (pos >= Length || !IsGroupChar(InternalBuffer[pos])) {
                args = null;
            }
            else {
                pos = ReadGroup(InternalBuffer, pos, out args) + 1;
            }
            return pos;
        }

        public bool SkipGroup()
        {
            int pos = FindNonWhiteSpace();
            if (pos >= Length || !IsGroupChar(InternalBuffer[pos])) {
                return false;
            }
            else {
                Position = IndexGroup(InternalBuffer, pos) + 1;
                return true;
            }
        }
        
        public bool NextValidIdentifier(out IEnumerable<char> name)
        {
            Match m = rxId.Match(BuffNextWord() ?? string.Empty);
            if (m.Success) {
                name = m.Value;
                AdvanceScanner(m.Length);
            }
            else {
                name = null;
            }
            return (name != null);
        }

        public bool NextFunction(out IEnumerable<char> name, out IList<IEnumerable<char>> args)
        {
            int start = Position;
            if (NextValidIdentifier(out name) && NextGroup(out args)) {
                return true;
            }
            else {
                Position = start;
                args = null;
                return false;
            }
        }

        public bool NextVariable(out IEnumerable<char> name, out IList<IEnumerable<char>> indices)
        {
            name = null; indices = null;
            string token = BuffNextWord();
            if (string.IsNullOrEmpty(token))
                return false;
            Match m = rxId.Match(token);
            if (m.Success && CharAt(TokenBuffer.Item1 + m.Length) == '$') {
                name = InternalBuffer.Substring(TokenBuffer.Item1, m.Length + 1);
                AdvanceScanner(m.Length + 1);
                NextIndices(out indices);
            }
            else if (token[0] == '@') {
                return NextMacro(out name, out indices);
            }
            return (name != null);
        }

        /// <summary>
        /// This assumes that the first char has already been checked to be an '@'!
        /// </summary>
        private bool NextMacro(out IEnumerable<char> name, out IList<IEnumerable<char>> indices)
        {
            name = null; indices = null;
            int start = Position++;
            string token = BuffNextWord();
            if (string.IsNullOrEmpty(token))
                return false;
            Match m = rxId.Match(token);
            if (m.Success) {
                name = InternalBuffer.Substring(start, token.Length + 1);
                AdvanceScanner(token.Length + 1);
                NextIndices(out indices);
                return true;
            }
            else {
                Position = start;
                return false;
            }
        }

        public bool NextIndices(out IList<IEnumerable<char>> indices)
        {
            int pos = Position;
            if (Current == '[' && NextGroup(out indices)) {
                return true;
            }
            else {
                Position = pos;
                indices = null;
                return false;
            }
        }
        
        private static unsafe int FindAcceptableFuncChars(string expr, int start)
        {
            fixed (char* lpseg = expr)
            {
                int len = expr.Length;
                int index = start;
                for (; index < len; ++index) {
                    if (!char.IsLetterOrDigit(lpseg[index]) && lpseg[index] != '_') {
                        return index;
                    }
                }
                return index;
            }
        }

        internal static bool NextFunctionInternal(IScanner scanner, TBasic exec, out Function func)
        {
            IEnumerable<char> name;
            IList<IEnumerable<char>> args;
            if (scanner.NextFunction(out name, out args)) {
                func = new Function(exec, name.ToString(), args);
            }
            else {
                func = null;
            }
            return (func != null);
        }

        internal static bool NextVariable(IScanner scanner, TBasic exec, out Variable variable)
        {
            IEnumerable<char> name;
            IList<IEnumerable<char>> indices;
            if (scanner.NextVariable(out name, out indices)) {
                variable = new Variable(name.ToString(), indices, exec);
            }
            else {
                variable = null;
            }
            return (variable != null);
        }

        public bool NextBool(out bool b)
        {
            string token = BuffNextWord();
            if (string.IsNullOrEmpty(token))
                return b = false;

            if (token.StartsWith(bool.TrueString, StringComparison.CurrentCultureIgnoreCase)) {
                AdvanceScanner(bool.TrueString);
                return b = true;
            }
            else if (token.StartsWith(bool.FalseString, StringComparison.CurrentCultureIgnoreCase)) {
                AdvanceScanner(bool.FalseString);
                b = false;
                return true;
            }
            return b = false;
        }

        public bool NextBinaryOp(ObjectContext context, out BinaryOperator foundOp)
        {
            foundOp = default(BinaryOperator);
            string token = BuffNextWord();
            if (string.IsNullOrEmpty(token))
                return false;

            if (MatchOperator(token, context, out foundOp)) {
                AdvanceScanner(foundOp.OperatorString.Length);
                return true;
            }
            return false;
        }

        public bool NextUnaryOp(ObjectContext context, object last, out UnaryOperator foundOp)
        {
            foundOp = default(UnaryOperator);
            if (!(last == null || last is BinaryOperator)) // unary operators can really only come after a binary operator or the beginning of the expression
                return false;

            string token = BuffNextWord();
            if (string.IsNullOrEmpty(token))
                return false;

            if (MatchOperator(token, context, out foundOp)) {
                AdvanceScanner(foundOp.OperatorString.Length);
                return true;
            }

            return false;
        }

        private static bool MatchOperator<T>(string token, ObjectContext context, out T foundOp) where T : IOperator
        {
            string foundStr = null;
            foundOp = default(T);
            foreach (var op in context.GetAllOperators<T>()) {
                string opStr = op.OperatorString;
                if (token.StartsWith(opStr, StringComparison.CurrentCultureIgnoreCase)) {
                    foundOp = op;
                    foundStr = opStr;
                }
            }
            return foundStr != null;
        }
        
        public virtual IScanner Scan(IEnumerable<char> buffer)
        {
            return new DefaultScanner(buffer.ToString());
        }

        public virtual IEnumerable<char> Range(int start, int count)
        {
            return InternalBuffer.Substring(start, count);
        }

        public virtual IEnumerable<char> Range(int start)
        {
            return InternalBuffer.Substring(start);
        }

        public void Skip(int count)
        {
            Position += count;
        }

        /// <summary>
        /// Converts this scanner's buffer to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return InternalBuffer.ToString();
        }
    }
}
