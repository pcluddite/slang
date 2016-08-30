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

        private object ArrayContains(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt<object[]>(1).Contains(stackdat.GetAt(2));
        }

        private object ArrayIndexOf(StackData stackdat)
        {
            object[] arr = stackdat.GetAt<object[]>(1);
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(arr.Length);
            }
            stackdat.AssertCount(5);
            object o = stackdat.GetAt(2);
            int i = stackdat.GetAt<int>(3);
            int count = stackdat.GetAt<int>(5);
            for (; i < arr.Length && i < count; i++) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }

        private object ArrayLastIndexOf(StackData stackdat)
        {
            object[] arr = stackdat.GetAt<object[]>(1);
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(arr.Length);
            }
            stackdat.AssertCount(5);
            int i = stackdat.GetAt<int>(3);
            object o = stackdat.GetAt(2);
            int count = stackdat.GetAt<int>(5);
            for (; i >= 0 && i > count; i--) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }
    }
}