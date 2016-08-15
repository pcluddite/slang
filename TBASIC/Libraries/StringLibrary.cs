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

        private object CharsToString(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            StringBuilder hanz = new StringBuilder();
            foreach (char c in stackFrame.GetParameter<char[]>(1)) {
                hanz.Append(c);
            }
            return hanz.ToString();
        }

        private object ToCharArray(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter<string>(1).ToCharArray();
        }

        private object StringSplit(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(3);
            return Regex.Split(stackFrame.GetParameter(1).ToString(), stackFrame.GetParameter(2).ToString());
        }

        private object Trim(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter(1).ToString().Trim();
        }

        private object TrimStart(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter(1).ToString().TrimStart();
        }

        private object TrimEnd(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter(1).ToString().TrimEnd();
        }

        private object StringContains(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(3);
            return stackFrame.GetParameter<string>(1).Contains(stackFrame.GetParameter<string>(2));
        }

        private object StringCompare(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(3);
            return stackFrame.GetParameter<string>(1).CompareTo(stackFrame.GetParameter<string>(2));
        }

        private object StringIndexOf(TFunctionData stackFrame)
        {
            if (stackFrame.ParameterCount == 3) {
                stackFrame.AddParameter(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.AddParameter(stackFrame.GetParameter<string>(1).Length);
            }
            stackFrame.AssertParamCount(5);
            char? cObj = stackFrame.GetParameter(2) as char?;
            if (cObj == null) {
                return stackFrame.GetParameter<string>(1).IndexOf(stackFrame.GetParameter<string>(2), stackFrame.GetParameter<int>(3), stackFrame.GetParameter<int>(4));
            }
            else {
                return stackFrame.GetParameter<string>(1).IndexOf(cObj.Value, stackFrame.GetParameter<int>(3), stackFrame.GetParameter<int>(4));
            }
        }

        private object StringLastIndexOf(TFunctionData stackFrame)
        {
            if (stackFrame.ParameterCount == 3) {
                stackFrame.AddParameter(0);
            }
            if (stackFrame.ParameterCount == 4) {
                stackFrame.AddParameter(stackFrame.GetParameter<string>(1).Length);
            }
            stackFrame.AssertParamCount(5);
            char? cObj = stackFrame.GetParameter(2) as char?;
            if (cObj == null) {
                return stackFrame.GetParameter<string>(1).LastIndexOf(stackFrame.GetParameter<string>(2), stackFrame.GetParameter<int>(3), stackFrame.GetParameter<int>(4));
            }
            else {
                return stackFrame.GetParameter<string>(1).LastIndexOf(cObj.Value, stackFrame.GetParameter<int>(3), stackFrame.GetParameter<int>(4));
            }
        }

        private object StringUpper(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter<string>(1).ToUpper();
        }

        private object StringLower(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter<string>(1).ToLower();
        }

        private object StringLeft(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(3);
            return stackFrame.GetParameter<string>(1).Substring(stackFrame.GetParameter<int>(2));
        }

        private object StringRight(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(3);
            return stackFrame.GetParameter<string>(1).Remove(stackFrame.GetParameter<int>(2));
        }

        private object Substring(TFunctionData stackFrame)
        {
            if (stackFrame.ParameterCount == 3) {
                return stackFrame.GetParameter<string>(1).Substring(
                                    stackFrame.GetParameter<int>(2)
                                    );
            }
            else {
                stackFrame.AssertParamCount(4);
                return stackFrame.GetParameter<string>(1).Substring(
                                    stackFrame.GetParameter<int>(2), stackFrame.GetParameter<int>(3)
                                    );
            }
        }
    }
}