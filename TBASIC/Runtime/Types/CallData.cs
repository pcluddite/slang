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
    public delegate object TBasicFunction(FuncData stack);

    internal struct CallData
    {
        public int ArgumentCount { get; set; }
        public TBasicFunction Function { get; set; }
        public Delegate CalledDelegate { get; set; }

        public CallData(TBasicFunction func)
        {
            Function = func;
            CalledDelegate = null;
            ArgumentCount = -1;
        }

        public CallData(Delegate d)
        {
            ArgumentCount = GetArgCount(d);
            CalledDelegate = d;
            Function = null; // squelching error message about not all fields assigned 8/18/16
            Function = FuncWrapper;
        }

        private static int GetArgCount(Delegate d)
        {
            if (!typeof(IConvertible).IsAssignableFrom(d.Method.ReturnType) && d.Method.ReturnType != typeof(object)) {
                throw new ArgumentException("Delegate must have an IConvertible return type");
            }
            ParameterInfo[] info = d.Method.GetParameters();
            for (int index = 0; index < info.Length; ++index) {
                if (!typeof(IConvertible).IsAssignableFrom(info[index].ParameterType)) {
                    throw new ArgumentException(string.Format("{0} cannot be {1} because {1} does not implement IConvertible", info[index].Name, info[index].ParameterType.Name));
                }
            }
            return info.Length;
        }

        private object FuncWrapper(FuncData fData)
        {
            fData.AssertCount(ArgumentCount + 1); // plus 1 for the name

            object[] args = new object[fData.ParameterCount - 1];
            ParameterInfo[] expectedArgs = CalledDelegate.Method.GetParameters();

            for (int index = 1; index < fData.ParameterCount; ++index) // make sure the types are correct for each parameter
                args[index - 1] = fData.ConvertAt(index, expectedArgs[index - 1].ParameterType);
            try {
                return CalledDelegate.DynamicInvoke(args);
            }
            catch (ArgumentException ex) {
                //if (ex.TargetSite.)
                throw;
            }
        }
    }
}
