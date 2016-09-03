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
using System.Linq;

namespace Tbasic.Runtime
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
        
        public ObjectContext CurrentContext { get { return Runtime.Context; } }

        public TBasic Runtime { get; set; }

        #endregion

        #region Construction
        
        public Function(TBasic runtime, string name, IList<IEnumerable<char>> parameters)
        {
            Runtime = runtime;
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
