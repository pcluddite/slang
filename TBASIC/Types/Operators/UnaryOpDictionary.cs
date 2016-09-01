// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Globalization;
using Tbasic.Runtime;
using Tbasic.Components;

namespace Tbasic.Types
{
    internal class UnaryOpDictionary : OperatorDictionary<UnaryOperator>
    {
        public UnaryOpDictionary()
        {
        }

        public UnaryOpDictionary(UnaryOpDictionary other)
            : base(other)
        {
        }

        public override void LoadStandardOperators()
        {
            operators.Add("NEW", new UnaryOperator("NEW", New));
            operators.Add("+", new UnaryOperator("+", Plus));
            operators.Add("-", new UnaryOperator("-", Minus));
            operators.Add("NOT ", new UnaryOperator("NOT ", Not));
            operators.Add("~", new UnaryOperator("~", BitNot));
        }

        private static object New(TBasic runtime, object value)
        {
            /* runtime.ScannerDelegate(new StringSegment(value.ToString()));
            TClass prototype;
            if (!runtime.Context.TryGetType(name, out prototype))
                throw new UndefinedObjectException($"The class {name} is undefined");
            return prototype.GetInstance(new StackData(stackdat.Runtime, stackdat.Parameters.Skip(1))); */
            throw new NotImplementedException();
        }

        private static object Plus(TBasic runtime, object value)
        {
            return +Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }

        private static object Minus(TBasic runtime, object value)
        {
            return -Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }

        private static object Not(TBasic runtime, object value)
        {
            return !Convert.ToBoolean(value, CultureInfo.CurrentCulture);
        }

        private static object BitNot(TBasic runtime, object value)
        {
            return ~Convert.ToUInt64(value, CultureInfo.CurrentCulture);
        }
    }
}
