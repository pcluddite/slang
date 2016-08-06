// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Runtime;
using System.IO;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Components;

namespace Tbasic.Libraries
{
    internal class StatementLibrary : Library
    {
        public StatementLibrary()
        {
            Add("#include", Include);
            Add("LET", Let);
            Add("EXIT", Exit);
            Add("BREAK", Break);
            Add("DIM", DIM);
            Add("SLEEP", Sleep);
            Add("ELSE", UhOh);
            Add("END", UhOh);
            Add("WEND", UhOh);
        }

        private void Include(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            string path = Path.GetFullPath(stackFrame.GetParameter<string>(1));
            if (!File.Exists(path)) {
                throw new FileNotFoundException();
            }

            CodeBlock[] funcs;
            LineCollection lines = Executer.ScanLines(File.ReadAllLines(path), out funcs);



            NULL(stackFrame);
        }

        private void Sleep(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            System.Threading.Thread.Sleep(stackFrame.GetParameter<int>(1));
            NULL(stackFrame);
        }

        private void Break(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(1);
            stackFrame.StackExecuter.RequestBreak();
            NULL(stackFrame);
        }

        internal void Exit(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(1);
            stackFrame.StackExecuter.RequestExit();
            NULL(stackFrame);
        }

        internal static void NULL(TFunctionData stackFrame)
        {
            stackFrame.Context.PersistReturns(stackFrame);
        }

        internal void UhOh(TFunctionData stackFrame)
        {
            throw ThrowHelper.NoOpeningStatement(stackFrame.Text);
        }

        internal void DIM(TFunctionData stackFrame)
        {
            if (stackFrame.ParameterCount < 2) {
                stackFrame.AssertParamCount(2);
            }

            StringSegment text = new StringSegment(stackFrame.Text);
            Scanner scanner = new DefaultScanner(text);
            scanner.IntPosition += stackFrame.Name.Length;
            scanner.SkipWhiteSpace();

            Variable v;
            if (!scanner.NextVariable(stackFrame.StackExecuter, out v))
                throw ThrowHelper.InvalidVariableName();

            string name = v.Name.ToString();
            ObjectContext context = stackFrame.Context.FindVariableContext(name);
            if (context == null) {
                stackFrame.Context.SetVariable(name, array_alloc(v.Indices, 0));
            }
            else {
                object obj = context.GetVariable(name);
                array_realloc(ref obj, v.Indices, 0);
                context.SetVariable(name, obj);
            }
            NULL(stackFrame);
        }

        private object array_alloc(int[] sizes, int index)
        {
            if (index < sizes.Length) {
                object[] o = new object[sizes[index]];
                index++;
                for (int i = 0; i < o.Length; i++) {
                    o[i] = array_alloc(sizes, index);
                }
                return o;
            }
            else {
                return null;
            }
        }

        private void array_realloc(ref object o, int[] sizes, int index)
        {
            if (o != null) {
                if (o.GetType().IsArray) {
                    object[] _aObj = (object[])o;
                    if (index < sizes.Length) {
                        Array.Resize<object>(ref _aObj, sizes[index]);
                        index++;
                        for (int i = 0; i < _aObj.Length; i++) {
                            array_realloc(ref _aObj[i], sizes, index);
                        }
                        o = _aObj;
                    }
                }
                else {
                    o = array_alloc(sizes, index);
                }
            }
        }

        private void Let(TFunctionData stackFrame)
        {
            SetVariable(stackFrame, constant: false);
        }

        internal void Const(TFunctionData stackFrame)
        {
            SetVariable(stackFrame, constant: true);
        }

        private void SetVariable(TFunctionData stackFrame, bool constant)
        {
            if (stackFrame.ParameterCount < 4) {
                stackFrame.AssertParamCount(4);
            }

            StringSegment text = new StringSegment(stackFrame.Text);

            Scanner scanner = new DefaultScanner(text);
            scanner.IntPosition += stackFrame.Name.Length;
            scanner.SkipWhiteSpace();

            Variable v;
            if (!scanner.NextVariable(stackFrame.StackExecuter, out v))
                throw ThrowHelper.InvalidVariableName();

            if (v.IsMacro)
                throw ThrowHelper.MacroRedefined();

            scanner.SkipWhiteSpace();

            if (!scanner.Next("="))
                throw ThrowHelper.InvalidDefinitionOperator();

            Evaluator e = new Evaluator(text.Subsegment(scanner.IntPosition), stackFrame.StackExecuter);
            object data = e.Evaluate();

            if (v.Indices == null) {
                if (!constant) {
                    stackFrame.Context.SetVariable(v.Name.ToString(), data);
                }
                else {
                    stackFrame.Context.SetConstant(v.Name.ToString(), data);
                }
            }
            else {
                if (constant)
                    throw ThrowHelper.ArraysCannotBeConstant();
                
                ObjectContext context = stackFrame.Context.FindVariableContext(v.Name.ToString());
                if (context == null)
                    throw new ArgumentException("Array has not been defined and cannot be indexed");
                context.SetArrayAt(v.Name.ToString(), data, v.Indices);
            }

            NULL(stackFrame);
        }
    }
}