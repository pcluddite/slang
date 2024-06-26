﻿/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System.Linq;
using Slang.Runtime;

namespace Slang.Libraries
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

        private object ArrayContains(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get<object[]>(1).Contains(stackdat.Get(2));
        }

        private object ArrayIndexOf(Executor runtime, StackFrame stackdat)
        {
            object[] arr = stackdat.Get<object[]>(1);
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(arr.Length);
            }
            stackdat.AssertCount(5);
            object o = stackdat.Get(2);
            int i = stackdat.Get<int>(3);
            int count = stackdat.Get<int>(5);
            for (; i < arr.Length && i < count; i++) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }

        private object ArrayLastIndexOf(Executor runtime, StackFrame stackdat)
        {
            object[] arr = stackdat.Get<object[]>(1);
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(arr.Length);
            }
            stackdat.AssertCount(5);
            int i = stackdat.Get<int>(3);
            object o = stackdat.Get(2);
            int count = stackdat.Get<int>(5);
            for (; i >= 0 && i > count; i--) {
                if (arr[i] == o) {
                    return i;
                }
            }
            return -1;
        }
    }
}