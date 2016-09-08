// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;
using TLang.Errors;
using TLang.Types;
using System;

namespace TLang.Runtime
{
    /// <summary>
    /// Class for evaluating a function
    /// </summary>
    internal class Function : IExpressionEvaluator
    {
        #region Private Members

        private string _name;
        private IList<IEnumerable<char>> _params;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the expression to be evaluated
        /// </summary>
        /// <value></value>
        public IEnumerable<char> Expression
        {
            get {
                return _name;
            }
            set {
                _name = value?.ToString();
            }
        }
        
        /// <summary>
        /// Gets or sets the context in which this function should be run. This is the global context by default.
        /// </summary>
        public ObjectContext CurrentContext { get; set; }

        public TRuntime Runtime { get; set; }

        public IList<IEnumerable<char>> Parameters
        {
            get {
                return _params;
            }
        }

        #endregion

        #region Construction
        
        public Function(TRuntime runtime, string name, IList<IEnumerable<char>> parameters)
        {
            Runtime = runtime;
            CurrentContext = runtime.Global;
            _name = name;
            _params = parameters;
        }

        #endregion

        #region Methods
        
        public object Evaluate()
        {
            return ExecuteFunction(_name, _params);
        }
        
        public override string ToString()
        {
            return Expression.ToString();
        }
        
        private object ExecuteFunction(string name, IList<IEnumerable<char>> l_params)
        {
            CallData func;
            if (CurrentContext.TryGetFunction(name, out func)) {
                StackData stackdat = new StackData(Runtime.Options, l_params.TB_ToStrings());
                stackdat.Name = name; // if this isn't before evaluation, EvaluateAll() won't eval properly 8/30/16
                if (func.ShouldEvaluate) {
                    stackdat.EvaluateAll(Runtime);
                }
                Runtime.ExecuteInContext(CurrentContext, func.Function, stackdat: stackdat);
                return stackdat.ReturnValue;
            }
            else {
                throw ThrowHelper.UndefinedFunction(name);
            }
        }

        #endregion
    }
}
