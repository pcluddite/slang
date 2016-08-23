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

        private object CStr(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt(1)?.ToString(); // return null if it is null
        }

        private object CBool(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            try {
                bool b;
                if (bool.TryParse(runtime.GetAt<string>(1), out b)) {
                    return b;
                }
                Number n;
                if (Number.TryParse(runtime.GetAt<string>(1), out n)) {
                    return (n != 0); // non-zero is true, zero is false
                }
                throw new InvalidCastException();
            }
            catch(InvalidCastException) {
                return Convert.ToBoolean(runtime.GetAt(1));
            }
        }

        private object CNum(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            try {
                Number n;
                if (Number.TryParse(runtime.GetAt<string>(1), out n)) {
                    return n;
                }
                bool b;
                if (bool.TryParse(runtime.GetAt<string>(1), out b)) {
                    return b ? 1 : 0;
                }
                throw new InvalidCastException();
            }
            catch (InvalidCastException) {
                return Number.Convert(runtime.GetAt(1));
            }
        }

        private object SizeOf(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            object obj = runtime.GetAt(1);
            if (obj == null) {
                return 0;
            }
            else if (obj is string) {
                return obj.ToString().Length;
            }
            else if (obj is Number) {
                return Number.SIZE;
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

        private object IsNum(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return Number.IsNumber(runtime.GetAt(1), runtime.StackExecuter.Options);
        }

        private object IsString(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt(1) is string;
        }

        private object IsBool(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return runtime.GetAt(1) is bool;
        }
        
        private object IsDefined(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            string name = runtime.GetAt<string>(1);
            ObjectContext context = runtime.Context.FindContext(name);
            return context != null;
        }
    }
}