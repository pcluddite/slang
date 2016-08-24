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

        private object Include(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            string path = Path.GetFullPath(runtime.GetAt<string>(1));
            if (!File.Exists(path)) {
                throw new FileNotFoundException();
            }

            CodeBlock[] funcs;
            //LineCollection lines = Executer.ScanLines(File.ReadAllLines(path), out funcs);



            return NULL(runtime);
        }

        private static object Option(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 2)
                runtime.Add(true);
            runtime.AssertCount(3);

            string szOpt = runtime.GetAt<string>(1);
            ExecuterOption opt;
            if (!Enum.TryParse(szOpt, out opt)) {
                opt = runtime.GetAt<ExecuterOption>(1); // this will throw an error if its not a valid flag
            }

            if (runtime.EvaluateAt<bool>(2)) {
                runtime.StackExecuter.EnableOption(opt);
            }
            else {
                runtime.StackExecuter.DisableOption(opt);
            }
            return NULL(runtime);
        }

        private object Sleep(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            System.Threading.Thread.Sleep(runtime.GetAt<int>(1));
            return NULL(runtime);
        }

        private object Break(RuntimeData runtime)
        {
            runtime.AssertCount(1);
            runtime.StackExecuter.RequestBreak();
            return NULL(runtime);
        }

        internal object Exit(RuntimeData runtime)
        {
            runtime.AssertCount(1);
            runtime.StackExecuter.RequestExit();
            return NULL(runtime);
        }

        internal static object NULL(RuntimeData runtime)
        {
            runtime.Context.PersistReturns(runtime);
            return runtime.Data;
        }

        internal object UhOh(RuntimeData runtime)
        {
            throw ThrowHelper.NoOpeningStatement(runtime.Text);
        }

        internal object DIM(RuntimeData runtime)
        {
            runtime.AssertAtLeast(2);

            StringSegment text = new StringSegment(runtime.Text);
            Scanner scanner = runtime.StackExecuter.ScannerDelegate(text);
            scanner.IntPosition += runtime.Name.Length;
            scanner.SkipWhiteSpace();

            Variable v;
            if (!scanner.NextVariableInternal(runtime.StackExecuter, out v))
                throw ThrowHelper.InvalidVariableName();

            string name = v.Name.ToString();
            ObjectContext context = runtime.Context.FindVariableContext(name);
            if (context == null) {
                runtime.Context.SetVariable(name, array_alloc(v.Indices, 0));
            }
            else {
                object obj = context.GetVariable(name);
                array_realloc(ref obj, v.Indices, 0);
                context.SetVariable(name, obj);
            }
            return NULL(runtime);
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

        private object Let(RuntimeData runtime)
        {
            return SetVariable(runtime, constant: false);
        }

        internal object Const(RuntimeData runtime)
        {
            return SetVariable(runtime, constant: true);
        }

        private object SetVariable(RuntimeData runtime, bool constant)
        {
            runtime.AssertAtLeast(2);
            StringSegment text = new StringSegment(runtime.Text);

            Scanner scanner = runtime.StackExecuter.ScannerDelegate(text);
            scanner.IntPosition += runtime.Name.Length;
            scanner.SkipWhiteSpace();

            Variable v;
            if (!scanner.NextVariableInternal(runtime.StackExecuter, out v))
                throw ThrowHelper.InvalidVariableName();

            if (v.IsMacro)
                throw ThrowHelper.MacroRedefined();

            if (!scanner.Next("="))
                throw ThrowHelper.InvalidDefinitionOperator();

            Evaluator e = new Evaluator(text.Subsegment(scanner.IntPosition), runtime.StackExecuter);
            object data = e.Evaluate();

            if (v.Indices == null) {
                if (!constant) {
                    runtime.Context.SetVariable(v.Name.ToString(), data);
                }
                else {
                    runtime.Context.SetConstant(v.Name.ToString(), data);
                }
            }
            else {
                if (constant)
                    throw ThrowHelper.ArraysCannotBeConstant();
                
                ObjectContext context = runtime.Context.FindVariableContext(v.Name.ToString());
                if (context == null)
                    throw new ArgumentException("Array has not been defined and cannot be indexed");
                context.SetArrayAt(v.Name.ToString(), data, v.Indices);
            }

            return data;
        }
    }
}