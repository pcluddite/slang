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
        
        public override bool NextPositiveInt(out int integer)
        {
            integer = 0;
            SkipWhiteSpace();
            if (EndOfStream)
                return false;
            int newPos = FindConsecutiveDigits(InternalBuffer, IntPosition);
            if (newPos > IntPosition) {
                integer = int.Parse(InternalBuffer.Substring(IntPosition, newPos - IntPosition));
                IntPosition = newPos;
                return true;
            }
            return false;
        }

        public override bool NextPositiveNumber(out Number num)
        {
            num = default(Number);
            SkipWhiteSpace();
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

        public override bool NextHexadecimal(out int number)
        {
            number = 0;
            SkipWhiteSpace();
            if (EndOfStream)
                return false;

            int endPos = IntPosition;
            if (InternalBuffer[endPos++] != '0')
                return false;
            if (endPos >= InternalBuffer.Length || InternalBuffer[endPos++] != 'x')
                return false;
            endPos = FindConsecutiveHex(InternalBuffer, endPos);
            number = Convert.ToInt32(InternalBuffer.Substring(IntPosition, endPos - IntPosition));
            IntPosition = endPos;
            return true;
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
            SkipWhiteSpace();
            if (EndOfStream)
                return false;
            if (!InternalBuffer.Subsegment(IntPosition).StartsWith(pattern, ignoreCase))
                return false;
            IntPosition += pattern.Length;
            return true;
        }

        public override bool NextString(out string parsed)
        {
            SkipWhiteSpace();
            if (EndOfStream || (InternalBuffer[IntPosition] != '\"' && InternalBuffer[IntPosition] != '\'')) {
                parsed = null;
                return false;
            }
            int endPos = GroupParser.ReadString(InternalBuffer, IntPosition, out parsed) + 1;
            IntPosition = endPos;
            return true;
        }

        public override bool NextFunction(Executer exec, out Function func)
        {
            SkipWhiteSpace();
            func = null;
            if (EndOfStream)
                return false;
            int originalPos = IntPosition;
            if (char.IsLetter(InternalBuffer[IntPosition]) || InternalBuffer[IntPosition] == '_') {
                IntPosition = FindAcceptableFuncChars(InternalBuffer, ++IntPosition);
                if (IntPosition < InternalBuffer.Length) {
                    StringSegment name = InternalBuffer.Subsegment(originalPos, IntPosition - originalPos);
                    SkipWhiteSpace();
                    if (Next("(")) {
                        IList<object> args;
                        IntPosition = GroupParser.ReadGroup(InternalBuffer, IntPosition - 1, exec, out args) + 1;
                        func = new Function(InternalBuffer.Subsegment(originalPos, IntPosition - originalPos), exec, name, args);
                        return true;
                    }
                }
            }
            IntPosition = originalPos;
            return false;
        }

        public override bool NextVariable(Executer exec, out Variable variable)
        {
            SkipWhiteSpace();
            variable = null;
            if (EndOfStream)
                return false;
            int originalPos = IntPosition;
            if (char.IsLetter(InternalBuffer[IntPosition]) || InternalBuffer[IntPosition] == '_') {
                IntPosition = FindAcceptableFuncChars(InternalBuffer, ++IntPosition);
                if (!EndOfStream && InternalBuffer[IntPosition++] == '$') {
                    StringSegment name = InternalBuffer.Subsegment(originalPos, IntPosition - originalPos);
                    SkipWhiteSpace();
                    int[] indices;
                    if (!NextIndices(exec, out indices))
                        indices = null;
                    variable = new Variable(InternalBuffer, name, indices, exec);
                    return true;
                }
            }
            else if (InternalBuffer[IntPosition] == '@') { // it's a macro
                return NextMacro(exec, out variable);
            }
            IntPosition = originalPos;
            return false;
        }

        /// <summary>
        /// This assumes that the first char has already been checked to be an '@'!
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        private bool NextMacro(Executer exec, out Variable variable)
        {
            SkipWhiteSpace();
            int originalPos = IntPosition;
            if (++IntPosition < InternalBuffer.Length) {
                IntPosition = FindAcceptableFuncChars(InternalBuffer, IntPosition);
                StringSegment name = InternalBuffer.Subsegment(originalPos, IntPosition - originalPos);
                SkipWhiteSpace();
                int[] indices;
                if (!NextIndices(exec, out indices))
                    indices = null;
                variable = new Variable(InternalBuffer, name, indices, exec);
                return true;
            }
            variable = null;
            IntPosition = originalPos;
            return false;
        }

        public override bool NextIndices(Executer exec, out int[] indices)
        {
            SkipWhiteSpace();
            indices = null;
            int originalPos = IntPosition;
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
            SkipWhiteSpace();
            if (EndOfStream) {
                b = false;
                return false;
            }
            int currPos = IntPosition;
            if (Next(bool.TrueString)) {
                b = true;
                IntPosition += bool.TrueString.Length;
                return true;
            }
            IntPosition = currPos; // reset the the pos when method was called
            b = !Next(bool.FalseString);
            if (!b) {
                IntPosition += bool.FalseString.Length;
                return true;
            }
            IntPosition = currPos;
            return false;
        }

        public override bool NextBinaryOp(BinOpDictionary _binOps, out BinaryOperator foundOp)
        {
            SkipWhiteSpace();
            if (EndOfStream || !MatchOperator(InternalBuffer, IntPosition, _binOps, out foundOp)) {
                foundOp = default(BinaryOperator);
                return false;
            }
            IntPosition += foundOp.OperatorString.Length;
            return true;
        }

        public override bool NextUnaryOp(UnaryOpDictionary _unOps, object last, out UnaryOperator foundOp)
        {
            SkipWhiteSpace();
            if (EndOfStream || (last != null && !(last is BinaryOperator))) {
                foundOp = default(UnaryOperator);
                return false;
            }
            if (!MatchOperator(InternalBuffer, IntPosition, _unOps, out foundOp))
                return false;
            IntPosition += foundOp.OperatorString.Length;
            return true;
        }

        private static bool MatchOperator<T>(StringSegment expr, int index, OperatorDictionary<T> ops, out T foundOp) where T : IOperator
        {
            string foundStr = null;
            foundOp = default(T);
            foreach (var op in ops) {
                string opStr = op.Value.OperatorString;
                if (expr.StartsWith(opStr, index, ignoreCase: true)) {
                    foundOp = op.Value;
                    foundStr = opStr;
                }
            }
            return foundStr != null;
        }
    }
}
