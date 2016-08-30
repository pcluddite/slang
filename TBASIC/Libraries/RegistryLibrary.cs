// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    /// <summary>
    /// Library for interacting with Windows registry.
    /// </summary>
    internal class RegistryLibrary : Library
    {
        public RegistryLibrary()
        {
            Add("RegEnumKeys", RegEnumKeys);
            Add("RegEnumValues", RegEnumValues);
            Add("RegRenameKey", RegRenameKey);
            Add("RegRename", RegRename);
            Add("RegDelete", RegDelete);
            Add("RegDeleteKey", RegDeleteKey);
            Add("RegCreateKey", RegCreateKey);
            Add("RegRead", RegRead);
            Add("RegWrite", RegWrite);
        }
        
        private object RegValueKind(StackData stackdat)
        {
            stackdat.AssertCount(3);
            return WinRegistry.GetValueKind(stackdat.GetAt<string>(1), stackdat.GetAt<string>(2)).ToString();
        }

        private object RegRead(StackData stackdat)
        {
            stackdat.AssertCount(atLeast: 3, atMost: 4);

            if (stackdat.ParameterCount == 3)
                stackdat.Add(null);
            
            return WinRegistry.Read(stackdat.GetAt<string>(1), stackdat.GetAt<string>(2), stackdat.GetAt<string>(3));
        }

        private object RegDelete(StackData stackdat)
        {
            stackdat.AssertCount(3);
            WinRegistry.Delete(stackdat.GetAt<string>(1), stackdat.GetAt<string>(2));
            return null;
        }

        private object RegRename(StackData stackdat)
        {
            stackdat.AssertCount(4);
            WinRegistry.Rename(stackdat.GetAt<string>(1), stackdat.GetAt<string>(2), stackdat.GetAt<string>(3));
            return null;
        }

        private object RegDeleteKey(StackData stackdat)
        {
            stackdat.AssertCount(2);
            WinRegistry.DeleteKey(stackdat.GetAt<string>(1));
            return null;
        }

        private object RegRenameKey(StackData stackdat)
        {
            stackdat.AssertCount(3);
            WinRegistry.RenameKey(stackdat.GetAt<string>(1), stackdat.GetAt<string>(2));
            return null;
        }

        private object RegCreateKey(StackData stackdat)
        {
            stackdat.AssertCount(3);
            WinRegistry.RenameKey(stackdat.GetAt<string>(1), stackdat.GetAt<string>(2));
            stackdat.Status = ErrorSuccess.Created;
            return null;
        }
        
        private object RegEnumValues(StackData stackdat)
        {
            stackdat.AssertCount(2);

            object[][] values = WinRegistry.EnumerateValues(stackdat.GetAt<string>(1));
            if (values.Length == 0) {
                stackdat.Status = ErrorSuccess.NoContent;
                return null;
            }
            else {
                return values;
            }
        }

        private static object RegEnumKeys(StackData stackdat)
        {
            stackdat.AssertCount(2);
            return WinRegistry.EnumeratKeys(stackdat.GetAt<string>(1));
        }

        private object RegWrite(StackData stackdat)
        {
            stackdat.AssertCount(5);

            object value = stackdat.GetAt(3);
            RegistryValueKind kind = stackdat.GetAt<RegistryValueKind>(4);

            switch (kind) {
                case RegistryValueKind.Binary:
                    value = Convert.FromBase64String(stackdat.GetAt<string>(3));
                    break;
                case RegistryValueKind.MultiString:
                    string strval = value as string;
                    if (value is string) {
                        value = stackdat.GetAt<string>(3).Replace("\r\n", "\n").Split('\n');
                    }
                    else if (value is string[]) {
                        value = stackdat.GetAt<string[]>(3);
                    }
                    else {
                        throw new ArgumentException("Parameter is not a valid multi-string");
                    }
                    break;
                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                case RegistryValueKind.String:
                    // natively supported
                    break;
                default:
                    throw new ArgumentException("Registry value of type '" + kind + "' is unsupported");
            }
            WinRegistry.Write(stackdat.GetAt<string>(1), stackdat.GetAt<string>(2), value, kind);
            return null;
        }
    }
}