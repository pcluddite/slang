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
        
        private object RegValueKind(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            return WinRegistry.GetValueKind(runtime.GetAt<string>(1), runtime.GetAt<string>(2)).ToString();
        }

        private object RegRead(RuntimeData runtime)
        {
            runtime.AssertCount(atLeast: 3, atMost: 4);

            if (runtime.ParameterCount == 3)
                runtime.Add(null);
            
            return WinRegistry.Read(runtime.GetAt<string>(1), runtime.GetAt<string>(2), runtime.GetAt<string>(3));
        }

        private object RegDelete(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            WinRegistry.Delete(runtime.GetAt<string>(1), runtime.GetAt<string>(2));
            return null;
        }

        private object RegRename(RuntimeData runtime)
        {
            runtime.AssertCount(4);
            WinRegistry.Rename(runtime.GetAt<string>(1), runtime.GetAt<string>(2), runtime.GetAt<string>(3));
            return null;
        }

        private object RegDeleteKey(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            WinRegistry.DeleteKey(runtime.GetAt<string>(1));
            return null;
        }

        private object RegRenameKey(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            WinRegistry.RenameKey(runtime.GetAt<string>(1), runtime.GetAt<string>(2));
            return null;
        }

        private object RegCreateKey(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            WinRegistry.RenameKey(runtime.GetAt<string>(1), runtime.GetAt<string>(2));
            runtime.Status = ErrorSuccess.Created;
            return null;
        }
        
        private object RegEnumValues(RuntimeData runtime)
        {
            runtime.AssertCount(2);

            object[][] values = WinRegistry.EnumerateValues(runtime.GetAt<string>(1));
            if (values.Length == 0) {
                runtime.Status = ErrorSuccess.NoContent;
                return null;
            }
            else {
                return values;
            }
        }

        private static object RegEnumKeys(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return WinRegistry.EnumeratKeys(runtime.GetAt<string>(1));
        }

        private object RegWrite(RuntimeData runtime)
        {
            runtime.AssertCount(5);

            object value = runtime.GetAt(3);
            RegistryValueKind kind = runtime.GetAt<RegistryValueKind>(4);

            switch (kind) {
                case RegistryValueKind.Binary:
                    value = Convert.FromBase64String(runtime.GetAt<string>(3));
                    break;
                case RegistryValueKind.MultiString:
                    string strval = value as string;
                    if (value is string) {
                        value = runtime.GetAt<string>(3).Replace("\r\n", "\n").Split('\n');
                    }
                    else if (value is string[]) {
                        value = runtime.GetAt<string[]>(3);
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
            WinRegistry.Write(runtime.GetAt<string>(1), runtime.GetAt<string>(2), value, kind);
            return null;
        }
    }
}