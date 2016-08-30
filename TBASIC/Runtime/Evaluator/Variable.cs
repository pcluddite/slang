// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Text;
using Tbasic.Components;
using Tbasic.Errors;

namespace Tbasic.Runtime
{
    internal class Variable : IExpressionEvaluator
    {
        private StringSegment _expression = null;

        #region Properties

        public StringSegment[] Indices { get; private set; }

        public bool IsMacro
        {
            get {
                return Name[0] == '@';
            }
        }

        public bool IsValid
        {
            get {
                return Name[Name.Length - 1] == '$';
            }
        }

        public ObjectContext CurrentContext
        {
            get {
                return Runtime.Context;
            }
        }

        public TBasic Runtime { get; set; }
        public StringSegment Name { get; private set; }

        public StringSegment Expression
        {
            get {
                return _expression;
            }
            set {
                _expression = value.Trim();
            }
        }

        #endregion

        public Variable(StringSegment full, StringSegment name, StringSegment[] indices, TBasic exec)
        {
            Runtime = exec;
            _expression = full;
            Name = name;
            Indices = indices;
        }

        public override string ToString()
        {
            return _expression.ToString();
        }

        public object Evaluate()
        {
            object obj = CurrentContext.GetVariable(Name.ToString());
            if (Indices != null) {
                obj = CurrentContext.GetArrayAt(Name.ToString(), EvaluateIndices());
            }
            return obj;
        }

        public int[] EvaluateIndices()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator(Runtime);
            int[] indices = new int[Indices.Length];
            for (int index = 0; index < indices.Length; ++index) {
                object o = eval.Evaluate(Indices[index]);
                Number? num = Number.AsNumber(o, Runtime.Options);
                if (num == null) {
                    throw ThrowHelper.InvalidTypeInExpression(o.GetType().Name, typeof(Number).Name);
                }
                else {
                    indices[index] = (int)num; // this will fail if there's a fractional part
                }
            }
            return indices;
        }

        public static string GetFullName(string name, int[] indices)
        {
            StringBuilder sb = new StringBuilder(name);
            if (indices != null && indices.Length > 0) {
                sb.Append("[");
                for (int i = 0; i < indices.Length; ++i) {
                    sb.AppendFormat("{0},", indices[i]);
                }
                sb.AppendFormat("{0}]", indices[0]);
            }
            return sb.ToString();
        }
    }
}