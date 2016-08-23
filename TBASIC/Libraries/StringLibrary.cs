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

        private object CharsToString(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            StringBuilder hanz = new StringBuilder();
            foreach (char c in runtime.GetAt<char[]>(1)) {
                hanz.Append(c);
            }
            return hanz.ToString();
        }

        private object ToCharArray(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt<string>(1).ToCharArray();
        }

        private object StringSplit(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            return Regex.Split(runtime.GetAt(1).ToString(), runtime.GetAt(2).ToString());
        }

        private object Trim(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt(1).ToString().Trim();
        }

        private object TrimStart(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt(1).ToString().TrimStart();
        }

        private object TrimEnd(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt(1).ToString().TrimEnd();
        }

        private object StringContains(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            return runtime.GetAt<string>(1).Contains(runtime.GetAt<string>(2));
        }

        private object StringCompare(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            return runtime.GetAt<string>(1).CompareTo(runtime.GetAt<string>(2));
        }

        private object StringIndexOf(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 3) {
                runtime.Add(0);
            }
            if (runtime.ParameterCount == 4) {
                runtime.Add(runtime.GetAt<string>(1).Length);
            }
            runtime.AssertCount(5);
            char? cObj = runtime.GetAt(2) as char?;
            if (cObj == null) {
                return runtime.GetAt<string>(1).IndexOf(runtime.GetAt<string>(2), runtime.GetAt<int>(3), runtime.GetAt<int>(4));
            }
            else {
                return runtime.GetAt<string>(1).IndexOf(cObj.Value, runtime.GetAt<int>(3), runtime.GetAt<int>(4));
            }
        }

        private object StringLastIndexOf(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 3) {
                runtime.Add(0);
            }
            if (runtime.ParameterCount == 4) {
                runtime.Add(runtime.GetAt<string>(1).Length);
            }
            runtime.AssertCount(5);
            char? cObj = runtime.GetAt(2) as char?;
            if (cObj == null) {
                return runtime.GetAt<string>(1).LastIndexOf(runtime.GetAt<string>(2), runtime.GetAt<int>(3), runtime.GetAt<int>(4));
            }
            else {
                return runtime.GetAt<string>(1).LastIndexOf(cObj.Value, runtime.GetAt<int>(3), runtime.GetAt<int>(4));
            }
        }

        private object StringUpper(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt<string>(1).ToUpper();
        }

        private object StringLower(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt<string>(1).ToLower();
        }

        private object StringLeft(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            return runtime.GetAt<string>(1).Substring(runtime.GetAt<int>(2));
        }

        private object StringRight(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            return runtime.GetAt<string>(1).Remove(runtime.GetAt<int>(2));
        }

        private object Substring(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 3) {
                return runtime.GetAt<string>(1).Substring(
                                    runtime.GetAt<int>(2)
                                    );
            }
            else {
                runtime.AssertCount(4);
                return runtime.GetAt<string>(1).Substring(
                                    runtime.GetAt<int>(2), runtime.GetAt<int>(3)
                                    );
            }
        }
    }
}