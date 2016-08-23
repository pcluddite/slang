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

        private object ArrayContains(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt<object[]>(1).Contains(runtime.GetAt(2));
        }

        private object ArrayIndexOf(RuntimeData runtime)
        {
            object[] arr = runtime.GetAt<object[]>(1);
            if (runtime.ParameterCount == 3) {
                runtime.Add(0);
            }
            if (runtime.ParameterCount == 4) {
                runtime.Add(arr.Length);
            }
            runtime.AssertCount(5);
            object o = runtime.GetAt(2);
            int i = runtime.GetAt<int>(3);
            int count = runtime.GetAt<int>(5);
            for (; i < arr.Length && i < count; i++) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }

        private object ArrayLastIndexOf(RuntimeData runtime)
        {
            object[] arr = runtime.GetAt<object[]>(1);
            if (runtime.ParameterCount == 3) {
                runtime.Add(0);
            }
            if (runtime.ParameterCount == 4) {
                runtime.Add(arr.Length);
            }
            runtime.AssertCount(5);
            int i = runtime.GetAt<int>(3);
            object o = runtime.GetAt(2);
            int count = runtime.GetAt<int>(5);
            for (; i >= 0 && i > count; i--) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }
    }
}