/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using Tbasic.Errors;
using Tbasic.Runtime;
using Tbasic.Types;

namespace Tbasic.Libraries
{
    internal class RuntimeLibrary : Library
    {
        public RuntimeLibrary(Scope context)
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
            context.AddConstant("@version", Executor.VERSION);
            context.AddConstant("@osversion", Environment.OSVersion.VersionString);
        }
        
        private object CStr(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get(1)?.ToString(); // return null if it is null
        }

        private object CBool(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            try {
                bool b;
                if (bool.TryParse(stackdat.Get<string>(1), out b)) {
                    return b;
                }
                Number n;
                if (Number.TryParse(stackdat.Get<string>(1), out n)) {
                    return (n != 0); // non-zero is true, zero is false
                }
                throw new InvalidCastException();
            }
            catch(InvalidCastException) {
                return Convert.ToBoolean(stackdat.Get(1));
            }
        }

        private object CNum(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            try {
                Number n;
                if (Number.TryParse(stackdat.Get<string>(1), out n)) {
                    return n;
                }
                bool b;
                if (bool.TryParse(stackdat.Get<string>(1), out b)) {
                    return b ? 1 : 0;
                }
                throw new InvalidCastException();
            }
            catch (InvalidCastException) {
                return Number.Convert(stackdat.Get(1), runtime.Options);
            }
        }

        private object SizeOf(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            object obj = stackdat.Get(1);
            if (obj == null) {
                return 0;
            }
            else if (obj is string) {
                return obj.ToString().Length;
            }
            else if (obj is Number) {
                return sizeof(double);
            }
            else if (obj is bool) {
                return sizeof(bool);
            }
            else if (obj.GetType().IsArray) {
                return ((object[])obj).Length;
            }
            else if (obj is int) {
                return sizeof(int);
            }
            else if (obj is long) {
                return sizeof(long);
            }
            else if (obj is byte) {
                return sizeof(byte);
            }
            else {
                throw new FunctionException(ErrorClient.Forbidden, "Object size cannot be determined");
            }
        }

        private object IsNum(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return Number.IsNumber(stackdat.Get(1), runtime.Options);
        }

        private object IsString(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get(1) is string;
        }

        private object IsBool(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.Get(1) is bool;
        }
        
        private object IsDefined(Executor runtime, StackFrame stackdat)
        {
            stackdat.AssertCount(2);
            string name = stackdat.Get<string>(1);
            Scope context = runtime.Context.FindContext(name);
            return context != null;
        }
    }
}