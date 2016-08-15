// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Runtime;
using Tbasic.Errors;

namespace Tbasic.Libraries
{
    internal class RuntimeLibrary : Library
    {
        public RuntimeLibrary(ObjectContext context)
        {
            Add("SizeOf", SizeOf);
            Add("Len", SizeOf);
            Add("IsStr", IsString);
            Add("IsNum", IsNum);
            Add("IsBool", IsBool);
            Add("IsDefined", IsDefined);
            Add("CStr", CStr);
            Add("CNum", CNum);
            Add("CBool", CBool);
            AddLibrary(new StringLibrary());
            AddLibrary(new ArrayLibrary());
            context.SetConstant("@version", Executer.VERSION);
            context.SetConstant("@osversion", Environment.OSVersion.VersionString);
        }

        private object CStr(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter(1)?.ToString(); // return null if it is null
        }

        private object CBool(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            try {
                bool b;
                if (bool.TryParse(stackFrame.GetParameter<string>(1), out b)) {
                    return b;
                }
                Number n;
                if (Number.TryParse(stackFrame.GetParameter<string>(1), out n)) {
                    return (n != 0); // non-zero is true, zero is false
                }
                throw new InvalidCastException();
            }
            catch(InvalidCastException) {
                return Convert.ToBoolean(stackFrame.GetParameter(1));
            }
        }

        private object CNum(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            try {
                Number n;
                if (Number.TryParse(stackFrame.GetParameter<string>(1), out n)) {
                    return n;
                }
                bool b;
                if (bool.TryParse(stackFrame.GetParameter<string>(1), out b)) {
                    return b ? 1 : 0;
                }
                throw new InvalidCastException();
            }
            catch (InvalidCastException) {
                return Number.Convert(stackFrame.GetParameter(1));
            }
        }

        private object SizeOf(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            object obj = stackFrame.GetParameter(1);
            if (obj == null) {
                return 0;
            }
            else if (obj is string) {
                return obj.ToString().Length;
            }
            else if (Number.IsNumber(obj)) {
                return Number.SIZE;
            }
            else if (obj is bool) {
                return sizeof(bool);
            }
            else if (obj.GetType().IsArray) {
                return ((object[])obj).Length;
            }
            else {
                throw new TbasicException(ErrorClient.Forbidden, "Object size cannot be determined");
            }
        }

        private object IsNum(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return Number.IsNumber(stackFrame.GetParameter(1));
        }

        private object IsString(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter(1) is string;
        }

        private object IsBool(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            return stackFrame.GetParameter(1) is bool;
        }
        
        private object IsDefined(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            string name = stackFrame.GetParameter<string>(1);
            ObjectContext context = stackFrame.Context.FindContext(name);
            return context != null;
        }
    }
}