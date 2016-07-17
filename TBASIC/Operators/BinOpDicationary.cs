/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
using System;
using System.Globalization;
using Tbasic.Runtime;

namespace Tbasic.Operators
{
    internal class BinOpDictionary : OperatorDictionary<BinaryOperator>
    {
        public override void LoadStandardOperators()
        {
            operators.Add("*",   new BinaryOperator("*",   0, Multiply));
            operators.Add("/",   new BinaryOperator("/",   0, Divide));
            operators.Add("MOD", new BinaryOperator("MOD", 0, Modulo));
            operators.Add("+",   new BinaryOperator("+",   1, Add));
            operators.Add("-",   new BinaryOperator("-",   1, Subtract));
            operators.Add(">>",  new BinaryOperator(">>",  2, ShiftRight));
            operators.Add("<<",  new BinaryOperator("<<",  2, ShiftLeft));
            operators.Add("<",   new BinaryOperator("<",   3, LessThan));
            operators.Add("=<",  new BinaryOperator("=<",  3, LessThanOrEqual));
            operators.Add("<=",  new BinaryOperator("<=",  3, LessThanOrEqual));
            operators.Add(">",   new BinaryOperator(">",   3, GreaterThan));
            operators.Add("=>",  new BinaryOperator("=>",  3, GreaterThanOrEqual));
            operators.Add(">=",  new BinaryOperator(">=",  3, GreaterThanOrEqual));
            operators.Add("==",  new BinaryOperator("==",  4, EqualTo));
            operators.Add("=",   new BinaryOperator("=",   4, EqualTo));
            operators.Add("<>",  new BinaryOperator("<>",  4, NotEqualTo));
            operators.Add("!=",  new BinaryOperator("!=",  4, NotEqualTo));
            operators.Add("&",   new BinaryOperator("&",   5, BitAnd));
            operators.Add("^",   new BinaryOperator("^",   6, BitXor));
            operators.Add("|",   new BinaryOperator("|",   7, BitOr));
            operators.Add("AND", new BinaryOperator("AND", 8, NotImplemented)); // These are special cases that are evaluated with short circuit evalutaion 6/20/16
            operators.Add("OR",  new BinaryOperator("OR",  9, NotImplemented));
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

        private static object Multiply(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) *
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static object Divide(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) /
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static object Modulo(object left, object right)
        {
            return Convert.ToInt64(left, CultureInfo.CurrentCulture) %
                   Convert.ToInt64(right, CultureInfo.CurrentCulture);
        }

        private static object Add(object left, object right)
        {
            string str1 = left as string,
                   str2 = right as string;
            if (str1 != null || str2 != null)
                return StringAdd(left, right, str1, str2);
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) +
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static string StringAdd(object left, object right, string str1, string str2)
        {
            InitializeStrings(left, right, ref str1, ref str2);
            return str1 + str2;
        }

        private static object Subtract(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) -
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static object LessThan(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) <
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static object LessThanOrEqual(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) <=
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static object GreaterThan(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) >
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static object GreaterThanOrEqual(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) >=
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static object EqualTo(object left, object right)
        {
            string str1 = left as string,
                   str2 = right as string;
            if (str1 != null || str2 != null)
                return StrEquals(left, right, str1, str2);
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) ==
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static bool StrEquals(object left, object right, string str1, string str2)
        {
            InitializeStrings(left, right, ref str1, ref str2);
            return str1 == str2;
        }

        private static void InitializeStrings(object left, object right, ref string str1, ref string str2)
        {
            if (str1 == null)
                str1 = Evaluator.ConvertToString(left);
            if (str2 == null)
                str2 = Evaluator.ConvertToString(right);
        }

        private static object NotEqualTo(object left, object right)
        {
            return Convert.ToDouble(left, CultureInfo.CurrentCulture) !=
                   Convert.ToDouble(right, CultureInfo.CurrentCulture);
        }

        private static bool StrNotEqualTo(object left, object right, string str1, string str2)
        {
            InitializeStrings(left, right, ref str1, ref str2);
            return str1 != str2;
        }

        private static object ShiftLeft(object left, object right)
        {
            return Convert.ToInt64(left, CultureInfo.CurrentCulture) <<
                   Convert.ToInt32(right, CultureInfo.CurrentCulture);
        }

        private static object ShiftRight(object left, object right)
        {
            return Convert.ToInt64(left, CultureInfo.CurrentCulture) >>
                   Convert.ToInt32(right, CultureInfo.CurrentCulture);
        }

        private static object BitAnd(object left, object right)
        {
            return Convert.ToUInt64(left, CultureInfo.CurrentCulture) &
                   Convert.ToUInt64(right, CultureInfo.CurrentCulture);
        }

        private static object BitXor(object left, object right)
        {
            return Convert.ToUInt64(left, CultureInfo.CurrentCulture) ^
                   Convert.ToUInt64(right, CultureInfo.CurrentCulture);
        }

        private static object BitOr(object left, object right)
        {
            return Convert.ToUInt64(left, CultureInfo.CurrentCulture) |
                   Convert.ToUInt64(right, CultureInfo.CurrentCulture);
        }

        private static object NotImplemented(object left, object right)
        {
            throw new NotImplementedException();
        }
    }
}
