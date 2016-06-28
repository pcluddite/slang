/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License for
 * non-commercial use.
 *
 **/

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
    internal class Function : IEvaluator
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

        public ObjectContext CurrentContext { get { return CurrentExecution.Context; } }

        public Executer CurrentExecution { get; set; }

        #endregion

        #region Construction
        
        public Function(StringSegment expr, Executer exec, StringSegment name, IList<object> parameters)
        {
            CurrentExecution = exec;
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
                    IEvaluator expr = a_evaluated[i] as IEvaluator;
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
                TFunctionData _sframe = new TFunctionData(CurrentExecution);
                _sframe.SetAll(a_evaluated);
                _sframe.Name = name;
                context.GetFunction(name).Invoke(_sframe);
                CurrentContext.SetReturns(_sframe);
                return _sframe.Data;
            }
        }

        #endregion
    }
}
