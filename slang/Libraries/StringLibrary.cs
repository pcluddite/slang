﻿/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System.Text;
using System.Text.RegularExpressions;
using Slang.Runtime;

namespace Slang.Libraries
{
    internal class StringLibrary : Library
    {
        public StringLibrary()
        {
            Add("StrContains", StringContains);
            Add("StrIndexOf", StringIndexOf);
            Add("StrLastIndexOf", StringLastIndexOf);
            Add("StrUpper", StringUpper);
            Add("StrCompare", StringCompare);
            Add("StrLower", StringLower);
            Add("StrLeft", StringLeft);
            Add("StrRight", StringRight);
            Add("StrTrim", Trim);
            Add("StrTrimStart", TrimStart);
            Add("StrTrimEnd", TrimEnd);
            Add("StrSplit", StringSplit);
            Add("StrToChars", ToCharArray);
            Add("CharsToStr", CharsToString);
            Add("StrInStr", Substring);
        }

        private object CharsToString(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            StringBuilder hanz = new StringBuilder();
            foreach (char c in stackdat.Get<char[]>(1)) {
                hanz.Append(c);
            }
            return hanz.ToString();
        }

        private object ToCharArray(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get<string>(1).ToCharArray();
        }

        private object StringSplit(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(3);
            return Regex.Split(stackdat.Get(1).ToString(), stackdat.Get(2).ToString());
        }

        private object Trim(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get(1).ToString().Trim();
        }

        private object TrimStart(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get(1).ToString().TrimStart();
        }

        private object TrimEnd(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get(1).ToString().TrimEnd();
        }

        private object StringContains(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.Get<string>(1).Contains(stackdat.Get<string>(2));
        }

        private object StringCompare(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.Get<string>(1).CompareTo(stackdat.Get<string>(2));
        }

        private object StringIndexOf(Executor runtime, StackFrame stackdat)
        {
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(stackdat.Get<string>(1).Length);
            }
            stackdat.AssertCount(5);
            return stackdat.Get<string>(1).IndexOf(stackdat.Get<string>(2), stackdat.Get<int>(3), stackdat.Get<int>(4));
        }

        private object StringLastIndexOf(Executor runtime, StackFrame stackdat)
        {
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(stackdat.Get<string>(1).Length);
            }
            stackdat.AssertCount(5);
            return stackdat.Get<string>(1).LastIndexOf(stackdat.Get<string>(2), stackdat.Get<int>(3), stackdat.Get<int>(4));
        }

        private object StringUpper(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get<string>(1).ToUpper();
        }

        private object StringLower(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get<string>(1).ToLower();
        }

        private object StringLeft(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.Get<string>(1).Substring(stackdat.Get<int>(2));
        }

        private object StringRight(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.Get<string>(1).Remove(stackdat.Get<int>(2));
        }

        private object Substring(Executor runtime, StackFrame stackdat)
        {
            if (stackdat.ParameterCount == 3) {
                return stackdat.Get<string>(1).Substring(
                                    stackdat.Get<int>(2)
                                    );
            }
            else {
                stackdat.AssertCount(4);
                return stackdat.Get<string>(1).Substring(
                                    stackdat.Get<int>(2), stackdat.Get<int>(3)
                                    );
            }
        }
    }
}