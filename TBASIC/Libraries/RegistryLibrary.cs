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
        
        private object RegValueKind(FuncData _sframe)
        {
            _sframe.AssertCount(3);
            return WinRegistry.GetValueKind(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2)).ToString();
        }

        private object RegRead(FuncData _sframe)
        {
            _sframe.AssertCount(atLeast: 3, atMost: 4);

            if (_sframe.ParameterCount == 3)
                _sframe.Add(null);
            
            return WinRegistry.Read(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2), _sframe.GetAt<string>(3));
        }

        private object RegDelete(FuncData _sframe)
        {
            _sframe.AssertCount(3);
            WinRegistry.Delete(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2));
            return null;
        }

        private object RegRename(FuncData _sframe)
        {
            _sframe.AssertCount(4);
            WinRegistry.Rename(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2), _sframe.GetAt<string>(3));
            return null;
        }

        private object RegDeleteKey(FuncData _sframe)
        {
            _sframe.AssertCount(2);
            WinRegistry.DeleteKey(_sframe.GetAt<string>(1));
            return null;
        }

        private object RegRenameKey(FuncData _sframe)
        {
            _sframe.AssertCount(3);
            WinRegistry.RenameKey(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2));
            return null;
        }

        private object RegCreateKey(FuncData _sframe)
        {
            _sframe.AssertCount(3);
            WinRegistry.RenameKey(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2));
            _sframe.Status = ErrorSuccess.Created;
            return null;
        }
        
        private object RegEnumValues(FuncData _sframe)
        {
            _sframe.AssertCount(2);

            object[][] values = WinRegistry.EnumerateValues(_sframe.GetAt<string>(1));
            if (values.Length == 0) {
                _sframe.Status = ErrorSuccess.NoContent;
                return null;
            }
            else {
                return values;
            }
        }

        private static object RegEnumKeys(FuncData _sframe)
        {
            _sframe.AssertCount(2);
            return WinRegistry.EnumeratKeys(_sframe.GetAt<string>(1));
        }

        private object RegWrite(FuncData _sframe)
        {
            _sframe.AssertCount(5);

            object value = _sframe.GetAt(3);
            RegistryValueKind kind = _sframe.GetAt<RegistryValueKind>(4);

            switch (kind) {
                case RegistryValueKind.Binary:
                    value = Convert.FromBase64String(_sframe.GetAt<string>(3));
                    break;
                case RegistryValueKind.MultiString:
                    string strval = value as string;
                    if (value is string) {
                        value = _sframe.GetAt<string>(3).Replace("\r\n", "\n").Split('\n');
                    }
                    else if (value is string[]) {
                        value = _sframe.GetAt<string[]>(3);
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
            WinRegistry.Write(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2), value, kind);
            return null;
        }
    }
}