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

    internal struct CallData
    {
        public int ArgumentCount { get; set; }
        public TBasicFunction Function { get; set; }
        public Delegate CalledDelegate { get; set; }
        public bool ReturnsType { get; private set; }
        public bool Evaluate { get; private set; }

        public CallData(TBasicFunction func, bool evaluate)
        {
            Function = func;
            CalledDelegate = null;
            ArgumentCount = -1;
            ReturnsType = true;
            Evaluate = evaluate;
        }

        public CallData(Delegate d, int args, bool evaluate)
        {
            ArgumentCount = args;
            CalledDelegate = d;
            ReturnsType = (d.Method.ReturnType != null);
            Evaluate = evaluate;
            Function = null; // squelching error message about not all fields assigned 8/18/16
            Function = NativeFuncWrapper;
        }

        private object NativeFuncWrapper(StackData runtime)
        {
            runtime.AssertCount(ArgumentCount + 1); // plus 1 for the name

            object[] args = new object[runtime.ParameterCount - 1];
            ParameterInfo[] expectedArgs = CalledDelegate.Method.GetParameters();

            for (int index = 1; index < runtime.ParameterCount; ++index) // make sure the types are correct for each parameter
                args[index - 1] = runtime.ConvertAt(index, expectedArgs[index - 1].ParameterType);

            if (ReturnsType) {
                return CalledDelegate.DynamicInvoke(args);
            }
            else {
                CalledDelegate.DynamicInvoke(args);
                return null;
            }
        }
    }
}
