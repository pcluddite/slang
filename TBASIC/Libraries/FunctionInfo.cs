// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Reflection;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    /// <summary>
    /// Delegate for processing a TBasic function
    /// </summary>
    /// <param name="stack">The object containing parameter and execution information</param>
    public delegate object TBasicFunction(RuntimeData stack);

    internal struct FunctionInfo
    {
        public int ArgumentCount { get; set; }
        public TBasicFunction Function { get; set; }
        public Delegate CalledDelegate { get; set; }
        public bool ReturnsType { get; private set; }

        public FunctionInfo(TBasicFunction func)
        {
            Function = func;
            CalledDelegate = null;
            ArgumentCount = -1;
            ReturnsType = true;
        }

        public FunctionInfo(Delegate d, int args)
        {
            ArgumentCount = args;
            CalledDelegate = d;
            ReturnsType = (d.Method.ReturnType != null);
            Function = null; // squelching error message about not all fields assigned 8/18/16
            Function = NativeFuncWrapper;
        }

        private object NativeFuncWrapper(RuntimeData fData)
        {
            fData.AssertCount(ArgumentCount + 1); // plus 1 for the name

            object[] args = new object[fData.ParameterCount - 1];
            ParameterInfo[] expectedArgs = CalledDelegate.Method.GetParameters();

            for (int index = 1; index < fData.ParameterCount; ++index) // make sure the types are correct for each parameter
                args[index - 1] = fData.ConvertAt(index, expectedArgs[index - 1].ParameterType);

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
