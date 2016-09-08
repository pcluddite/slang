// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.IO;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Types;
using System.Linq;

namespace Tbasic.Runtime
{
    /// <summary>
    /// An event handler for a user exit
    /// </summary>
    /// <param name="sender">the object that has sent this message</param>
    /// <param name="e">any additional event arguments</param>
    public delegate void UserExittedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Executes a script and stores information on the current runtime
    /// </summary>
    public class TBasic
    {
        /// <summary>
        /// A string containing information on this version of TBasic
        /// </summary>
        public const string VERSION = "TBASIC 2.5.2016 Beta";

        #region Properties
        /// <summary>
        /// Gets or sets the scanner
        /// </summary>
        public IScanner Scanner { get; set; } = Scanners.Default;
        /// <summary>
        /// Gets or sets the preprocessor
        /// </summary>
        public IPreprocessor Preprocessor { get; set; } = Preprocessors.Default;

        /// <summary>
        /// The global context for this object
        /// </summary>
        public ObjectContext Global { get; private set; }

        /// <summary>
        /// Gets or sets the rules that the runtime should adhere to
        /// </summary>
        public ExecuterOption Options { get; set; } = ExecuterOption.None;

        /// <summary>
        /// Gets if request to break has been petitioned
        /// </summary>
        public bool BreakRequest { get; internal set; }

        /// <summary>
        /// Gets the current line in the script that the executer is processing
        /// </summary>
        public int CurrentLine { get; internal set; }

        /// <summary>
        /// Gets or sets the ObjectContext in which the code is executed
        /// </summary>
        public ObjectContext Context { get; set; }

        /// <summary>
        /// Gets if a request to exit has been petitioned. This should apply to the scope of the whole application.
        /// </summary>
        public static bool ExitRequest { get; internal set; }

        /// <summary>
        /// Raised when a user has requested to exit
        /// </summary>
        public event UserExittedEventHandler OnUserExitRequest;

        #endregion

        /// <summary>
        /// Initializes a new object to execute scripts or single lines of code
        /// </summary>
        public TBasic()
        {
            Global = new ObjectContext(null);
            Context = Global;
            CurrentLine = 0;
            BreakRequest = false;
        }

        /// <summary>
        /// Runs a script
        /// </summary>
        /// <param name="script">the full text of the script to process</param>
        public void Execute(string script)
        {
            Execute(new StringReader(script));
        }

        /// <summary>
        /// Runs a script
        /// </summary>
        /// <param name="lines">the lines of the script to process</param>
        public void Execute(TextReader lines)
        {
            IPreprocessor p = Preprocessor.Preprocess(this, lines);
            if (p.Functions.Count > 0) {
                foreach (FunctionBlock func in p.Functions) {
                    Global.AddFunction(func.Prototype[0], func.Execute);
                }
            }
            if (p.Classes.Count > 0) {
                foreach(TClass t in p.Classes) {
                    Global.AddType(t.Name, t);
                }
            }
            Execute(p.Lines);

            /*if (ManagedWindows.Count != 0 && !this.ExitRequest) {
                System.Windows.Forms.Application.Run(new FormLoader(this));
            }*/
        }

        /// <summary>
        /// Executes a single line of code
        /// </summary>
        /// <param name="codeLine"></param>
        /// <returns></returns>
        public StackData Execute(Line codeLine)
        {
#if !NO_EXCEPT
            TbasicRuntimeException runEx;
            try {
#endif
                return Execute(this, codeLine);
#if !NO_EXCEPT
            }
            catch(Exception ex) when ((runEx = TbasicRuntimeException.WrapException(ex)) != null) {
                throw runEx;
            }
#endif
        }

        internal StackData Execute(IList<Line> lines)
        {
            StackData runtime = null;
            for (int index = 0; index < lines.Count; index++) {
                if (BreakRequest) {
                    break;
                }
                Line current = lines[index];
                CurrentLine = current.LineNumber;
#if !NO_EXCEPT
                TbasicRuntimeException runEx;
                try {
#endif
                    BlockCreator block_init;
                    if (Context.TryGetBlock(current.Name, out block_init)) {
                        CodeBlock block = block_init(index, lines);
                        Context = Context.CreateSubContext();
                        block.Execute(this);
                        Context = Context.Collect();
                        index += block.Length - 1; // skip the length of the executed block
                    }
                    else {
                        runtime = Execute(this, current);
                    }
#if !NO_EXCEPT
                }
                catch (Exception ex) when ((runEx = TbasicRuntimeException.WrapException(ex)) != null) { // only catch errors that we understand 8/16/16
                    HandleError(current, runtime ?? new StackData(this), runEx);
                }
#endif
            }
            return runtime ?? new StackData(this);
        }

        internal static StackData Execute(TBasic exec, Line codeLine)
        {
            StackData runtime;
            CallData data;
            if (!codeLine.IsFunction && exec.Context.TryGetCommand(codeLine.Name, out data)) {
                runtime = new StackData(exec, codeLine.Text);
                if (data.Evaluate) {
                    runtime.EvaluateAll();
                }
                runtime.Data = data.Function(runtime);
            }
            else {
                runtime = new StackData(exec);
                ExpressionEvaluator eval = new ExpressionEvaluator(codeLine.Text, exec);
                exec.Context.PersistReturns(runtime);
                runtime.Data = eval.Evaluate();
            }
            runtime.Context.SetReturns(runtime);
            return runtime;
        }

        internal object ExecuteInContext(TBasicFunction func, StackData args, ObjectContext newcontext)
        {
            ObjectContext old = Context;
            Context = newcontext.CreateSubContext();
            args.Data = func(args);
            Context.SetReturns(args);
            Context = old;
            return args.Data;
        }

        internal StackData ExecuteInSubContext(IList<Line> lines)
        {
            StackData ret;
            Context = Context.CreateSubContext();
            ret = Execute(lines);
            Context = Context.Collect();
            return ret;
        }

        /// <summary>
        /// Executes a function or command with the given arguments
        /// </summary>
        public object ExecuteFunction(string name, params object[] args)
        {
            CallData calldat;
            StackData stackdat = new StackData(this, args);
            stackdat.Name = name;

            if (Context.TryGetFunction(name, out calldat) || Context.TryGetCommand(name, out calldat)) {
                if (calldat.Evaluate) {
                    stackdat.EvaluateAll();
                }
                return calldat.Function(stackdat);
            }
            else {
                throw ThrowHelper.UndefinedFunction(name);
            }
        }

        private void HandleError(Line current, StackData runtime, TbasicRuntimeException ex)
        {
            FunctionException cEx = ex as FunctionException;
            if (cEx != null) {
                int status = cEx.Status;
                string msg = runtime.Data as string;
                if (string.IsNullOrEmpty(msg)) {
                    msg = cEx.Message;
                }
                runtime.Status = status;
                runtime.Data = msg;
                runtime.Context.SetReturns(runtime);
                if (Options.HasFlag(ExecuterOption.ThrowErrors)) { // throw errors if the user wants it
                    throw new LineException(current.LineNumber, current.VisibleName, cEx);
                }
            }
            else {
                throw new LineException(current.LineNumber, current.VisibleName, ex);
            }
        }

        /// <summary>
        /// Requests a break from a code block
        /// </summary>
        public void RequestBreak()
        {
            BreakRequest = true;
        }

        /// <summary>
        /// Signals that a break request will be honored in the immediate future. This should only be called if you actually intend to break.
        /// </summary>
        public void HonorBreak()
        {
            if (!ExitRequest) {
                BreakRequest = false;
            }
        }

        /// <summary>
        /// Requests the script to exit. This implies a break request.
        /// </summary>
        public void RequestExit()
        {
            ExitRequest = true;
            BreakRequest = true;
            OnUserExit(EventArgs.Empty);
        }

        /// <summary>
        /// Enables an executer option
        /// </summary>
        /// <param name="option"></param>
        public void EnableOption(ExecuterOption option)
        {
            Options |= option;
        }

        /// <summary>
        /// Disables an executer option
        /// </summary>
        /// <param name="option"></param>
        public void DisableOption(ExecuterOption option)
        {
            Options &= ~option;
        }

        /// <summary>
        /// Gets whether or not an option has been enabled
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public bool IsEnforced(ExecuterOption option)
        {
            return Options.HasFlag(option);
        }

        private void OnUserExit(EventArgs e)
        {
            OnUserExitRequest?.Invoke(this, e);
        }
    }
}
