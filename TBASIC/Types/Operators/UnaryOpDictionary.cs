// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Globalization;
using Tbasic.Runtime;
using Tbasic.Errors;

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
            operators.Add("NEW", new UnaryOperator("NEW", New, evaluate: false));
            operators.Add("+", new UnaryOperator("+", Plus));
            operators.Add("-", new UnaryOperator("-", Minus));
            operators.Add("NOT ", new UnaryOperator("NOT ", Not));
            operators.Add("~", new UnaryOperator("~", BitNot));
        }

        private static object New(TBasic runtime, object value)
        {
            Function eval = value as Function;
            if (eval == null) {
                throw ThrowHelper.InvalidTypeInExpression(value?.GetType().Name, "function");
            }
            string name = eval.Expression.ToString();
            TClass prototype;
            if (!runtime.Context.TryGetType(name, out prototype))
                throw new UndefinedObjectException($"The class {name} is undefined");
            StackData stackdat = new StackData(runtime, eval.Parameters.TB_ToStrings());
            stackdat.Name = eval.Expression.ToString();
            stackdat.EvaluateAll();
            return prototype.GetInstance(stackdat);
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
