// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Operators;
using Tbasic.Runtime;

namespace Tbasic.Parsing
{
    /// <summary>
    /// The default implementation of Scanner
    /// </summary>
    internal class DefaultScanner : Scanner
    {
        public DefaultScanner(StringSegment buffer)
        {
            InternalBuffer = buffer;
        }

        public override bool NextUnsignedNumber(out Number num)
        {
            int originalPos = IntPosition;
            try {
                num = default(Number);
                if (EndOfStream)
                    return false;
                int endPos = FindConsecutiveDigits(InternalBuffer, IntPosition);
                if (endPos > IntPosition) {
                    if (endPos < InternalBuffer.Length && InternalBuffer[endPos] == '.') {
                        endPos = FindConsecutiveDigits(InternalBuffer, endPos + 1);
                    }
                    if (endPos < InternalBuffer.Length && (InternalBuffer[endPos] == 'e' || InternalBuffer[endPos] == 'E')) {
                        if (InternalBuffer[++endPos] == '-')
                            ++endPos;
                        endPos = FindConsecutiveDigits(InternalBuffer, endPos);
                    }
                }
                else {
                    return false;
                }
                num = Number.Parse(InternalBuffer.Substring(IntPosition, endPos - IntPosition));
                IntPosition = endPos;
                return true;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        public override bool NextHexadecimal(out long number)
        {
            int originalPos = IntPosition;
            try {
                number = default(long);

                int endPos = IntPosition;
                if (EndOfStream || InternalBuffer[endPos++] != '0' ||
                    endPos >= InternalBuffer.Length || InternalBuffer[endPos++] != 'x') {
                    IntPosition = originalPos;
                    return false;
                }
                endPos = FindConsecutiveHex(InternalBuffer, endPos);
                number = Convert.ToInt64(InternalBuffer.Substring(IntPosition, endPos - IntPosition), 16);
                IntPosition = endPos;
                return true;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        private static unsafe int FindConsecutiveDigits(StringSegment expr, int start)
        {
            fixed (char* lpfullstr = expr.FullString)
            {
                char* lpseg = lpfullstr + expr.Offset;
                int len = expr.Length;
                int index = start;
                for (; index < len; ++index) {
                    if (!char.IsDigit(lpseg[index])) {
                        return index;
                    }
                }
                return index;
            }
        }

        private static unsafe int FindConsecutiveHex(StringSegment expr, int start)
        {
            fixed (char* lpfullstr = expr.FullString)
            {
                char* lpseg = lpfullstr + expr.Offset;
                int len = expr.Length;
                int index = start;
                for (; index < len; ++index) {
                    if (!IsHexDigit(lpseg[index])) {
                        return index;
                    }
                }
                return index;
            }
        }

        private static bool IsHexDigit(char c)
        {
            return char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
        }

        public override bool Next(string pattern, bool ignoreCase = true)
        {
            int originalPos = IntPosition;
            try {
                if (EndOfStream || !InternalBuffer.Subsegment(IntPosition).StartsWith(pattern, ignoreCase)) {
                    IntPosition = originalPos;
                    return false;
                }
                IntPosition += pattern.Length;
                return true;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        public override bool NextString(out string parsed)
        {
            int originalPos = IntPosition;
            try {
                if (EndOfStream || (InternalBuffer[IntPosition] != '\"' && InternalBuffer[IntPosition] != '\'')) {
                    parsed = null;
                    IntPosition = originalPos;
                    return false;
                }
                int endPos = GroupParser.ReadString(InternalBuffer, IntPosition, out parsed) + 1;
                IntPosition = endPos;
                return true;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        public override bool NextFunction(Executer exec, out StringSegment name, out StringSegment func, out IList<object> args)
        {
            int originalPos = IntPosition;
            try {
                func = null;
                args = null;
                name = null;
                if (EndOfStream)
                    return false;
                if (char.IsLetter(InternalBuffer[IntPosition]) || InternalBuffer[IntPosition] == '_') {
                    IntPosition = FindAcceptableFuncChars(InternalBuffer, ++IntPosition);
                    if (IntPosition < InternalBuffer.Length) {
                        name = InternalBuffer.Subsegment(originalPos, IntPosition - originalPos);
                        SkipWhiteSpace();
                        if (Next("(")) {
                            IntPosition = GroupParser.ReadGroup(InternalBuffer, IntPosition - 1, exec, out args) + 1;
                            func = InternalBuffer.Subsegment(originalPos, IntPosition - originalPos);
                            return true;
                        }
                    }
                }
                IntPosition = originalPos;
                return false;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        public override bool NextVariable(Executer exec, out StringSegment variable, out StringSegment name, out int[] indices)
        {
            int originalPos = IntPosition;
            try {
                int start = IntPosition;
                variable = null;
                indices = null;
                name = null;
                if (EndOfStream)
                    return false;
                if (char.IsLetter(InternalBuffer[IntPosition]) || InternalBuffer[IntPosition] == '_') {
                    IntPosition = FindAcceptableFuncChars(InternalBuffer, ++IntPosition);
                    if (!EndOfStream && InternalBuffer[IntPosition++] == '$') {
                        name = InternalBuffer.Subsegment(start, IntPosition - start);
                        SkipWhiteSpace();
                        if (!NextIndices(exec, out indices))
                            indices = null;
                        variable = InternalBuffer.Subsegment(originalPos, IntPosition - originalPos);
                        return true;
                    }
                }
                else if (InternalBuffer[IntPosition] == '@') { // it's a macro
                    return NextMacro(exec, out variable, out name, out indices);
                }
                IntPosition = originalPos;
                return false;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        /// <summary>
        /// This assumes that the first char has already been checked to be an '@'!
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="variable"></param>
        /// <param name="indices"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool NextMacro(Executer exec, out StringSegment variable, out StringSegment name, out int[] indices)
        {
            int originalPos = IntPosition;
            try {
                SkipWhiteSpace();
                int start = IntPosition;
                if (++IntPosition < InternalBuffer.Length) {
                    IntPosition = FindAcceptableFuncChars(InternalBuffer, IntPosition);
                    name = InternalBuffer.Subsegment(start, IntPosition - start);
                    SkipWhiteSpace();
                    if (!NextIndices(exec, out indices))
                        indices = null;
                    variable = InternalBuffer.Subsegment(originalPos, IntPosition);
                    return true;
                }
                variable = null;
                name = null;
                indices = null;
                IntPosition = originalPos;
                return false;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        public override bool NextIndices(Executer exec, out int[] indices)
        {
            int originalPos = IntPosition;
            try {
                indices = null;
                if (!EndOfStream && InternalBuffer[IntPosition] == '[') {
                    IList<object> args;
                    IntPosition = GroupParser.ReadGroup(InternalBuffer, IntPosition, exec, out args) + 1;
                    indices = new int[args.Count];
                    for (int i = 0; i < args.Count; ++i) {
                        int? index = args[i] as int?;
                        if (index == null) {
                            throw ThrowHelper.InvalidTypeInExpression(args[i].GetType().Name, typeof(int).Name);
                        }
                        else {
                            indices[i] = index.Value;
                        }
                    }
                    return true;
                }
                IntPosition = originalPos;
                return false;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }
        
        private static unsafe int FindAcceptableFuncChars(StringSegment expr, int start)
        {
            fixed (char* lpfullstr = expr.FullString)
            {
                char* lpseg = lpfullstr + expr.Offset;
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
            int originalPos = IntPosition;
            try {
                if (EndOfStream) {
                    b = default(bool);
                    return false;
                }
                int currPos = IntPosition;
                if (Next(bool.TrueString)) {
                    b = true;
                    return true;
                }
                IntPosition = currPos; // reset the the pos when method was called
                if (Next(bool.FalseString)) {
                    b = false;
                    return true;
                }
                b = default(bool);
                IntPosition = originalPos;
                return false;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        public override bool NextBinaryOp(ObjectContext context, out BinaryOperator foundOp)
        {
            int originalPos = IntPosition;
            try {
                if (EndOfStream || !MatchOperator(InternalBuffer, IntPosition, context, out foundOp)) {
                    foundOp = default(BinaryOperator);
                    IntPosition = originalPos;
                    return false;
                }
                IntPosition += foundOp.OperatorString.Length;
                return true;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        public override bool NextUnaryOp(ObjectContext context, object last, out UnaryOperator foundOp)
        {
            int originalPos = IntPosition;
            try {
                if (EndOfStream || (last != null && !(last is BinaryOperator))) {
                    foundOp = default(UnaryOperator);
                    return false;
                }
                if (!MatchOperator(InternalBuffer, IntPosition, context, out foundOp)) {
                    IntPosition = originalPos;
                    return false;
                }
                IntPosition += foundOp.OperatorString.Length;
                return true;
            }
            catch {
                IntPosition = originalPos;
                throw;
            }
        }

        private static bool MatchOperator<T>(StringSegment expr, int index, ObjectContext context, out T foundOp) where T : IOperator
        {
            string foundStr = null;
            foundOp = default(T);
            foreach (var op in context.GetAllOperators<T>()) {
                string opStr = op.OperatorString;
                if (expr.StartsWith(opStr, index, ignoreCase: true)) {
                    foundOp = op;
                    foundStr = opStr;
                }
            }
            return foundStr != null;
        }
    }
}
