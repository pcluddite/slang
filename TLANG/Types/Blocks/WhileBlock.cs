﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;
using Tint.Errors;
using Tint.Parsing;
using Tint.Runtime;

namespace Tint.Types
{
    internal class WhileBlock : CodeBlock
    {
        public WhileBlock(int index, IList<Line> code)
        {
            LoadFromCollection(
                LineCollection.ParseBlock(index, code,
                    c => c.Name.EqualsIgnoreCase("WHILE"),
                    c => c.Text.EqualsIgnoreCase("WEND")
                ));
        }

        public override void Execute(TRuntime runtime)
        {
            Statement line = new Statement(runtime.Scanner.Scan(Header.Text));

            if (line.Count < 2) {
                throw ThrowHelper.NoCondition();
            }

            string condition = Header.Text.Substring(Header.Text.IndexOf(' '));

            ExpressionEvaluator eval = new ExpressionEvaluator(condition, runtime);

            while (eval.EvaluateBool()) {
                runtime.Execute(Body);
                if (runtime.BreakRequest) {
                    runtime.HonorBreak();
                    break;
                }
                eval.Parsed = false;
            }
        }
    }
}