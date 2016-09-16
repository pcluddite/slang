// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Text;
using Tbasic.Errors;
using Tbasic.Types;
using System.Diagnostics.Contracts;

namespace Tbasic.Runtime
{
    internal class VariableEvaluator : IExpressionEvaluator
    {
        #region Properties

        [ContractPublicPropertyName(nameof(Name))]
        private string _name = null;

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
        
        TbasicType IRuntimeObject.TypeCode
        {
            get {
                return TbasicType.Evaluator;
            }
        }

        object IRuntimeObject.Value
        {
            get {
                return this;
            }
        }

        #endregion

        internal VariableEvaluator(string name, IList<IEnumerable<char>> indices, TRuntime runtime)
        {
            if (runtime == null)
                throw new ArgumentNullException(nameof(runtime));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            Contract.EndContractBlock();

            Runtime = runtime;
            _name = name;
            Indices = indices;
        }

        public override string ToString()
        {
            return _name.ToString();
        }

        public Variable Evaluate()
        {
            if (Indices == null) {
                return CurrentContext.GetVariable(_name);
            }
            else { 
                return CurrentContext.GetArrayAt(_name, EvaluateIndices());
            }
        }

        IRuntimeObject IExpressionEvaluator.Evaluate()
        {
            return Evaluate();
        }

        public int[] EvaluateIndices()
        {
            Contract.Ensures(Contract.Result<int[]>() != null);

            ExpressionEvaluator eval = new ExpressionEvaluator(Runtime);
            int[] indices = new int[Indices.Count];
            for (int index = 0; index < indices.Length; ++index) {
                IRuntimeObject o = eval.Evaluate(Indices[index]);
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