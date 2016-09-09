﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Reflection;
using Tint.Runtime;

namespace Tint.Types
{
    /// <summary>
    /// Delegate for processing a Tint function
    /// </summary>
    /// <param name="runtime">the runtime that called this function</param>
    /// <param name="stackdat">The object containing parameter and execution information</param>
    public delegate object TintFunction(TRuntime runtime, StackData stackdat);

    /// <summary>
    /// Contains call information for each function or command
    /// </summary>
    public struct CallData
    {
        /// <summary>
        /// Gets or sets the argument count for this function (excluding the function name). This is only relevant for native functions.
        /// </summary>
        public int ArgumentCount { get; set; }
        /// <summary>
        /// Gets the TintFunction delegate that is called
        /// </summary>
        public TintFunction Function { get; }
        /// <summary>
        /// Gets the delegate for the native method that is being called. If this is not a direct call to a native method, this value is null.
        /// </summary>
        public Delegate NativeDelegate { get; }
        /// <summary>
        /// Gets a value indicating whether this function returns anything (i.e. whether or not this is a void function)
        /// </summary>
        public bool Returns { get; }
        /// <summary>
        /// Gets or sets a value indicating whether or not this function's arguments should be evaluated
        /// </summary>
        public bool ShouldEvaluate { get; set; }

        /// <summary>
        /// Constructs a CallData object
        /// </summary>
        /// <param name="func">the TintFunction that is called</param>
        /// <param name="evaluate">whether or not its parameters should be evaluated</param>
        public CallData(TintFunction func, bool evaluate)
        {
            Function = func;
            NativeDelegate = null;
            ArgumentCount = -1;
            Returns = true;
            ShouldEvaluate = evaluate;
        }

        /// <summary>
        /// Constructs a CallData object for a native method
        /// </summary>
        /// <param name="d">the delegate to the native C# delegate</param>
        /// <param name="args">the number of arguments this function takes. If this is different from the native method's argument number, default values are passed</param>
        /// <param name="evaluate">whether or not its parameters should be evaluated</param>
        public CallData(Delegate d, int args, bool evaluate)
        {
            ArgumentCount = args;
            NativeDelegate = d;
            Returns = (d.Method.ReturnType != null);
            ShouldEvaluate = evaluate;
            Function = null; // squelching error message about not all fields assigned 8/18/16
            Function = NativeFuncWrapper;
        }

        private object NativeFuncWrapper(TRuntime runtime, StackData stackdat)
        {
            ParameterInfo[] expectedArgs = NativeDelegate.Method.GetParameters();
            if (expectedArgs.Length == ArgumentCount) {
                stackdat.AssertCount(ArgumentCount + 1); // plus 1 for the name
            }
            else {
                stackdat.AssertAtLeast(ArgumentCount + 1); // plus 1 for the name
            }
            object[] args = new object[expectedArgs.Length];

            int index = 0;
            for (; index < stackdat.ParameterCount - 1; ++index) // make sure the types are correct for each parameter
                args[index] = stackdat.Convert(index + 1, expectedArgs[index].ParameterType);

            for (; index < expectedArgs.Length; ++index) { // default initialize any remaining values
                if (expectedArgs[index].ParameterType.IsValueType) {
                    args[index] = Activator.CreateInstance(expectedArgs[index].ParameterType);
                }
                else {
                    args[index] = null;
                }
            }

            if (Returns) {
                return NativeDelegate.DynamicInvoke(args);
            }
            else {
                NativeDelegate.DynamicInvoke(args);
                return null;
            }
        }

        /// <summary>
        /// Converts a TintFunction to CallData
        /// </summary>
        /// <param name="func"></param>
        public static implicit operator CallData(TintFunction func)
        {
            return new CallData(func, evaluate: true);
        }

        /// <summary>
        /// Converts CallData to a TintFunction. This is by its nature lossy, so it requires explicit conversion.
        /// </summary>
        /// <param name="data"></param>
        public static explicit operator TintFunction(CallData data)
        {
            return data.Function;
        }
    }
}
