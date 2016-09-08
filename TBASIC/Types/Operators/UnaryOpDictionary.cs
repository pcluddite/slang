// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Globalization;
using TLang.Runtime;
using TLang.Errors;

namespace TLang.Types
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
            operators.Add("NOT", new UnaryOperator("NOT", Not));
            operators.Add("~", new UnaryOperator("~", BitNot));
        }

        private static object New(TRuntime runtime, object value)
        {
            Function eval = value as Function;
            if (eval == null) {
                throw ThrowHelper.InvalidTypeInExpression(value?.GetType().Name, "function");
            }
            string name = eval.Expression.ToString();
            TClass prototype;
            if (!runtime.Context.TryGetType(name, out prototype))
                throw new UndefinedObjectException($"The class {name} is undefined");
            StackData stackdat = new StackData(runtime.Options, eval.Parameters.TB_ToStrings());
            stackdat.Name = eval.Expression.ToString();
            stackdat.EvaluateAll(runtime);
            return prototype.GetInstance(runtime, stackdat);
        }

        private static object Plus(TRuntime runtime, object value)
        {
            return +Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }

        private static object Minus(TRuntime runtime, object value)
        {
            return -Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }

        private static object Not(TRuntime runtime, object value)
        {
            return !Convert.ToBoolean(value, CultureInfo.CurrentCulture);
        }

        private static object BitNot(TRuntime runtime, object value)
        {
            return ~Convert.ToUInt64(value, CultureInfo.CurrentCulture);
        }
    }
}
