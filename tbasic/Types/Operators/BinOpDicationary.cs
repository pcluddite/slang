// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Globalization;
using Tbasic.Runtime;
using Tbasic.Errors;
using System.Linq;

namespace Tbasic.Types
{
    internal class BinOpDictionary : OperatorDictionary<BinaryOperator>
    {
        public BinOpDictionary()
        {
        }

        public BinOpDictionary(BinOpDictionary other)
            : base(other)
        {
        }

        public override void LoadStandardOperators()
        {
            operators.Add(".", new BinaryOperator(".", -1, Dot, BinaryOperator.OperandPosition.Left)); // only evaluate the left operand
            operators.Add("*", new BinaryOperator("*", 0, Multiply));
            operators.Add("/", new BinaryOperator("/", 0, Divide));
            operators.Add("%", new BinaryOperator("%", 0, Modulo));
            operators.Add("+", new BinaryOperator("+", 1, Add));
            operators.Add("-", new BinaryOperator("-", 1, Subtract));
            operators.Add(">>", new BinaryOperator(">>", 2, ShiftRight));
            operators.Add("<<", new BinaryOperator("<<", 2, ShiftLeft));
            operators.Add("<", new BinaryOperator("<", 3, LessThan));
            operators.Add("=<", new BinaryOperator("=<", 3, LessThanOrEqual));
            operators.Add("<=", new BinaryOperator("<=", 3, LessThanOrEqual));
            operators.Add(">", new BinaryOperator(">", 3, GreaterThan));
            operators.Add("=>", new BinaryOperator("=>", 3, GreaterThanOrEqual));
            operators.Add(">=", new BinaryOperator(">=", 3, GreaterThanOrEqual));
            operators.Add("==", new BinaryOperator("==", 4, EqualTo));
            operators.Add("=", new BinaryOperator("=", 4, EqualTo));
            operators.Add("~=", new BinaryOperator("~=", 4, SortaEquals));
            operators.Add("<>", new BinaryOperator("<>", 4, NotEqualTo));
            operators.Add("!=", new BinaryOperator("!=", 4, NotEqualTo));
            operators.Add("&", new BinaryOperator("&", 5, BitAnd));
            operators.Add("^", new BinaryOperator("^", 6, BitXor));
            operators.Add("|", new BinaryOperator("|", 7, BitOr));
            operators.Add("&&", new BinaryOperator("&&", 8, NotImplemented)); // These are special cases that are evaluated with short circuit evalutaion 6/20/16
            operators.Add("||", new BinaryOperator("||", 9, NotImplemented));
        }

        /// <summary>
        /// This method gets the precedence of a binary operator
        /// </summary>
        /// <param name="strOp"></param>
        /// <returns></returns>
        public int OperatorPrecedence(string strOp)
        {
            return operators[strOp].Precedence;
        }

        private static object Dot(TRuntime runtime, object left, object right)
        {
            if (left == null || right == null) {
                throw new UndefinedObjectException("The dot operator does not accept null operands");
            }
            TClass n = left as TClass;
            if (n == null)
                throw new TbasicRuntimeException("The dot operator cannot be used on primitive types");
            IExpressionEvaluator e = right as IExpressionEvaluator;
            if (e != null) {
                e.CurrentContext = n; // set the context to the scope of the class
                return e.Evaluate();
            }
            else {
                throw ThrowHelper.InvalidExpression($"{n.Name}.{right}");
            }
        }

        private static object Multiply(TRuntime runtime, object left, object right)
        {
            return Number.Convert(left) *
                   Number.Convert(right);
        }

        private static object Divide(TRuntime runtime, object left, object right)
        {
            return Number.Convert(left) /
                   Number.Convert(right);
        }

        private static object Modulo(TRuntime runtime, object left, object right)
        {
            return Convert.ToInt64(left, CultureInfo.CurrentCulture) %
                   Convert.ToInt64(right, CultureInfo.CurrentCulture);
        }

        private static object Add(TRuntime runtime, object left, object right)
        {
            string str1 = left as string,
                   str2 = right as string;
            if (str1 != null || str2 != null)
                return StringAdd(left, right, str1, str2);
            return Number.Convert(left) +
                   Number.Convert(right);
        }

        private static string StringAdd(object left, object right, string str1, string str2)
        {
            InitializeStrings(left, right, ref str1, ref str2);
            return str1 + str2;
        }

        private static object Subtract(TRuntime runtime, object left, object right)
        {
            return Number.Convert(left) -
                   Number.Convert(right);
        }

        private static object LessThan(TRuntime runtime, object left, object right)
        {
            return Number.Convert(left) <
                   Number.Convert(right);
        }

        private static object LessThanOrEqual(TRuntime runtime, object left, object right)
        {
            return Number.Convert(left) <=
                   Number.Convert(right);
        }

        private static object GreaterThan(TRuntime runtime, object left, object right)
        {
            return Number.Convert(left) >
                   Number.Convert(right);
        }

        private static object GreaterThanOrEqual(TRuntime runtime, object left, object right)
        {
            return Number.Convert(left) >=
                   Number.Convert(right);
        }

        private static object EqualTo(TRuntime runtime, object left, object right)
        {
            return EqualToAsBool(runtime, left, right);
        }

        private static bool EqualToAsBool(TRuntime runtime, object left, object right) // separate method so that it won't be boxed and unboxed unnecessarily 8/8/16
        {
            if (left == right)
                return true;

            if (left == null || right == null)
                return false;

            return left.Equals(right);
        }

        private static void InitializeStrings(object left, object right, ref string str1, ref string str2)
        {
            if (str1 == null)
                str1 = ExpressionEvaluator.GetStringRepresentation(left);
            if (str2 == null)
                str2 = ExpressionEvaluator.GetStringRepresentation(right);
        }

        private static object SortaEquals(TRuntime runtime, object left, object right)
        {
            if (left == null ^ right == null) // exclusive or
                return false;
            if (left.GetType() == right.GetType())
                return EqualTo(runtime, left, right);

            string str_left = left as string;
            if (str_left != null)
                return StrSortaEqualsObj(str_left, right);
            string str_right = right as string;
            if (str_right != null)
                return StrSortaEqualsObj(str_right, left);

            return false;
        }

        private const ExecuterOption SortaEqualsOptions = ExecuterOption.None;

        private static bool StrSortaEqualsObj(string str_left, object right)
        {
            Number? n_right = Number.AsNumber(right, SortaEqualsOptions);
            if (n_right != null) {
                Number n_left;
                if (Number.TryParse(str_left, out n_left)) {
                    return n_left == n_right.Value;
                }
                bool b_left;
                if (bool.TryParse(str_left, out b_left)) {
                    return (n_right != 0) == b_left;
                }
            }
            bool b_right;
            if (bool.TryParse(right.ToString(), out b_right)) {
                bool b_left;
                if (bool.TryParse(str_left, out b_left)) {
                    return b_left == b_right;
                }
                Number n_left;
                if (Number.TryParse(str_left, out n_left)) {
                    return (n_left != 0) == b_right;
                }
            }
            return false;
        }

        private static object NotEqualTo(TRuntime runtime, object left, object right)
        {
            return !EqualToAsBool(runtime, left, right);
        }

        private static bool StrNotEqualTo(object left, object right, string str1, string str2)
        {
            InitializeStrings(left, right, ref str1, ref str2);
            return str1 != str2;
        }

        private static object ShiftLeft(TRuntime runtime, object left, object right)
        {
            return Convert.ToInt64(left, CultureInfo.CurrentCulture) <<
                   Convert.ToInt32(right, CultureInfo.CurrentCulture);
        }

        private static object ShiftRight(TRuntime runtime, object left, object right)
        {
            return Convert.ToInt64(left, CultureInfo.CurrentCulture) >>
                   Convert.ToInt32(right, CultureInfo.CurrentCulture);
        }

        private static object BitAnd(TRuntime runtime, object left, object right)
        {
            return Convert.ToUInt64(left, CultureInfo.CurrentCulture) &
                   Convert.ToUInt64(right, CultureInfo.CurrentCulture);
        }

        private static object BitXor(TRuntime runtime, object left, object right)
        {
            return Convert.ToUInt64(left, CultureInfo.CurrentCulture) ^
                   Convert.ToUInt64(right, CultureInfo.CurrentCulture);
        }

        private static object BitOr(TRuntime runtime, object left, object right)
        {
            return Convert.ToUInt64(left, CultureInfo.CurrentCulture) |
                   Convert.ToUInt64(right, CultureInfo.CurrentCulture);
        }

        private static object NotImplemented(TRuntime runtime, object left, object right)
        {
            throw new NotImplementedException();
        }
    }
}
