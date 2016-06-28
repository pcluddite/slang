/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License for
 * non-commercial use.
 *
 **/

using Tbasic.Runtime;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Components;

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
            TFunctionData parameters = new TFunctionData(exec, Header.Text);

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