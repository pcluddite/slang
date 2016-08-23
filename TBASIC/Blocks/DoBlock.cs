// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
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
                    c => c.Name.EqualsIgnoreCase("LOOP")
                ));
        }

        public override void Execute(Executer exec)
        {
            Scanner scanner;
            bool doLoop;
            RuntimeData parameters = new RuntimeData(exec, Footer.Text);

            if (parameters.ParameterCount < 3) {
                parameters = new RuntimeData(exec, Header.Text);
                if (parameters.ParameterCount < 3) { // still less than three? there's no condition
                    throw ThrowHelper.NoCondition();
                }
                if (Footer.Name != Footer.Text) {
                    throw new UnexpectedTokenExceptiopn("Unexpected arguments in loop footer", prependGeneric: false);
                }
                scanner = exec.ScannerDelegate(new StringSegment(Header.Text, Header.Text.IndexOf(' ', Header.Name.Length)));
                doLoop = false;
            }
            else {
                if (Header.Name != Header.Text) {
                    throw new UnexpectedTokenExceptiopn("Unexpected arguments in loop header", prependGeneric: false);
                }
                scanner = exec.ScannerDelegate(new StringSegment(Footer.Text, Footer.Text.IndexOf(' ', Footer.Name.Length)));
                doLoop = true;
            }

            Predicate<Evaluator> condition;
            string loop = scanner.Next();

            if (string.Equals(loop, "UNTIL", StringComparison.OrdinalIgnoreCase)) {
                condition = (e => !e.EvaluateBool());
            }
            else if (string.Equals(loop, "WHILE", StringComparison.OrdinalIgnoreCase)) {
                condition = (e => e.EvaluateBool());
            }
            else {
                throw ThrowHelper.ExpectedToken("UNTIL' or 'WHILE");
            }

            Evaluator eval = new Evaluator(scanner.Read(scanner.IntPosition, scanner.IntLength - scanner.IntPosition), exec);

            if (doLoop) {
                do {
                    exec.Execute(Body);
                    if (exec.BreakRequest) {
                        exec.HonorBreak();
                        break;
                    }
                    eval.ShouldParse = true;
                }
                while (condition(eval));
            }
            else {
                while (condition(eval)) {
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
}
