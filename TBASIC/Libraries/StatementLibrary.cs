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
            Add("LET", Let);
            Add("DIM", DIM);
            Add("SLEEP", Sleep);
            Add("ELSE", UhOh);
            Add("END", UhOh);
            Add("WEND", UhOh);
            Add("CONST", Const);
            Add("EXIT", Exit);
            Add("BREAK", Break);
            Add("OPTION", Option);
            Add("OPT", Option);
        }

        private object Include(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            string path = Path.GetFullPath(stackFrame.GetAt<string>(1));
            if (!File.Exists(path)) {
                throw new FileNotFoundException();
            }

            CodeBlock[] funcs;
            LineCollection lines = Executer.ScanLines(File.ReadAllLines(path), out funcs);



            return NULL(stackFrame);
        }

        private static object Option(RuntimeData fData)
        {
            if (fData.ParameterCount == 2)
                fData.Add(true);
            fData.AssertCount(3);

            string szOpt = fData.GetAt<string>(1);
            ExecuterOption opt;
            if (!Enum.TryParse(szOpt, out opt)) {
                opt = fData.GetAt<ExecuterOption>(1); // this will throw an error if its not a valid flag
            }

            if (fData.EvaluateAt<bool>(2)) {
                fData.StackExecuter.EnableOption(opt);
            }
            else {
                fData.StackExecuter.DisableOption(opt);
            }
            return NULL(fData);
        }

        private object Sleep(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            System.Threading.Thread.Sleep(stackFrame.GetAt<int>(1));
            return NULL(stackFrame);
        }

        private object Break(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(1);
            stackFrame.StackExecuter.RequestBreak();
            return NULL(stackFrame);
        }

        internal object Exit(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(1);
            stackFrame.StackExecuter.RequestExit();
            return NULL(stackFrame);
        }

        internal static object NULL(RuntimeData stackFrame)
        {
            stackFrame.Context.PersistReturns(stackFrame);
            return stackFrame.Data;
        }

        internal object UhOh(RuntimeData stackFrame)
        {
            throw ThrowHelper.NoOpeningStatement(stackFrame.Text);
        }

        internal object DIM(RuntimeData stackFrame)
        {
            stackFrame.AssertAtLeast(2);

            StringSegment text = new StringSegment(stackFrame.Text);
            Scanner scanner = stackFrame.StackExecuter.ScannerDelegate(text);
            scanner.IntPosition += stackFrame.Name.Length;

            Variable v;
            if (!scanner.NextVariableInternal(stackFrame.StackExecuter, out v))
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
            return NULL(stackFrame);
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

        private object Let(RuntimeData stackFrame)
        {
            return SetVariable(stackFrame, constant: false);
        }

        internal object Const(RuntimeData stackFrame)
        {
            return SetVariable(stackFrame, constant: true);
        }

        private object SetVariable(RuntimeData stackFrame, bool constant)
        {
            stackFrame.AssertAtLeast(2);
            StringSegment text = new StringSegment(stackFrame.Text);

            Scanner scanner = stackFrame.StackExecuter.ScannerDelegate(text);
            scanner.IntPosition += stackFrame.Name.Length;

            Variable v;
            if (!scanner.NextVariableInternal(stackFrame.StackExecuter, out v))
                throw ThrowHelper.InvalidVariableName();

            if (v.IsMacro)
                throw ThrowHelper.MacroRedefined();

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

            return data;
        }
    }
}