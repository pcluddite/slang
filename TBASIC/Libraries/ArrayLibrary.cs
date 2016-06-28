/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
using System.Linq;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    internal class ArrayLibrary : Library
    {
        public ArrayLibrary()
        {
            Add("ArrayContains", ArrayContains);
            Add("ArrayIndexOf", ArrayIndexOf);
            Add("ArrayLastIndexOf", ArrayLastIndexOf);
            //Add("ArrayResize", ArrayResize);
        }

        private void ArrayContains(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = stackFrame.GetParameter<object[]>(1).Contains(stackFrame.GetParameter(2));
        }

        private void ArrayIndexOf(TFunctionData stackFrame)
        {
            object[] arr = stackFrame.GetParameter<object[]>(1);
            if (stackFrame.ParameterCount == 3) {
                stackFrame.AddParameter(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.AddParameter(arr.Length);
            }
            stackFrame.AssertParamCount(5);
            object o = stackFrame.GetParameter(2);
            int i = stackFrame.GetParameter<int>(3);
            int count = stackFrame.GetParameter<int>(5);
            for (; i < arr.Length && i < count; i++) {
                if (arr[i] == o) {
                    stackFrame.Data = i;
                    return;
                }
            }
            stackFrame.Data = -1;
        }

        private void ArrayLastIndexOf(TFunctionData stackFrame)
        {
            object[] arr = stackFrame.GetParameter<object[]>(1);
            if (stackFrame.ParameterCount == 3) {
                stackFrame.AddParameter(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.AddParameter(arr.Length);
            }
            stackFrame.AssertParamCount(5);
            int i = stackFrame.GetParameter<int>(3);
            object o = stackFrame.GetParameter(2);
            int count = stackFrame.GetParameter<int>(5);
            for (; i >= 0 && i > count; i--) {
                if (arr[i] == o) {
                    stackFrame.Data = i;
                    return;
                }
            }
            stackFrame.Data = -1;
        }
    }
}