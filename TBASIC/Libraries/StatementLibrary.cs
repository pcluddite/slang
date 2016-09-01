// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.IO;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Runtime;
using Tbasic.Types;

namespace Tbasic.Libraries
{
    internal class StatementLibrary : Library
    {
        public StatementLibrary()
        {
            Add("LET", Let, evaluate: false);
            Add("SET", Let, evaluate: false);
            Add("DIM", DIM, evaluate: false);
            Add("OPTION", Option, evaluate: false);
            Add("OPT", Option, evaluate: false);
            Add("SLEEP", Sleep);
            Add("ELSE", UhOh);
            Add("END", UhOh);
            Add("WEND", UhOh);
            Add("CONST", Const);
            Add("EXIT", Exit);
            Add("BREAK", Break);
            Add("#INCLUDE", Include);
        }

        private static object Include(StackData stackdat)
        {
            stackdat.AssertCount(2);
            string path = Path.GetFullPath(stackdat.EvaluateAt<string>(1));

            try {
                Preprocessor p;
                using (StreamReader reader = new StreamReader(File.OpenRead(path))) {
                    p = Preprocessor.Preprocess(reader, stackdat.Runtime);
                }

                if (p.Functions.Count > 0) {
                    foreach (FuncBlock func in p.Functions) {
                        ObjectContext context = stackdat.Context.FindFunctionContext(func.Prototype.Name);
                        if (context != null)
                            throw ThrowHelper.AlreadyDefined(func.Prototype.Name + "()");
                        stackdat.Context.SetFunction(func.Prototype.Name, func.CreateDelegate());
                    }
                }
            }
            catch(Exception ex) when (ex is TbasicRuntimeException || ex is IOException || ex is UnauthorizedAccessException) {
                throw new TbasicRuntimeException("Unable to load library. " + ex.Message, ex);
            }

            return NULL(stackdat);
        }

        private static object Option(StackData stackdat)
        {
            if (stackdat.ParameterCount == 2)
                stackdat.Add(true);
            stackdat.AssertCount(3);

            string szOpt = stackdat.GetAt<string>(1);
            ExecuterOption opt;
            if (!Enum.TryParse(szOpt, out opt)) {
                opt = stackdat.GetAt<ExecuterOption>(1); // this will throw an error if its not a valid flag
            }

            if (stackdat.EvaluateAt<bool>(2)) {
                stackdat.Runtime.EnableOption(opt);
            }
            else {
                stackdat.Runtime.DisableOption(opt);
            }
            return NULL(stackdat);
        }

        private object Sleep(StackData stackdat)
        {
            stackdat.AssertCount(2);
            System.Threading.Thread.Sleep(stackdat.GetAt<int>(1));
            return NULL(stackdat);
        }

        private object Break(StackData stackdat)
        {
            stackdat.AssertCount(1);
            stackdat.Runtime.RequestBreak();
            return NULL(stackdat);
        }

        internal object Exit(StackData stackdat)
        {
            stackdat.AssertCount(1);
            stackdat.Runtime.RequestExit();
            return NULL(stackdat);
        }

        internal static object NULL(StackData stackdat)
        {
            stackdat.Context.PersistReturns(stackdat);
            return stackdat.Data;
        }

        internal object UhOh(StackData stackdat)
        {
            throw ThrowHelper.NoOpeningStatement(stackdat.Text);
        }

        internal object DIM(StackData stackdat)
        {
            stackdat.AssertAtLeast(2);

            StringSegment text = new StringSegment(stackdat.Text);
            Scanner scanner = stackdat.Runtime.ScannerDelegate(text);
            scanner.IntPosition += stackdat.Name.Length;
            scanner.SkipWhiteSpace();

            Variable v;
            if (!scanner.NextVariableInternal(stackdat.Runtime, out v))
                throw ThrowHelper.InvalidVariableName();

            string name = v.Name.ToString();
            ObjectContext context = stackdat.Context.FindVariableContext(name);
            if (context == null) {
                if (v.Indices != null) {
                    stackdat.Context.SetVariable(name, array_alloc(v.EvaluateIndices(), 0));
                }
                else {
                    if (!scanner.EndOfStream && scanner.Next() == "=") {
                        SetVariable(stackdat, false);
                    }
                    else {
                        stackdat.Context.SetVariable(name, null); // just declare it.
                    }
                }
            }
            else {
                object obj = context.GetVariable(name);
                array_realloc(ref obj, v.EvaluateIndices(), 0);
                context.SetVariable(name, obj);
            }
            return NULL(stackdat);
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

        private object Let(StackData stackdat)
        {
            return SetVariable(stackdat, constant: false);
        }

        internal object Const(StackData stackdat)
        {
            return SetVariable(stackdat, constant: true);
        }

        private object SetVariable(StackData stackdat, bool constant)
        {
            stackdat.AssertAtLeast(2);
            StringSegment text = new StringSegment(stackdat.Text);

            Scanner scanner = stackdat.Runtime.ScannerDelegate(text);
            scanner.IntPosition += stackdat.Name.Length;
            scanner.SkipWhiteSpace();

            Variable v;
            if (!scanner.NextVariableInternal(stackdat.Runtime, out v))
                throw ThrowHelper.InvalidVariableName();

            if (v.IsMacro)
                throw ThrowHelper.MacroRedefined();

            scanner.SkipWhiteSpace();
            if (!scanner.Next("="))
                throw ThrowHelper.InvalidDefinitionOperator();

            ExpressionEvaluator e = new ExpressionEvaluator(text.Subsegment(scanner.IntPosition), stackdat.Runtime);
            object data = e.Evaluate();

            if (v.Indices == null) {
                if (!constant) {
                    stackdat.Context.SetVariable(v.Name.ToString(), data);
                }
                else {
                    stackdat.Context.SetConstant(v.Name.ToString(), data);
                }
            }
            else {
                if (constant)
                    throw ThrowHelper.ArraysCannotBeConstant();
                
                ObjectContext context = stackdat.Context.FindVariableContext(v.Name.ToString());
                if (context == null)
                    throw new ArgumentException("Array has not been defined and cannot be indexed");
                context.SetArrayAt(v.Name.ToString(), data, v.EvaluateIndices());
            }

            return data;
        }
    }
}