// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Reflection;

namespace Tbasic.Runtime
{
    /// <summary>
    /// Delegate for processing a TBasic function
    /// </summary>
    /// <param name="stack">The object containing parameter and execution information</param>
    public delegate object TBasicFunction(StackData stack);

    /// <summary>
    /// Contains call information for each function or command
    /// </summary>
    public struct CallData
    {
        /// <summary>
        /// Gets the argument count for this function (excluding the function name)
        /// </summary>
        public int ArgumentCount { get; }
        /// <summary>
        /// Gets the TBasicFunction delegate that is called
        /// </summary>
        public TBasicFunction Function { get; }
        /// <summary>
        /// Gets the delegate for the native method that is being called. If this is not a direct call to a native method, this value is null.
        /// </summary>
        public Delegate NativeDelegate { get; }
        /// <summary>
        /// Gets a value indicating whether this function returns anything (i.e. whether or not this is a void function)
        /// </summary>
        public bool Returns { get; }
        /// <summary>
        /// Gets a value indicating whether or not this function's arguments should be evaluated
        /// </summary>
        public bool Evaluate { get; }

        /// <summary>
        /// Constructs a CallData object
        /// </summary>
        /// <param name="func">the TBasicFunction that is called</param>
        /// <param name="evaluate">whether or not its parameters should be evaluated</param>
        public CallData(TBasicFunction func, bool evaluate)
        {
            Function = func;
            NativeDelegate = null;
            ArgumentCount = -1;
            Returns = true;
            Evaluate = evaluate;
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
            Evaluate = evaluate;
            Function = null; // squelching error message about not all fields assigned 8/18/16
            Function = NativeFuncWrapper;
        }

        private object NativeFuncWrapper(StackData runtime)
        {
            runtime.AssertCount(ArgumentCount + 1); // plus 1 for the name

            object[] args = new object[runtime.ParameterCount - 1];
            ParameterInfo[] expectedArgs = NativeDelegate.Method.GetParameters();

            for (int index = 1; index < runtime.ParameterCount; ++index) // make sure the types are correct for each parameter
                args[index - 1] = runtime.ConvertAt(index, expectedArgs[index - 1].ParameterType);

            if (Returns) {
                return NativeDelegate.DynamicInvoke(args);
            }
            else {
                NativeDelegate.DynamicInvoke(args);
                return null;
            }
        }

        /// <summary>
        /// Converts a TBasicFunction to CallData
        /// </summary>
        /// <param name="func"></param>
        public static implicit operator CallData(TBasicFunction func)
        {
            return new CallData(func, evaluate: true);
        }

        /// <summary>
        /// Converts CallData to a TBasicFunction. This is by its nature lossy, so it requires explicit conversion.
        /// </summary>
        /// <param name="data"></param>
        public static explicit operator TBasicFunction(CallData data)
        {
            return data.Function;
        }
    }
}
