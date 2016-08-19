// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Runtime;

namespace Tbasic
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

        public override void Execute(Executer exec)
        {
            RuntimeData parameters = new RuntimeData(exec, Header.Text);

            if (parameters.ParameterCount < 2) {
                throw ThrowHelper.NoCondition();
            }

            StringSegment condition = new StringSegment(Header.Text, Header.Text.IndexOf(' '));

            Evaluator eval = new Evaluator(condition, exec);

            while (eval.EvaluateBool()) {
                exec.Execute(Body);
                if (exec.BreakRequest) {
                    exec.HonorBreak();
                    break;
                }
                eval.ShouldParse = true;
            }
        }
    }
}