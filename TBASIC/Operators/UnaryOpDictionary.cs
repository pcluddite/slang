/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Tbasic.Operators
{
    internal partial class UnaryOpDictionary
    {
        private Dictionary<string, UnaryOperator> unaryOps = new Dictionary<string, UnaryOperator>(22 /* magic number of standard operators */, StringComparer.OrdinalIgnoreCase);

        public void LoadStandardOperators()
        {
            unaryOps.Add("+", new UnaryOperator("+", Plus));
            unaryOps.Add("-", new UnaryOperator("-", Minus));
            unaryOps.Add("NOT ", new UnaryOperator("NOT ", Not));
            unaryOps.Add("~", new UnaryOperator("~", BitNot));
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
