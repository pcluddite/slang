﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Linq;
using Tbasic.Errors;
using Tbasic.Runtime;
using Tbasic.Types;

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
            context.SetConstant("@version", TBasic.VERSION);
            context.SetConstant("@osversion", Environment.OSVersion.VersionString);
        }
        
        private object CStr(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt(1)?.ToString(); // return null if it is null
        }

        private object CBool(StackData stackdat)
        {
            stackdat.AssertCount(2);
            try {
                bool b;
                if (bool.TryParse(stackdat.GetAt<string>(1), out b)) {
                    return b;
                }
                Number n;
                if (Number.TryParse(stackdat.GetAt<string>(1), out n)) {
                    return (n != 0); // non-zero is true, zero is false
                }
                throw new InvalidCastException();
            }
            catch(InvalidCastException) {
                return Convert.ToBoolean(stackdat.GetAt(1));
            }
        }

        private object CNum(StackData stackdat)
        {
            stackdat.AssertCount(2);
            try {
                Number n;
                if (Number.TryParse(stackdat.GetAt<string>(1), out n)) {
                    return n;
                }
                bool b;
                if (bool.TryParse(stackdat.GetAt<string>(1), out b)) {
                    return b ? 1 : 0;
                }
                throw new InvalidCastException();
            }
            catch (InvalidCastException) {
                return Number.Convert(stackdat.GetAt(1));
            }
        }

        private object SizeOf(StackData stackdat)
        {
            stackdat.AssertCount(2);
            object obj = stackdat.GetAt(1);
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

        private object IsNum(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return Number.IsNumber(stackdat.GetAt(1), stackdat.Runtime.Options);
        }

        private object IsString(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt(1) is string;
        }

        private object IsBool(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return stackdat.GetAt(1) is bool;
        }
        
        private object IsDefined(StackData stackdat)
        {
            stackdat.AssertCount(2);
            string name = stackdat.GetAt<string>(1);
            ObjectContext context = stackdat.Context.FindContext(name);
            return context != null;
        }
    }
}