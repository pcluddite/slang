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
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Tbasic.Borrowed;
using Tbasic.Runtime;
using Tbasic.Win32;

namespace Tbasic.Libraries {

    internal class WindowLibrary : Library {

        public WindowLibrary() {
            Add("winactivate", WinActivate);
            Add("winclose", WinClose);
            Add("winkill", WinKill);
            Add("winmove", WinMove);
            Add("winsize", WinSize);
            Add("wingethandle", WinGetHandle);
            Add("wingettitle", WinGetTitle);
            Add("winsettitle", WinSetTitle);
            Add("winsettrans", WinSetTrans);
            Add("winsetstate", WinSetState);
            Add("winlist", WinList);
            Add("ScreenCapture", GetScreen);
            Add("winpicture", WinPicture);
            Add("winexists", WinExists);
        }

        public static int WinRemoveClose(IntPtr hwnd) {;
            IntPtr hMenu = User32.GetSystemMenu(hwnd, false);
            int menuItemCount = User32.GetMenuItemCount(hMenu);
            return User32.RemoveMenu(hMenu, menuItemCount - 1, User32.MF_BYPOSITION);
        }

        private void WinRemoveClose(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            parameters.Data = WinRemoveClose(new IntPtr(parameters.Get<long>(1)));
        }

        public static IntPtr WinGetHandle(string title, string sz_class = null) {
            return User32.FindWindow(sz_class, title);
        }

        private void WinGetHandle(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            parameters.Data = WinGetHandle(parameters.Get<string>(1)).ToInt64();
        }

        private void WinGetTitle(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            parameters.Data = WinGetTitle(new IntPtr(parameters.Get<long>(1)));
        }

        public static string WinGetTitle(IntPtr hwnd) {
            int capacity = User32.GetWindowTextLength(hwnd) + 1;
            StringBuilder sb = new StringBuilder(capacity);
            User32.GetWindowText(hwnd, sb, capacity);
            return sb.ToString();
        }

        private void WinSetTitle(ref Paramaters parameters) {
            parameters.AssertArgs(3);
            IntPtr hwnd = new IntPtr(parameters.Get<long>(1));
            if (!User32.SetWindowText(hwnd, parameters.Get<string>(2))) {
                parameters.Status = 1;
            }
        }

        public static bool WinSetState(IntPtr hwnd, uint flag) {
            return User32.ShowWindow(hwnd, flag);
        }

        private void WinGetState(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            parameters.Data = WinGetState(parameters.Get<long>(1));
        }

        public static int WinGetState(long hwnd) {
            return WinGetState(new IntPtr(hwnd));
        }

        public static int WinGetState(IntPtr hwnd) {
            return Windows.GetState(hwnd);
        }

        private void WinSetState(ref Paramaters parameters) {
            parameters.AssertArgs(3);
            uint flag = parameters.Get<uint>(2);
            if (!WinSetState(new IntPtr(parameters.Get<long>(1)), flag)) {
                parameters.Status = 1;
            }
        }

        public static bool WinActivate(IntPtr hwnd) {
            return User32.SetForegroundWindow(hwnd);
        }

        private void WinActivate(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            if (!WinActivate(new IntPtr(parameters.Get<long>(1)))) {
                parameters.Status = 1;
            }
        }

        public static bool WinMove(IntPtr hwnd, int x, int y) {
            RECT rect = new RECT();
            if (!User32.GetWindowRect(hwnd, out rect)) {
                return false;
            }
            if (User32.SetWindowPos(hwnd, HWND.NoTopMost,
                x, y,
                (rect.Right - rect.Left), (rect.Bottom - rect.Top), SWP.NOACTIVATE)) {
                return true;
            }
            else {
                return false;
            }
        }

        private void WinMove(ref Paramaters parameters) {
            parameters.AssertArgs(4);
            IntPtr hwnd = new IntPtr(parameters.Get<long>(1));
            if (!WinMove(hwnd, parameters.Get<int>(2), parameters.Get<int>(3))) {
                parameters.Status = 1;
            }
        }
        
        public static bool WinSize(IntPtr hwnd, int width, int height) {
            WINDOWPLACEMENT place = new WINDOWPLACEMENT();
            if (!User32.GetWindowPlacement(hwnd, out place)) {
                return false;
            }
            if (User32.SetWindowPos(hwnd, HWND.NoTopMost,
                place.rcNormalPosition.X, place.rcNormalPosition.Y,
                width,
                height,
                SWP.NOACTIVATE)) {
                return true;
            }
            else {
                return false;
            }
        }

        private void WinSize(ref Paramaters parameters) {
            parameters.AssertArgs(4);
            IntPtr hwnd = new IntPtr(parameters.Get<long>(1));
            if (!WinSize(hwnd, parameters.Get<int>(2), parameters.Get<int>(3))) {
                parameters.Status = 1;
            }
        }

        public static bool WinKill(IntPtr hwnd) {
            return User32.DestroyWindow(hwnd);
        }

        private void WinKill(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            long hwnd = parameters.Get<long>(1);
            if (!WinKill(new IntPtr(hwnd))) {
                parameters.Status = 1;
            }
        }

        public static int WinClose(IntPtr hwnd) {
            return User32.SendMessage(hwnd, 0x0112, 0xF060, 0);
        }

        private void WinClose(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            parameters.Data = WinClose(new IntPtr(parameters.Get<long>(1)));
        }

        public static bool WinSetTrans(IntPtr hwnd, byte trans) {
            User32.SetWindowLong(hwnd, User32.GWL_EXSTYLE, User32.GetWindowLong(hwnd, User32.GWL_EXSTYLE) ^ User32.WS_EX_LAYERED);
            return User32.SetLayeredWindowAttributes(hwnd, 0, trans, User32.LWA_ALPHA);
        }

        private void WinSetTrans(ref Paramaters parameters) {
            parameters.AssertArgs(3);
            IntPtr hwnd = new IntPtr(parameters.Get<long>(1));
            if (!WinSetTrans(hwnd, parameters.Get<byte>(2))) {
                parameters.Status = 1;
            }
        }

        public static IntPtr[] WinList(int flag = 0) {
            return flag == 0 ? Windows.Enum() : Windows.Enum(flag);
        }

        private void WinList(ref Paramaters parameters) {
            if (parameters.Count == 1) {
                parameters.Add((int)WindowFlag.Existing);
            }
            parameters.AssertArgs(2);
            int state = parameters.Get<int>(1);
            
            IntPtr[] hwnds = Windows.Enum(state);

            if (hwnds.Length > 0) {
                object[][] windows = new object[hwnds.Length][];
                for (int index = 0; index < windows.Length; index++) {
                    windows[index] = new object[] { hwnds[index], WinGetTitle(hwnds[index]) };
                }
                parameters.Data = windows;
            }
            else {
                parameters.Status = -1; // -1 not found 2/24
            }
        }

        private void GetScreen(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            int compression = parameters.GetIntRange(1, 0, 100);
            parameters.Data = GetScreen(compression);
        }

        public static byte[] GetScreen(int compression = 50) {
            ScreenCapture sc = new ScreenCapture();
            Image img = sc.CaptureScreen();
            using (MemoryStream ms = Compress.DoIt(img, compression)) {
                return AutoLib.ReadToEnd(ms);
            }
        }

        public static byte[] WinPicture(IntPtr hwnd, int compression = 50) {
            ScreenCapture sc = new ScreenCapture();
            System.Drawing.Image pic = sc.CaptureWindow(hwnd);
            using (MemoryStream ms = Compress.DoIt(pic, compression)) {
                return AutoLib.ReadToEnd(ms);
            }
        }

        private void WinPicture(ref Paramaters parameters) {
            parameters.AssertArgs(3);
            int compression = parameters.GetIntRange(2, 0, 100);
            IntPtr hwnd = new IntPtr(parameters.Get<long>(1));
            parameters.Data = WinPicture(hwnd, compression);
        }

        public static bool WindowExists(IntPtr hwnd) {
            return Windows.WindowExists(hwnd);
        }

        private void WinExists(ref Paramaters parameters) {
            parameters.AssertArgs(2);
            parameters.Data = Windows.WindowExists(new IntPtr(parameters.Get<long>(1)));
        }
    }
}
