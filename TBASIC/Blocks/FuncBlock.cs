﻿/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Libraries;
using Tbasic.Parsing;
using Tbasic.Runtime;

namespace Tbasic
{
    internal class FuncBlock : CodeBlock
    {
        public TFunctionData Template { get; private set; }

        public FuncBlock(int index, LineCollection code)
        {
            LoadFromCollection(
                code.ParseBlock(
                    index,
                    c => c.Name.EqualsIgnoreCase("FUNCTION"),
                    c => c.Text.EqualsIgnoreCase("END FUNCTION")
                ));
            Template = new TFunctionData(null);
            Template.SetAll(ParseFunction(Header.Text.Substring(Header.Name.Length)));
        }

        public TBasicFunction CreateDelegate()
        {
            return new TBasicFunction(Execute);
        }

        public void Execute(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(Template.ParameterCount);

            Executer exec = stackFrame.StackExecuter;
            exec.Context = exec.Context.CreateSubContext();

            for (int index = 1; index < Template.ParameterCount; index++) {
                exec.Context.SetVariable(Template.GetParameter<string>(index), stackFrame.GetParameter(index));
            }
            exec.Context.SetCommand("return", Return);
            exec.Context.SetFunction("SetStatus", SetStatus);

            stackFrame.CopyFrom(exec.Execute(Body));
            exec.HonorBreak();
            exec.Context = exec.Context.Collect();
        }

        private void Return(TFunctionData stackFrame)
        {
            if (stackFrame.ParameterCount < 2) {
                stackFrame.AssertParamCount(2);
            }
            Evaluator e = new Evaluator(
                new StringSegment(stackFrame.Text, stackFrame.Name.Length),
                stackFrame.StackExecuter);
            stackFrame.Data = e.Evaluate();
            stackFrame.StackExecuter.RequestBreak();
        }

        private void SetStatus(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Status = stackFrame.GetParameter<int>(1);
        }

        public override void Execute(Executer exec)
        {
            throw new NotImplementedException();
        }

        private object[] ParseFunction(string text)
        {
            Line codeLine = new Line(0, text);
            text = codeLine.Text; // it's trimmed
            List<object> result = new List<object>();
            result.Add(codeLine.Name);

            int c_index = codeLine.Name.Length; // Start at the end of the name
            int expected = 0;
            int last = c_index;

            for (; c_index < text.Length; c_index++) {
                char cur = text[c_index];
                switch (cur) {
                    case ' ': // ignore spaces
                        continue;
                    case '\'':
                    case '\"': {
                            c_index = GroupParser.IndexString(text, c_index);
                        }
                        break;
                    case '(':
                        expected++;
                        break;
                    case ')':
                        expected--;
                        break;
                }

                if ((expected == 1 && cur == ',') ||
                     expected == 0) { // The commas in between other parentheses are not ours.
                    string param = text.Substring(last + 1, c_index - last - 1).Trim();
                    if (!param.Equals("")) {
                        result.Add(param); // From the last comma to this one. That's a parameter.
                    }
                    last = c_index;
                    if (expected == 0) { // fin
                        return result.ToArray();
                    }
                }
            }
            throw ThrowHelper.UnterminatedBlock("FUNC");
        }
    }
}
