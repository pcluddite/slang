// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Text;
using Tint.Errors;
using Tint.Types;

namespace Tint.Runtime
{
    internal class Variable : IExpressionEvaluator
    {
        private string _name = null;

        #region Properties

        public IList<IEnumerable<char>> Indices { get; private set; }
        
        public ObjectContext CurrentContext
        {
            get {
                return Runtime.Context;
            }
            set {
                throw new NotImplementedException();
            }
        }

        public TRuntime Runtime { get; set; }

        public IEnumerable<char> Expression
        {
            get {
                return _name;
            }
            set {
                _name = value?.ToString();
            }
        }

        public string Name
        {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        #endregion

        public Variable(string name, IList<IEnumerable<char>> indices, TRuntime exec)
        {
            Runtime = exec;
            _name = name;
            Indices = indices;
        }

        public override string ToString()
        {
            return _name.ToString();
        }

        public object Evaluate()
        {
            object obj = CurrentContext.GetVariable(_name);
            if (Indices != null) {
                obj = CurrentContext.GetArrayAt(_name, EvaluateIndices());
            }
            return obj;
        }

        public int[] EvaluateIndices()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator(Runtime);
            int[] indices = new int[Indices.Count];
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