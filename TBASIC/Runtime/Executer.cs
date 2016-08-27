// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;
using System.IO;

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
    public class Executer
    {
        /// <summary>
        /// A string containing information on this version of TBasic
        /// </summary>
        public const string VERSION = "TBASIC 2.5.2016 Beta";

        #region Properties
        /// <summary>
        /// Gets or sets the scanner creation delegate
        /// </summary>
        public CreateScannerDelegate ScannerDelegate { get; set; } = Scanner.Default;

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
        public Executer()
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
            Preprocessor p = Preprocessor.Preprocess(lines, this);
            if (p.Functions.Count > 0) {
                foreach (FuncBlock func in p.Functions) {
                    if (Global.FindFunctionContext(func.Template.Name) != null)
                        throw ThrowHelper.AlreadyDefined(func.Template.Name + "()");
                    Global.SetFunction(func.Template.Name, func.CreateDelegate());
                }
            }
            if (p.Types.Count > 0) {
                foreach(TClass t in p.Types) {
                    Global.AddType(t);
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
        public RuntimeData Execute(Line codeLine)
        {
            try {
                return Execute(this, codeLine);
            }
            catch(Exception ex) {
                TbasicRuntimeException runEx = TbasicRuntimeException.WrapException(ex);
                if (runEx == null)
                    throw;
                throw runEx;
            }
        }

        internal RuntimeData Execute(LineCollection lines)
        {
            RuntimeData runtime = null;
            for (int index = 0; index < lines.Count; index++) {
                if (BreakRequest) {
                    break;
                }
                Line current = lines[index];
                CurrentLine = current.LineNumber;
                try {
                    ObjectContext blockContext = Context.FindBlockContext(current.Name);
                    if (blockContext == null) {
                        runtime = Execute(this, current);
                    }
                    else {
                        CodeBlock block = blockContext.GetBlock(current.Name).Invoke(index, lines);
                        Context = Context.CreateSubContext();
                        block.Execute(this);
                        Context = Context.Collect();
                        index += block.Length - 1; // skip the length of the executed block
                    }
                }
                catch (Exception ex) {
                    TbasicRuntimeException runEx = TbasicRuntimeException.WrapException(ex);
                    if (runEx == null) // only catch errors that we understand 8/16/16
                        throw;
                    HandleError(current, runtime ?? new RuntimeData(this), runEx);
                }
            }
            return runtime ?? new RuntimeData(this);
        }

        internal static RuntimeData Execute(Executer exec, Line codeLine)
        {
            RuntimeData runtime;
            ObjectContext context = exec.Context.FindCommandContext(codeLine.Name);
            if (context == null) {
                runtime = new RuntimeData(exec);
                Evaluator eval = new Evaluator(new StringSegment(codeLine.Text), exec);
                exec.Context.PersistReturns(runtime);
                runtime.Data = eval.Evaluate();
            }
            else {
                runtime = new RuntimeData(exec, codeLine.Text);
                runtime.Data = context.GetCommand(codeLine.Name).Invoke(runtime);
            }
            runtime.Context.SetReturns(runtime);
            return runtime;
        }

        private void HandleError(Line current, RuntimeData runtime, TbasicRuntimeException ex)
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
