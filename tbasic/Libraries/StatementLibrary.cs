// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            Add("IMPORT", Include);
        }

        private static object Include(TRuntime runtime, StackData stackdat)
        {
            stackdat.AssertCount(2);
            string path = Path.GetFullPath(stackdat.Evaluate<string>(1, runtime));

            try {
                IPreprocessor p;
                using (StreamReader reader = new StreamReader(File.OpenRead(path))) {
                    p = runtime.Preprocessor.Preprocess(runtime, reader);
                }

                if (p.Functions.Count > 0) {
                    foreach (FunctionBlock func in p.Functions) {
                        runtime.Context.AddFunction(func.Prototype.First(), func.Execute);
                    }
                }
                if (p.Classes.Count > 0) {
                    foreach (TClass t in p.Classes) {
                        runtime.Context.AddClass(t.Name, t);
                    }
                }
            }
            catch(Exception ex) when (ex is TbasicRuntimeException || ex is IOException || ex is UnauthorizedAccessException) {
                throw new TbasicRuntimeException("Unable to load library. " + ex.Message, ex);
            }

            return NULL(runtime, stackdat);
        }

        private static object Option(TRuntime runtime, StackData stackdat)
        {
            if (stackdat.ParameterCount == 2)
                stackdat.Add(true);
            stackdat.AssertCount(3);

            string szOpt = stackdat.Get<string>(1);
            ExecuterOption opt;
            if (!Enum.TryParse(szOpt, out opt)) {
                opt = stackdat.Get<ExecuterOption>(1); // this will throw an error if its not a valid flag
            }

            if (stackdat.Evaluate<bool>(2, runtime)) {
                runtime.EnableOption(opt);
            }
            else {
                runtime.DisableOption(opt);
            }
            return NULL(runtime, stackdat);
        }

        private object Sleep(TRuntime runtime, StackData stackdat)
        {
            stackdat.AssertCount(2);
            System.Threading.Thread.Sleep(stackdat.Get<int>(1));
            return NULL(runtime, stackdat);
        }

        private object Break(TRuntime runtime, StackData stackdat)
        {
            stackdat.AssertCount(1);
            runtime.RequestBreak();
            return NULL(runtime, stackdat);
        }

        internal object Exit(TRuntime runtime, StackData stackdat)
        {
            stackdat.AssertCount(1);
            runtime.RequestExit();
            return NULL(runtime, stackdat);
        }

        internal static object NULL(TRuntime runtime, StackData stackdat)
        {
            runtime.Context.PersistReturns(stackdat);
            return stackdat.ReturnValue;
        }

        internal object UhOh(TRuntime runtime, StackData stackdat)
        {
            throw ThrowHelper.NoOpeningStatement(stackdat.Text);
        }

        internal object DIM(TRuntime runtime, StackData stackdat)
        {
            stackdat.AssertAtLeast(2);
            
            IScanner scanner = runtime.Scanner.Scan(stackdat.Text);
            scanner.Position += stackdat.Name.Length;
            scanner.SkipWhiteSpace();

            VariableEvaluator v;
            if (!DefaultScanner.NextVariable(scanner, runtime, out v))
                throw ThrowHelper.InvalidVariableName();

            string name = v.Name.ToString();
            ObjectContext context = runtime.Context.FindVariableContext(name);
            if (context == null) {
                if (v.Indices != null) {
                    runtime.Context.SetVariable(name, array_alloc(v.EvaluateIndices(), 0));
                }
                else {
                    scanner.SkipWhiteSpace();
                    if (!scanner.EndOfStream && scanner.Next("=")) {
                        SetVariable(runtime, stackdat, false);
                    }
                    else {
                        runtime.Context.SetVariable(name, (IRuntimeObject)null); // just declare it.
                    }
                }
            }
            else {
                IRuntimeObject obj = context.GetVariable(name).Value;
                array_realloc(ref obj, v.EvaluateIndices(), 0);
                context.SetVariable(name, obj);
            }
            return NULL(runtime, stackdat);
        }

        private IRuntimeObject array_alloc(int[] sizes, int index)
        {
            if (index < sizes.Length) {
                IRuntimeObject[] o = new IRuntimeObject[sizes[index]];
                index++;
                for (int i = 0; i < o.Length; i++) {
                    o[i] = array_alloc(sizes, index);
                }
                return new TbasicArray(o);
            }
            else {
                return null;
            }
        }

        private void array_realloc(ref IRuntimeObject o, int[] sizes, int index)
        {
            if (o != null) {
                if (o.TypeCode == TbasicType.Array) {
                    IRuntimeObject[] _aObj = (IRuntimeObject[])o.Value;
                    if (index < sizes.Length) {
                        Array.Resize<IRuntimeObject>(ref _aObj, sizes[index]);
                        index++;
                        for (int i = 0; i < _aObj.Length; i++) {
                            array_realloc(ref _aObj[i], sizes, index);
                        }
                        o = new TbasicArray(_aObj);
                    }
                }
                else {
                    o = array_alloc(sizes, index);
                }
            }
        }

        private object Let(TRuntime runtime, StackData stackdat)
        {
            return SetVariable(runtime, stackdat, constant: false);
        }

        internal object Const(TRuntime runtime, StackData stackdat)
        {
            return SetVariable(runtime, stackdat, constant: true);
        }

        private object SetVariable(TRuntime runtime, StackData stackdat, bool constant)
        {
            stackdat.AssertAtLeast(2);

            IScanner scanner = runtime.Scanner.Scan(stackdat.Text);
            scanner.Position += stackdat.Name.Length;

            VariableEvaluator v;
            if (!DefaultScanner.NextVariable(scanner, runtime, out v))
                throw ThrowHelper.InvalidVariableName();

            if (!scanner.Next("="))
                throw ThrowHelper.InvalidDefinitionOperator();

            ExpressionEvaluator e = new ExpressionEvaluator(stackdat.Text.Substring(scanner.Position + 1), runtime);
            IRuntimeObject result = e.Evaluate();

            if (v.Indices == null) {
                if (!constant) {
                    //runtime.Context.SetVariable(v.Name.ToString(), data);
                }
                else {
                    //runtime.Context.SetConstant(v.Name.ToString(), data);
                }
            }
            else {
                if (constant)
                    throw ThrowHelper.ArraysCannotBeConstant();
                
                ObjectContext context = runtime.Context.FindVariableContext(v.Name.ToString());
                if (context == null)
                    throw new ArgumentException("Array has not been defined and cannot be indexed");
                context.SetArrayAt(v.Name.ToString(), result, v.EvaluateIndices());
            }

            return result;
        }
    }
}