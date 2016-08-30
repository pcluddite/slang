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

        private object CharsToString(StackData stackdat)
        {
            stackdat.AssertCount(2);
            StringBuilder hanz = new StringBuilder();
            foreach (char c in stackdat.GetAt<char[]>(1)) {
                hanz.Append(c);
            }
            return hanz.ToString();
        }

        private object ToCharArray(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt<string>(1).ToCharArray();
        }

        private object StringSplit(StackData stackdat)
        {
            stackdat.AssertCount(3);
            return Regex.Split(stackdat.GetAt(1).ToString(), stackdat.GetAt(2).ToString());
        }

        private object Trim(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt(1).ToString().Trim();
        }

        private object TrimStart(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt(1).ToString().TrimStart();
        }

        private object TrimEnd(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt(1).ToString().TrimEnd();
        }

        private object StringContains(StackData stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.GetAt<string>(1).Contains(stackdat.GetAt<string>(2));
        }

        private object StringCompare(StackData stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.GetAt<string>(1).CompareTo(stackdat.GetAt<string>(2));
        }

        private object StringIndexOf(StackData stackdat)
        {
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(stackdat.GetAt<string>(1).Length);
            }
            stackdat.AssertCount(5);
            char? cObj = stackdat.GetAt(2) as char?;
            if (cObj == null) {
                return stackdat.GetAt<string>(1).IndexOf(stackdat.GetAt<string>(2), stackdat.GetAt<int>(3), stackdat.GetAt<int>(4));
            }
            else {
                return stackdat.GetAt<string>(1).IndexOf(cObj.Value, stackdat.GetAt<int>(3), stackdat.GetAt<int>(4));
            }
        }

        private object StringLastIndexOf(StackData stackdat)
        {
            if (stackdat.ParameterCount == 3) {
                stackdat.Add(0);
            }
            if (stackdat.ParameterCount == 4) {
                stackdat.Add(stackdat.GetAt<string>(1).Length);
            }
            stackdat.AssertCount(5);
            char? cObj = stackdat.GetAt(2) as char?;
            if (cObj == null) {
                return stackdat.GetAt<string>(1).LastIndexOf(stackdat.GetAt<string>(2), stackdat.GetAt<int>(3), stackdat.GetAt<int>(4));
            }
            else {
                return stackdat.GetAt<string>(1).LastIndexOf(cObj.Value, stackdat.GetAt<int>(3), stackdat.GetAt<int>(4));
            }
        }

        private object StringUpper(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt<string>(1).ToUpper();
        }

        private object StringLower(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt<string>(1).ToLower();
        }

        private object StringLeft(StackData stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.GetAt<string>(1).Substring(stackdat.GetAt<int>(2));
        }

        private object StringRight(StackData stackdat)
        {
            stackdat.AssertCount(3);
            return stackdat.GetAt<string>(1).Remove(stackdat.GetAt<int>(2));
        }

        private object Substring(StackData stackdat)
        {
            if (stackdat.ParameterCount == 3) {
                return stackdat.GetAt<string>(1).Substring(
                                    stackdat.GetAt<int>(2)
                                    );
            }
            else {
                stackdat.AssertCount(4);
                return stackdat.GetAt<string>(1).Substring(
                                    stackdat.GetAt<int>(2), stackdat.GetAt<int>(3)
                                    );
            }
        }
    }
}