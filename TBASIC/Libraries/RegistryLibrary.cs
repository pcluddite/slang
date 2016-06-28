/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
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
        
        private void RegValueKind(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            _sframe.Data = WinRegistry.GetValueKind(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2)).ToString();
        }

        private void RegRead(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(atLeast: 3, atMost: 4);

            if (_sframe.ParameterCount == 3)
                _sframe.AddParameter(null);
            
            _sframe.Data = WinRegistry.Read(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2), _sframe.GetParameter<string>(3));
        }

        private void RegDelete(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            WinRegistry.Delete(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2));
        }

        private void RegRename(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(4);
            WinRegistry.Rename(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2), _sframe.GetParameter<string>(3));
        }

        private void RegDeleteKey(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(2);
            WinRegistry.DeleteKey(_sframe.GetParameter<string>(1));
        }

        private void RegRenameKey(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            WinRegistry.RenameKey(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2));
        }

        private void RegCreateKey(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(3);
            WinRegistry.RenameKey(_sframe.GetParameter<string>(1), _sframe.GetParameter<string>(2));
            _sframe.Status = ErrorSuccess.Created;
        }
        
        private void RegEnumValues(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(2);

            object[][] values = WinRegistry.EnumerateValues(_sframe.GetParameter<string>(1));
            if (values.Length == 0) {
                _sframe.Status = ErrorSuccess.NoContent;
            }
            else {
                _sframe.Data = values;
            }
        }

        private static void RegEnumKeys(TFunctionData _sframe)
        {
            _sframe.AssertParamCount(2);
            _sframe.Data = WinRegistry.EnumeratKeys(_sframe.GetParameter<string>(1));
        }

        private void RegWrite(TFunctionData _sframe)
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
        }
    }
}