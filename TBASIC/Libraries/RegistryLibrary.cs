﻿/**
 *  TBASIC
 *  Copyright (C) 2013-2016 Timothy Baxendale
 *  
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *  
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *  
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301
 *  USA
 **/
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using Tbasic.Borrowed;
using Tbasic.Runtime;

namespace Tbasic.Libraries {
    internal class RegistryLibrary : Library {

        public RegistryLibrary() {
            Add("regenumkeys", RegEnumKeys);
            Add("regenumvalues", RegEnumValues);
            Add("regrenamekey", RegRenameKey);
            Add("regrename", RegRename);
            Add("regdelete", RegDelete);
            Add("regdeletekey", RegDeleteKey);
            Add("regcreatekey", RegCreateKey);
            Add("regread", RegRead);
            Add("regwrite", RegWrite);
        }

        private RegistryKey GetRootKey(string key) {
            key = key.ToUpper();
            if (key.StartsWith("HKEY_CURRENT_USER")) {
                return Registry.CurrentUser;
            }
            else if (key.StartsWith("HKEY_CLASSES_ROOT")) { 
                return Registry.ClassesRoot;
            }
            else if (key.StartsWith("HKEY_LOCAL_MACHINE")) { 
                return Registry.LocalMachine;
            }
            else if (key.StartsWith("HKEY_USERS")) { 
                return Registry.Users;
            }
            else if (key.StartsWith("HKEY_CURRENT_CONFIG")) { 
                return Registry.CurrentConfig; 
            }
            return null;
        }

        private string RemoveKeyRoot(string key) {
            int indexOfRoot = key.IndexOf('\\');
            if (indexOfRoot < 0) {
                return "";
            }
            string ret = key.Remove(0, indexOfRoot);
            while (ret.StartsWith("\\")) {
                ret = ret.Remove(0, 1);
            }
            return ret;
        }

        private void RegValueKind(Paramaters _sframe) {
            _sframe.AssertArgs(3);
            using (RegistryKey key = OpenKey(_sframe.Get<string>(1), false)) {
                _sframe.Data = key.GetValueKind(_sframe.Get<string>(2)).ToString();
            }
        }

        private RegistryValueKind RegValueKind(string key, string value) {
            RegistryKey keyBase = GetRootKey(key);
            using (keyBase = keyBase.OpenSubKey(RemoveKeyRoot(key))) {
                RegistryValueKind kind = keyBase.GetValueKind(value);
                return kind;
            }
        }

        private void RegRead(Paramaters _sframe) {
            _sframe.AssertArgs(3);

            object ret = RegRead(_sframe.Get<string>(1), _sframe.Get<string>(2));

            if (ret == null) {
                _sframe.Status = -1; // -1 not found 2/24/15
            }
            else {
                _sframe.Data = ret;
            }
        }

        static string byteToString(byte b) {
            return Convert.ToString(b, 16);
        }

        public object RegRead(string key, string value) {
            return RegistryUtilities.Read(GetRootKey(key), RemoveKeyRoot(key), value, null);
        }

        private void RegDelete(Paramaters _sframe) {
            _sframe.AssertArgs(3);
            RegistryKey key = GetRootKey(_sframe.Get<string>(1));
            using (key = key.OpenSubKey(RemoveKeyRoot(_sframe.Get<string>(1)), true)) {
                key.DeleteValue(_sframe.Get<string>(2), true);
            }
        }

        private void RegRename(Paramaters _sframe) {
            _sframe.AssertArgs(4);
            RegistryKey key = GetRootKey(_sframe.Get<string>(1));
            using (key = key.OpenSubKey(RemoveKeyRoot(_sframe.Get<string>(1)), true)) {
                key.SetValue(_sframe.Get<string>(3), key.GetValue(_sframe.Get<string>(2)), key.GetValueKind(_sframe.Get<string>(2)));
                key.DeleteValue(_sframe.Get<string>(2), true);
            }
        }

        private void RegDeleteKey(Paramaters _sframe) {
            _sframe.AssertArgs(2);
            using (RegistryKey key = GetRootKey(_sframe.Get<string>(1))) {
                key.DeleteSubKeyTree(RemoveKeyRoot(_sframe.Get<string>(1)));
            }
        }

        private void RegRenameKey(Paramaters _sframe) {
            _sframe.AssertArgs(3);
            using (RegistryKey key = OpenParentKey(_sframe.Get<string>(1), true)) {
                RegistryUtilities.RenameSubKey(key, RemoveKeyRoot(_sframe.Get<string>(1)), _sframe.Get<string>(2));
            }
        }

        private void RegCreateKey(Paramaters _sframe) {
            _sframe.AssertArgs(3);
            using (RegistryKey key = OpenKey(_sframe.Get<string>(1), true)) {
                key.CreateSubKey(_sframe.Get<string>(2));
            }
        }

        private string ByteToHex(byte b) {
            return int.Parse(b.ToString()).ToString("x2");
        }

        private void RegEnumValues(Paramaters _sframe) {
            _sframe.AssertArgs(2);

            using (RegistryKey key = OpenKey(_sframe.Get<string>(1), false)) {
                List<object[]> values = new List<object[]>();
                foreach (string valueName in key.GetValueNames()) {
                    values.Add(new object[] { valueName, key.GetValue(valueName) });
                }
                _sframe.Data = values.ToArray();
            }
        }

        public RegistryKey OpenKey(string path, bool write) {
            using (RegistryKey key = GetRootKey(path)) {
                return key.OpenSubKey(RemoveKeyRoot(path), write);
            }
        }

        public RegistryKey OpenParentKey(string path, bool write) {
            int indexOfLast = path.LastIndexOf('\\');
            if (indexOfLast > -1) {
                path = path.Substring(0, indexOfLast);
            }
            return OpenKey(path, write);
        }

        private void RegEnumKeys(Paramaters _sframe) {
            _sframe.AssertArgs(2);
            using (RegistryKey key = OpenKey(_sframe.Get<string>(1), false)) {
                _sframe.Data = key.GetSubKeyNames();
            }
        }

        private void RegWrite(Paramaters _sframe) {
            _sframe.AssertArgs(5);

            object value = _sframe.Get(3);

            RegistryValueKind kind;
            switch (_sframe.Get<string>(4).ToLower()) {
                case "binary": kind = RegistryValueKind.Binary;
                    List<string> slist = new List<string>();
                    slist.AddRange(_sframe.Get<string>(3).Split(' '));
                    value = slist.ConvertAll<byte>(new Converter<string, byte>(stringToByte)).ToArray();
                    break;
                case "dword": 
                    kind = RegistryValueKind.DWord; 
                    break;
                case "expandstring": 
                    kind = RegistryValueKind.ExpandString; 
                    break;
                case "multistring": 
                    kind = RegistryValueKind.MultiString;
                    if (_sframe.Get(3) is string) {
                        value = _sframe.Get<string>(3).Replace("\r\n", "\n").Split('\n');
                    }
                    else if (_sframe.Get(3) is string[]) {
                        value = _sframe.Get<string[]>(3);
                    }
                    else {
                        throw new ArgumentException("parameter is not a valid multi-string");
                    }
                    break;
                case "qword": 
                    kind = RegistryValueKind.QWord; 
                    break;
                case "string": 
                    kind = RegistryValueKind.String; 
                    break;
                default:
                    throw new ArgumentException("unknown registry type '" + _sframe.Get(4) + "'");
            }

            using (RegistryKey key = OpenKey(_sframe.Get<string>(1), true)) {
                key.SetValue(_sframe.Get<string>(2), value, kind);
            }
        }

        private byte stringToByte(string s) {
            return Convert.ToByte(s, 16);
        }
    }
}