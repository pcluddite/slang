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
        
        private object RegValueKind(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            return WinRegistry.GetValueKind(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2)).ToString();
        }

        private object RegRead(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(atLeast: 3, atMost: 4);

            if (_sframe.ParameterCount == 3)
                _sframe.AddParameter(null);
            
            return WinRegistry.Read(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2), _sframe.GetParameter<string>(3));
        }

        private object RegDelete(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            WinRegistry.Delete(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2));
            return null;
        }

        private object RegRename(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(4);
            WinRegistry.Rename(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2), _sframe.GetParameter<string>(3));
            return null;
        }

        private object RegDeleteKey(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(2);
            WinRegistry.DeleteKey(_sframe.GetParameter<string>(1));
            return null;
        }

        private object RegRenameKey(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            WinRegistry.RenameKey(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2));
            return null;
        }

        private object RegCreateKey(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            WinRegistry.RenameKey(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2));
            _sframe.Status = ErrorSuccess.Created;
            return null;
        }
        
        private object RegEnumValues(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(2);

            object[][] values = WinRegistry.EnumerateValues(_sframe.GetParameter<string>(1));
            if (values.Length == 0) {
                _sframe.Status = ErrorSuccess.NoContent;
                return null;
            }
            else {
                return values;
            }
        }

        private static object RegEnumKeys(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(2);
            return WinRegistry.EnumeratKeys(_sframe.GetParameter<string>(1));
        }

        private object RegWrite(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(5);

            object value = _sframe.GetParameter(3);
            RegistryValueKind kind = _sframe.GetParameter<RegistryValueKind>(4);

            switch (kind) {
                case RegistryValueKind.Binary:
                    value = Convert.FromBase64String(_sframe.GetParameter<string>(3));
                    break;
                case RegistryValueKind.MultiString:
                    string strval = value as string;
                    if (value is string) {
                        value = _sframe.GetParameter<string>(3).Replace("\r\n", "\n").Split('\n');
                    }
                    else if (value is string[]) {
                        value = _sframe.GetParameter<string[]>(3);
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
            WinRegistry.Write(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2), value, kind);
            return null;
        }
    }
}