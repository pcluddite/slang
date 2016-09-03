// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Linq;
using Tbasic.Parsing;
using Tbasic.Runtime;

namespace Tbasic.Types
{
    internal class FuncBlock : CodeBlock
    {
        public StackData Prototype { get; private set; }
        public static readonly Predicate<Line> CheckBegin = (line => line.Name.EqualsIgnoreCase("FUNCTION"));
        public static readonly Predicate<Line> CheckEnd = (line => line.Text.EqualsIgnoreCase("END FUNCTION"));
        
        internal FuncBlock(StackData prototype, Line header, LineCollection body, Line footer)
        {
            Header = header;
            Footer = footer;
            Body = body;
            Prototype = prototype;
        }

        public TBasicFunction CreateDelegate()
        {
            return new TBasicFunction(Execute);
        }

        public object Execute(StackData stackdat)
        {
            stackdat.AssertCount(Prototype.ParameterCount);

            TBasic runtime = stackdat.Runtime;
            runtime.Context = runtime.Context.CreateSubContext();

            for (int index = 1; index < Prototype.ParameterCount; index++) {
                runtime.Context.SetVariable((string)Prototype.GetAt(index), stackdat.GetAt(index));
            }
            runtime.Context.AddCommand("return", Return);
            runtime.Context.AddCommand("SetStatus", SetStatus);

            stackdat.CopyFrom(runtime.Execute(Body));
            runtime.HonorBreak();
            runtime.Context = runtime.Context.Collect();
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

        public override void Execute(TBasic exec)
        {
            throw new NotImplementedException();
        }
    }
}
