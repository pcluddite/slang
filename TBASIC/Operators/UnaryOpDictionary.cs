﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Globalization;

namespace Tbasic.Operators
{
    internal class UnaryOpDictionary : OperatorDictionary<UnaryOperator>
    {
        public override void LoadStandardOperators()
        {
            operators.Add("+", new UnaryOperator("+", Plus));
            operators.Add("-", new UnaryOperator("-", Minus));
            operators.Add("NOT ", new UnaryOperator("NOT ", Not));
            operators.Add("~", new UnaryOperator("~", BitNot));
        }
        
        private static object Plus(object value)
        {
            return +Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }

        private static object Minus(object value)
        {
            return -Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }

        private static object Not(object value)
        {
            return !Convert.ToBoolean(value, CultureInfo.CurrentCulture);
        }

        private static object BitNot(object value)
        {
            return ~Convert.ToUInt64(value, CultureInfo.CurrentCulture);
        }
    }
}
