// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;

namespace Tbasic.Runtime
{
    /// <summary>
    /// Class for evaluating a function
    /// </summary>
    internal class Function : IExpressionEvaluator
    {
        #region Private Members

        private StringSegment _expression;
        private StringSegment _function;
        private IList<object> _params;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the expression to be evaluated
        /// </summary>
        /// <value></value>
        public StringSegment Expression
        {
            get {
                return _expression;
            }
            set {
                _expression = value.Trim();
            }
        }

        public int LastIndex { get; private set; }

        public StringSegment Name
        {
            get {
                if (StringSegment.IsNullOrEmpty(_function)) {
                    int index = _expression.IndexOf('(');
                    if (index < 1) {
                        throw new FormatException("string is not a function");
                    }
                    _function = _expression.Remove(index);
                }
                return _function;
            }
        }

        public ObjectContext CurrentContext { get { return Runtime.Context; } }

        public TBasic Runtime { get; set; }

        #endregion

        #region Construction
        
        public Function(StringSegment expr, TBasic runtime, StringSegment name, IList<object> parameters)
        {
            Runtime = runtime;
            _expression = expr;
            _params = parameters;
            _function = name;
        }

        #endregion

        #region Methods
        
        
        public object Evaluate()
        {
            return ExecuteFunction(_function, _params);
        }
        
        public override string ToString()
        {
            return Expression.ToString();
        }
        
        private object ExecuteFunction(StringSegment _name, IList<object> l_params)
        {
            string name = _name.Trim().ToString();
            object[] a_evaluated = null;
            if (l_params != null) {
                a_evaluated = new object[l_params.Count];
                l_params.CopyTo(a_evaluated, 0);
                for (int i = 0; i < a_evaluated.Length; ++i) {
                    IExpressionEvaluator expr = a_evaluated[i] as IExpressionEvaluator;
                    if (expr != null) {
                        a_evaluated[i] = expr.Evaluate();
                    }
                }
            }
            ObjectContext context = CurrentContext.FindFunctionContext(name);
            if (context == null) {
                throw ThrowHelper.UndefinedFunction(name);
            }
            else {
                StackData _sframe = new StackData(Runtime, a_evaluated);
                _sframe.Name = name;
                _sframe.Data = context.GetFunction(name).Invoke(_sframe);
                CurrentContext.SetReturns(_sframe);
                return _sframe.Data;
            }
        }

        #endregion
    }
}
