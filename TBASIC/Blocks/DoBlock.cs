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
    internal class DoBlock : CodeBlock
    {
        public DoBlock(int index, LineCollection code)
        {
            LoadFromCollection(
                code.ParseBlock(
                    index,
                    c => c.Name.EqualsIgnoreCase("DO"),
                    c => c.Text.EqualsIgnoreCase("LOOP")
                ));
        }

        public override void Execute(Executer exec)
        {
            TFunctionData parameters = new TFunctionData(exec, Header.Text);

            if (parameters.ParameterCount < 3) {
                throw ThrowHelper.NoCondition();
            }

            StringSegment condition = new StringSegment(Header.Text, Header.Text.IndexOf(' ', 3));

            if (parameters.GetParameter<string>(1).EqualsIgnoreCase("UNTIL")) {
                condition = new StringSegment(string.Format("NOT ({0})", condition)); // Until means inverted
            }
            else if (parameters.GetParameter<string>(1).EqualsIgnoreCase("WHILE")) {
                // don't do anything, you're golden
            }
            else {
                throw ThrowHelper.ExpectedToken("UNTIL' or 'WHILE");
            }

            Evaluator eval = new Evaluator(condition, exec);

            do {
                exec.Execute(Body);
                if (exec.BreakRequest) {
                    exec.HonorBreak();
                    break;
                }
                eval.ShouldParse = true;
            }
            while (eval.EvaluateBool());
        }
    }
}
