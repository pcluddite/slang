﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Runtime;

namespace Tbasic.Types
{
    internal class WhileBlock : CodeBlock
    {
        public WhileBlock(int index, LineCollection code)
        {
            LoadFromCollection(
                code.ParseBlock(
                    index,
                    c => c.Name.EqualsIgnoreCase("WHILE"),
                    c => c.Text.EqualsIgnoreCase("WEND")
                ));
        }

        public override void Execute(TBasic runtime)
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
                eval.Evaluated = true;
            }
        }
    }
}