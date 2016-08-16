// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
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

        private object ArrayContains(FuncData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.GetAt<object[]>(1).Contains(stackFrame.GetAt(2));
        }

        private object ArrayIndexOf(FuncData stackFrame)
        {
            object[] arr = stackFrame.GetAt<object[]>(1);
            if (stackFrame.ParameterCount == 3) {
                stackFrame.Add(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.Add(arr.Length);
            }
            stackFrame.AssertCount(5);
            object o = stackFrame.GetAt(2);
            int i = stackFrame.GetAt<int>(3);
            int count = stackFrame.GetAt<int>(5);
            for (; i < arr.Length && i < count; i++) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }

        private object ArrayLastIndexOf(FuncData stackFrame)
        {
            object[] arr = stackFrame.GetAt<object[]>(1);
            if (stackFrame.ParameterCount == 3) {
                stackFrame.Add(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.Add(arr.Length);
            }
            stackFrame.AssertCount(5);
            int i = stackFrame.GetAt<int>(3);
            object o = stackFrame.GetAt(2);
            int count = stackFrame.GetAt<int>(5);
            for (; i >= 0 && i > count; i--) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }
    }
}