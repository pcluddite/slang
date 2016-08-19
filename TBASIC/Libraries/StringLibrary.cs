// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Text;
using System.Text.RegularExpressions;
using Tbasic.Runtime;

namespace Tbasic.Libraries
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

        private object CharsToString(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            StringBuilder hanz = new StringBuilder();
            foreach (char c in stackFrame.GetAt<char[]>(1)) {
                hanz.Append(c);
            }
            return hanz.ToString();
        }

        private object ToCharArray(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.GetAt<string>(1).ToCharArray();
        }

        private object StringSplit(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(3);
            return Regex.Split(stackFrame.GetAt(1).ToString(), stackFrame.GetAt(2).ToString());
        }

        private object Trim(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.GetAt(1).ToString().Trim();
        }

        private object TrimStart(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.GetAt(1).ToString().TrimStart();
        }

        private object TrimEnd(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.GetAt(1).ToString().TrimEnd();
        }

        private object StringContains(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(3);
            return stackFrame.GetAt<string>(1).Contains(stackFrame.GetAt<string>(2));
        }

        private object StringCompare(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(3);
            return stackFrame.GetAt<string>(1).CompareTo(stackFrame.GetAt<string>(2));
        }

        private object StringIndexOf(RuntimeData stackFrame)
        {
            if (stackFrame.ParameterCount == 3) {
                stackFrame.Add(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.Add(stackFrame.GetAt<string>(1).Length);
            }
            stackFrame.AssertCount(5);
            char? cObj = stackFrame.GetAt(2) as char?;
            if (cObj == null) {
                return stackFrame.GetAt<string>(1).IndexOf(stackFrame.GetAt<string>(2), stackFrame.GetAt<int>(3), stackFrame.GetAt<int>(4));
            }
            else {
                return stackFrame.GetAt<string>(1).IndexOf(cObj.Value, stackFrame.GetAt<int>(3), stackFrame.GetAt<int>(4));
            }
        }

        private object StringLastIndexOf(RuntimeData stackFrame)
        {
            if (stackFrame.ParameterCount == 3) {
                stackFrame.Add(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.Add(stackFrame.GetAt<string>(1).Length);
            }
            stackFrame.AssertCount(5);
            char? cObj = stackFrame.GetAt(2) as char?;
            if (cObj == null) {
                return stackFrame.GetAt<string>(1).LastIndexOf(stackFrame.GetAt<string>(2), stackFrame.GetAt<int>(3), stackFrame.GetAt<int>(4));
            }
            else {
                return stackFrame.GetAt<string>(1).LastIndexOf(cObj.Value, stackFrame.GetAt<int>(3), stackFrame.GetAt<int>(4));
            }
        }

        private object StringUpper(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.GetAt<string>(1).ToUpper();
        }

        private object StringLower(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(2);
            return stackFrame.GetAt<string>(1).ToLower();
        }

        private object StringLeft(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(3);
            return stackFrame.GetAt<string>(1).Substring(stackFrame.GetAt<int>(2));
        }

        private object StringRight(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(3);
            return stackFrame.GetAt<string>(1).Remove(stackFrame.GetAt<int>(2));
        }

        private object Substring(RuntimeData stackFrame)
        {
            if (stackFrame.ParameterCount == 3) {
                return stackFrame.GetAt<string>(1).Substring(
                                    stackFrame.GetAt<int>(2)
                                    );
            }
            else {
                stackFrame.AssertCount(4);
                return stackFrame.GetAt<string>(1).Substring(
                                    stackFrame.GetAt<int>(2), stackFrame.GetAt<int>(3)
                                    );
            }
        }
    }
}