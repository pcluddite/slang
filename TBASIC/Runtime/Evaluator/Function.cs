// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Types;

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
        private IList<StringSegment> _params;

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
        
        public Function(StringSegment expr, TBasic runtime, StringSegment name, IList<StringSegment> parameters)
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
        
        private object ExecuteFunction(StringSegment _name, IList<StringSegment> l_params)
        {
            string name = _name.Trim().ToString();
            CallData func;
            if (CurrentContext.TryGetFunction(name, out func)) {
                StackData stackdat = new StackData(Runtime, l_params.TB_ToStrings());
                stackdat.Name = name; // if this isn't before evaluation, EvaluateAll() won't eval properly 8/30/16
                if (func.Evaluate) {
                    stackdat.EvaluateAll();
                }
                stackdat.Data = func.Function(stackdat);
                CurrentContext.SetReturns(stackdat);
                return stackdat.Data;
            }
            else {
                throw ThrowHelper.UndefinedFunction(name);
            }
        }

        #endregion
    }
}
