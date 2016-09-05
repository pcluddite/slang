// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Runtime;
using Tbasic.Types;

namespace Tbasic.Parsing
{
	internal partial class DefaultScanner
	{
        public bool NextBinaryOp(ObjectContext context, out BinaryOperator foundOp)
        {
            foundOp = default(BinaryOperator);
            			string token = BuffNextWord();
            if (string.IsNullOrEmpty(token))
                return false;

            if (MatchBinaryOperator(token, context, out foundOp)) {
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

            if (MatchUnaryOperator(token, context, out foundOp)) {
                AdvanceScanner(foundOp.OperatorString.Length);
                return true;
            }

			return false;
        }

        private static bool MatchBinaryOperator(string token, ObjectContext context, out BinaryOperator foundOp)
        {
            string foundStr = null;
            foundOp = default(BinaryOperator);
            foreach (var op in context.GetAllBinaryOperators()) {
                string opStr = op.OperatorString;
                if (token.StartsWith(opStr, StringComparison.CurrentCultureIgnoreCase)) {
                    foundOp = op;
                    foundStr = opStr;
                }
            }
            return foundStr != null;
        }

        private static bool MatchUnaryOperator(string token, ObjectContext context, out UnaryOperator foundOp)
        {
            string foundStr = null;
            foundOp = default(UnaryOperator);
            foreach (var op in context.GetAllUnaryOperators()) {
                string opStr = op.OperatorString;
                if (token.StartsWith(opStr, StringComparison.CurrentCultureIgnoreCase)) {
                    foundOp = op;
                    foundStr = opStr;
                }
            }
            return foundStr != null;
        }
	}
}

