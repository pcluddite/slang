﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;

namespace Tbasic.Runtime
{
    /// <summary>
    /// An event handler for a user exit
    /// </summary>
    /// <param name="sender">the object that has sent this message</param>
    /// <param name="e">any additional event arguments</param>
    public delegate void UserExittedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Executes a script and stores information on the current state of the line being executed
    /// </summary>
    public class Executer
    {
        /// <summary>
        /// A string containing information on this version of Tbasic
        /// </summary>
        public const string VERSION = "TBASIC 2.5.2016 Beta";

        #region Properties
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
        /// Initializes a new script executer
        /// </summary>
        public Executer()
        {
            Global = new ObjectContext(null);
            Context = Global;
            CurrentLine = 0;
            BreakRequest = false;
        }

        /// <summary>
        /// Runs a Tbasic script
        /// </summary>
        /// <param name="script">the full text of the script to process</param>
        public void Execute(string script)
        {
            Execute(script.Replace("\r\n", "\n").Split('\n'));
        }

        /// <summary>
        /// Runs a Tbasic script
        /// </summary>
        /// <param name="lines">the lines of the script to process</param>
        public void Execute(string[] lines)
        {
            CodeBlock[] userFuncs;
            LineCollection code = ScanLines(lines, out userFuncs);

            if (userFuncs != null && userFuncs.Length > 0) {
                foreach (CodeBlock cb in userFuncs) {
                    FuncBlock fBlock = (FuncBlock)cb;
                    Global.SetFunction(fBlock.Template.Name, fBlock.CreateDelegate());
                }
            }
            Execute(code);

            /*if (ManagedWindows.Count != 0 && !this.ExitRequest) {
                System.Windows.Forms.Application.Run(new FormLoader(this));
            }*/
        }

        /// <summary>
        /// Executes a single line of code
        /// </summary>
        /// <param name="codeLine"></param>
        /// <returns></returns>
        public FuncData Execute(Line codeLine)
        {
            FuncData data = new FuncData(this);
            try {
                Execute(ref data, codeLine);
            }
            catch(Exception ex) {
                TbasicRuntimeException runEx = TbasicRuntimeException.WrapException(ex);
                if (runEx == null)
                    throw;
                throw runEx;
            }
            return data;
        }

        internal FuncData Execute(LineCollection lines)
        {
            FuncData stackFrame = new FuncData(this);
            for (int index = 0; index < lines.Count; index++) {
                if (BreakRequest) {
                    break;
                }
                Line current = lines[index];
                CurrentLine = current.LineNumber;
                try {
                    ObjectContext blockContext = Context.FindBlockContext(current.Name);
                    if (blockContext == null) {
                        Execute(ref stackFrame, current);
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
                    if (ex == null) // only catch errors that we understand 8/16/16
                        throw;
                    HandleError(current, stackFrame, runEx);
                }
            }
            return stackFrame;
        }

        internal static void Execute(ref FuncData stackFrame, Line codeLine)
        {
            ObjectContext context = stackFrame.Context.FindCommandContext(codeLine.Name);
            if (context == null || codeLine.IsFunction) {
                Evaluator eval = new Evaluator(new StringSegment(codeLine.Text), stackFrame.StackExecuter);
                object result = eval.Evaluate();
                stackFrame.Context.PersistReturns(stackFrame);
                stackFrame.Data = result;
            }
            else {
                stackFrame = new FuncData(stackFrame.StackExecuter, codeLine.Text);
                stackFrame.Data = context.GetCommand(codeLine.Name).Invoke(stackFrame);
            }
            stackFrame.Context.SetReturns(stackFrame);
        }

        private void HandleError(Line current, FuncData stackFrame, TbasicRuntimeException ex)
        {
            FunctionException cEx = ex as FunctionException;
            if (cEx != null) {
                int status = cEx.Status;
                string msg = stackFrame.Data as string;
                if (string.IsNullOrEmpty(msg)) {
                    msg = cEx.Message;
                }
                stackFrame.Status = status;
                stackFrame.Data = msg;
                stackFrame.Context.SetReturns(stackFrame);
                if (Options.HasFlag(ExecuterOption.ThrowErrors)) { // only actually throw anything if we understand the error
                    throw new LineException(current.LineNumber, current.VisibleName, cEx);
                }
            }
            else {
                throw new LineException(current.LineNumber, current.VisibleName, ex.InnerException);
            }
        }

        internal static LineCollection ScanLines(string[] lines, out CodeBlock[] userFunctions)
        {
            LineCollection allLines = new LineCollection();
            List<int> funLines = new List<int>();

            for (int lineNumber = 0; lineNumber < lines.Length; ++lineNumber) {
                Line current = new Line(lineNumber + 1, lines[lineNumber]); // Tag all lines with its line number (index + 1)

                if (string.IsNullOrEmpty(current.Text) || current.Text[0] == ';') {
                    continue;
                }
                if (current.Name[current.Name.Length - 1] == '$' || current.Name[current.Name.Length - 1] == ']') {
                    current.Text = "LET " + current.Text; // add the word LET if it's an equality, but use the original name as visible name
                }
                else if (current.Name.EqualsIgnoreCase("FUNCTION")) {
                    funLines.Add(current.LineNumber);
                }
                else {
                    current.VisibleName = current.VisibleName.ToUpper();
                }

                while (current.Text[current.Text.Length - 1] == '_') { // line continuation
                    lineNumber++;
                    if (lineNumber >= lines.Length) {
                        throw new EndOfCodeException("line continuation character '_' cannot end script");
                    }
                    current = new Line(current.LineNumber, current.Text.Remove(current.Text.LastIndexOf('_')) + lines[lineNumber].Trim());
                }

                allLines.Add(current);
            }

            List<CodeBlock> userFuncs = new List<CodeBlock>();
            foreach (int funcLine in funLines) {
                FuncBlock func = new FuncBlock(allLines.IndexOf(funcLine), allLines);
                userFuncs.Add(func);
                allLines.Remove(func.Header);
                allLines.Remove(func.Body);
                allLines.Remove(func.Footer);
            }
            userFunctions = userFuncs.ToArray();
            return allLines;
        }

        /// <summary>
        /// Requests a break from a code block
        /// </summary>
        public void RequestBreak()
        {
            BreakRequest = true;
        }

        /// <summary>
        /// Signals that a break request will be honored in the immediate future
        /// </summary>
        public void HonorBreak()
        {
            if (!ExitRequest) {
                BreakRequest = false;
            }
        }

        /// <summary>
        /// Requests the script to exit
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
