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
    internal partial class DefaultScanner : AbstractScanner
    {
        protected static readonly Regex rxNumeric = new Regex(@"^((?:[0-9]+)?(?:\.[0-9]+)?(?:[eE]-?[0-9]+)?)", RegexOptions.Compiled);
        protected static readonly Regex rxHex = new Regex(@"^(0x([0-9a-fA-F]+))", RegexOptions.Compiled);
        protected static readonly Regex rxId = new Regex(@"^((_|[a-zA-Z])\w+)", RegexOptions.Compiled);
        
        public DefaultScanner(string buffer)
        {
            InternalBuffer = buffer;
        }

        public DefaultScanner()
        {
        }

        public override bool NextNumber(out Number num)
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

        public override bool NextHexadecimal(out long hex)
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

        public override bool Next(string pattern, bool ignoreCase)
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

        public override bool NextStringOrToken(out IEnumerable<char> token)
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

        public override bool NextString(out string parsed)
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

        public override bool SkipString()
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

        public override bool NextGroup(out IList<IEnumerable<char>> args)
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

        public override bool SkipGroup()
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
        
        public override bool NextValidIdentifier(out IEnumerable<char> name)
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

        public override bool NextFunction(out IEnumerable<char> name, out IList<IEnumerable<char>> args)
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

        public override bool NextVariable(out IEnumerable<char> name, out IList<IEnumerable<char>> indices)
        {
            name = null; indices = null;
            string token = BuffNextWord();
            if (string.IsNullOrEmpty(token))
                return false;
            Match m = rxId.Match(token);
            if (m.Success && CharAt(TokenBuffer.Item1 + m.Length) == '$') {
                name = InternalBuffer.Substring(TokenBuffer.Item1, m.Length + 1);
                AdvanceScanner(token.Length + 1);
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

        public override bool NextIndices(out IList<IEnumerable<char>> indices)
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

        public override bool NextBool(out bool b)
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

        public override bool NextBinaryOp(ObjectContext context, out BinaryOperator foundOp)
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

        public override bool NextUnaryOp(ObjectContext context, object last, out UnaryOperator foundOp)
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
        
        public override IScanner Scan(IEnumerable<char> buffer)
        {
            return new DefaultScanner(buffer.ToString());
        }
    }
}
