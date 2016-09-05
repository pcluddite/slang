// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Linq;
using Tbasic.Parsing;
using Tbasic.Runtime;

namespace Tbasic.Types
{
    /// <summary>
    /// A code block that sores information on a function
    /// </summary>
    public class FunctionBlock : CodeBlock
    {
        /// <summary>
        /// Gets or sets this function's prototype (the name followed by a collection of expected arguments)
        /// </summary>
        public virtual IList<string> Prototype { get; set; }

        internal static readonly Predicate<Line> CheckBegin = (line => line.Name.EqualsIgnoreCase("FUNCTION"));
        internal static readonly Predicate<Line> CheckEnd = (line => line.Text.EqualsIgnoreCase("END FUNCTION"));

        /// <summary>
        /// Constructs a function block without initializing any members
        /// </summary>
        protected FunctionBlock()
        {
        }

        /// <summary>
        /// Constructs a function block
        /// </summary>
        /// <param name="prototype">the name and list of arguments for the function</param>
        /// <param name="body">the lines for the body of the function</param>
        /// <param name="header">the function header</param>
        /// <param name="footer">the function footer</param>
        public FunctionBlock(IList<string> prototype, Line header, Line footer, IList<Line> body)
        {
            Header = header;
            Footer = footer;
            Body = body;
            Prototype = prototype;
        }

        /// <summary>
        /// Executes the body of the function
        /// </summary>
        public virtual object Execute(StackData stackdat)
        {
            stackdat.AssertCount(Prototype.Count);

            ObjectContext context = stackdat.Context;
            TBasic runtime = stackdat.Runtime;

            int index = 0;
            foreach(string param in Prototype.Skip(1)) { // skip the first item in the collection, which is assumed to be the name
                context.SetVariable(param, stackdat.GetAt(++index));
            }
            
            context.AddCommand("return", Return);
            context.AddCommand("SetStatus", SetStatus);

            StackData ret = runtime.Execute(Body);
            stackdat.CopyFrom(ret);
            runtime.HonorBreak();
            return stackdat.Data;
        }

        private object Return(StackData stackFrame)
        {
            if (stackFrame.ParameterCount < 2) {
                stackFrame.AssertCount(2);
            }
            ExpressionEvaluator e = new ExpressionEvaluator(
                stackFrame.Text.Substring(stackFrame.Name.Length),
                stackFrame.Runtime);
            stackFrame.Runtime.RequestBreak();
            return stackFrame.Data = e.Evaluate();
        }

        private object SetStatus(StackData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.Status = stackFrame.GetAt<int>(1);
        }

        /// <summary>
        /// Do not call this method. This is not implemented by default.
        /// </summary>
        public override void Execute(TBasic exec)
        {
            throw new NotImplementedException();
        }
    }
}
