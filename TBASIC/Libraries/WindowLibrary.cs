﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Drawing;
using System.IO;
using System.Text;
using Tbasic.Components;
using Tbasic.Runtime;
using Tbasic.Win32;
using Tbasic.Errors;
using System.Linq;
using System.Collections.Generic;

namespace Tbasic.Libraries
{
    internal class WindowLibrary : Library
    {
        public WindowLibrary(ObjectContext context)
        {
            Add("WinActivate", WinActivate);
            Add("WinClose", WinClose);
            Add("WinKill", WinKill);
            Add("WinMove", WinMove);
            Add("WinSize", WinSize);
            Add("WinGetHandle", WinGetHandle);
            Add("WinGetTitle", WinGetTitle);
            Add("WinGetState", WinGetState);
            Add("WinSetTitle", WinSetTitle);
            Add("WinSetTrans", WinSetTrans);
            Add("WinSetState", WinSetState);
            Add("WinList", WinList);
            Add("ScreenCapture", GetScreen);
            Add("WinPicture", WinPicture);
            Add("WinExists", WinExists);
            // window constants for WinSetState()
            context.SetConstant("@SW_HIDE", WindowState.SW_HIDE);
            context.SetConstant("@SW_MAXIMIZE", WindowState.SW_MAXIMIZE);
            context.SetConstant("@SW_MINIMIZE", WindowState.SW_MINIMIZE);
            context.SetConstant("@SW_NORMAL", WindowState.SW_NORMAL);
            context.SetConstant("@SW_RESTORE", WindowState.SW_RESTORE);
            context.SetConstant("@SW_SHOW", WindowState.SW_SHOW);
            context.SetConstant("@SW_SHOWMAXIMIZED", WindowState.SW_SHOWMAXIMIZED);
            context.SetConstant("@SW_SHOWMINIMIZED", WindowState.SW_SHOWMINIMIZED);
            context.SetConstant("@SW_SHOWMINNOACTIVE", WindowState.SW_SHOWMINNOACTIVE);
            context.SetConstant("@SW_SHOWNA", WindowState.SW_SHOWNA);
            context.SetConstant("@SW_SHOWNOACTIVATE", WindowState.SW_SHOWNOACTIVATE);
            context.SetConstant("@SW_SHOWNORMAL", WindowState.SW_SHOWNORMAL);
        }

        public static int WinRemoveClose(IntPtr hwnd)
        {
            IntPtr hMenu = User32.GetSystemMenu(hwnd, false);
            int menuItemCount = User32.GetMenuItemCount(hMenu);
            return User32.RemoveMenu(hMenu, menuItemCount - 1, User32.MF_BYPOSITION);
        }

        private object WinRemoveClose(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            return WinRemoveClose(new IntPtr(parameters.GetAt<long>(1)));
        }

        public static IntPtr WinGetHandle(string title, string sz_class = null)
        {
            return User32.FindWindow(sz_class, title);
        }

        private object WinGetHandle(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            return WinGetHandle(parameters.GetAt<string>(1)).ToInt64();
        }

        private object WinGetTitle(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            return WinGetTitle(new IntPtr(parameters.GetAt<long>(1)));
        }

        public static string WinGetTitle(IntPtr hwnd)
        {
            int capacity = User32.GetWindowTextLength(hwnd) + 1;
            StringBuilder sb = new StringBuilder(capacity);
            User32.GetWindowText(hwnd, sb, capacity);
            return sb.ToString();
        }

        private object WinSetTitle(RuntimeData parameters)
        {
            parameters.AssertCount(3);
            IntPtr hwnd = new IntPtr(parameters.GetAt<long>(1));
            if (!User32.SetWindowText(hwnd, parameters.GetAt<string>(2))) {
                throw new FunctionException(ErrorServer.GenericError, "Unable to set window title");
            }
            return null;
        }

        public static bool WinSetState(IntPtr hwnd, uint flag)
        {
            return User32.ShowWindow(hwnd, flag);
        }

        private object WinGetState(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            return WinGetState(parameters.GetAt<long>(1));
        }

        public static int WinGetState(long hwnd)
        {
            return (int)WinGetState(new IntPtr(hwnd));
        }

        public static WindowFlag WinGetState(IntPtr hwnd)
        {
            return Windows.GetState(hwnd);
        }

        private object WinSetState(RuntimeData parameters)
        {
            parameters.AssertCount(3);
            uint flag = parameters.GetAt<uint>(2);
            if (!WinSetState(new IntPtr(parameters.GetAt<long>(1)), flag)) {
                throw new FunctionException(ErrorServer.GenericError, "Unable to set window state");
            }
            return null;
        }

        public static bool WinActivate(IntPtr hwnd)
        {
            return User32.SetForegroundWindow(hwnd);
        }

        private object WinActivate(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            if (!WinActivate(new IntPtr(parameters.GetAt<long>(1)))) {
                throw new FunctionException(ErrorServer.GenericError, "Unable to activate window");
            }
            return null;
        }

        public static bool WinMove(IntPtr hwnd, int x, int y)
        {
            RECT rect = new RECT();
            if (!User32.GetWindowRect(hwnd, out rect)) {
                return false;
            }
            return User32.SetWindowPos(hwnd, HWND.NoTopMost,
                x, y,
                (rect.Right - rect.Left), (rect.Bottom - rect.Top), SWP.NOACTIVATE);
        }

        private object WinMove(RuntimeData parameters)
        {
            parameters.AssertCount(4);
            IntPtr hwnd = new IntPtr(parameters.GetAt<long>(1));
            if (!WinMove(hwnd, parameters.GetAt<int>(2), parameters.GetAt<int>(3))) {
                throw new FunctionException(ErrorServer.GenericError, "Unable to move window");
            }
            return null;
        }

        public static bool WinSize(IntPtr hwnd, int width, int height)
        {
            WINDOWPLACEMENT place = new WINDOWPLACEMENT();
            if (!User32.GetWindowPlacement(hwnd, out place)) {
                return false;
            }
            return User32.SetWindowPos(hwnd, HWND.NoTopMost,
                place.rcNormalPosition.X, place.rcNormalPosition.Y, width, height, SWP.NOACTIVATE);
        }

        private object WinSize(RuntimeData parameters)
        {
            parameters.AssertCount(4);
            IntPtr hwnd = new IntPtr(parameters.GetAt<long>(1));
            if (!WinSize(hwnd, parameters.GetAt<int>(2), parameters.GetAt<int>(3))) {
                throw new FunctionException(ErrorServer.GenericError, "Unable to resize window");
            }
            return null;
        }

        public static bool WinKill(IntPtr hwnd)
        {
            return User32.DestroyWindow(hwnd);
        }

        private object WinKill(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            long hwnd = parameters.GetAt<long>(1);
            if (!WinKill(new IntPtr(hwnd))) {
                throw new FunctionException(ErrorServer.GenericError, "Unable to kill window");
            }
            return null;
        }

        public static IntPtr WinClose(IntPtr hwnd)
        {
            return User32.SendMessage(hwnd, SendMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        private object WinClose(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            return WinClose(new IntPtr(parameters.GetAt<long>(1)));
        }

        public static bool WinSetTrans(IntPtr hwnd, byte trans)
        {
            User32.SetWindowLong(hwnd, User32.GWL_EXSTYLE, User32.GetWindowLong(hwnd, User32.GWL_EXSTYLE) ^ User32.WS_EX_LAYERED);
            return User32.SetLayeredWindowAttributes(hwnd, 0, trans, User32.LWA_ALPHA);
        }

        private object WinSetTrans(RuntimeData parameters)
        {
            parameters.AssertCount(3);
            IntPtr hwnd = new IntPtr(parameters.GetAt<long>(1));
            if (!WinSetTrans(hwnd, parameters.GetAt<byte>(2))) {
                throw new FunctionException(ErrorServer.GenericError, "Unable to set window transparency");
            }
            return null;
        }

        public static IEnumerable<IntPtr> WinList(WindowFlag flag = WindowFlag.None)
        {
            if (flag == 0) {
                return Windows.List();
            }
            else {
                return Windows.List(flag);
            }
        }

        private object WinList(RuntimeData parameters)
        {
            if (parameters.ParameterCount == 1) {
                parameters.Add(WindowFlag.Existing);
            }
            parameters.AssertCount(2);
            WindowFlag state = parameters.GetAt<WindowFlag>(1);

            IntPtr[] hwnds = WinList(state).ToArray();

            if (hwnds.Length > 0) {
                object[][] windows = new object[hwnds.Length][];
                for (int index = 0; index < windows.Length; index++) {
                    windows[index] = new object[] {
                        Evaluator.ConvertToSimpleType(hwnds[index]),
                        WinGetTitle(hwnds[index])
                    };
                }
                return windows;
            }
            else {
                parameters.Status = ErrorSuccess.NoContent;
                return null;
            }
        }

        private object GetScreen(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            int compression = parameters.GetFromRange(1, 0, 100);
            return GetScreen(compression);
        }

        public static byte[] GetScreen(int compression = 50)
        {
            Image img = ScreenCapture.CaptureScreen();
            using (MemoryStream ms = Compress.DoIt(img, compression)) {
                return ms.ToArray();
            }
        }

        public static byte[] WinPicture(IntPtr hwnd, int compression = 50)
        {
            Image pic = ScreenCapture.CaptureWindow(hwnd);
            using (MemoryStream ms = Compress.DoIt(pic, compression)) {
                return ms.ToArray();
            }
        }

        private object WinPicture(RuntimeData parameters)
        {
            parameters.AssertCount(3);
            int compression = parameters.GetFromRange(2, 0, 100);
            IntPtr hwnd = new IntPtr(parameters.GetAt<long>(1));
            return WinPicture(hwnd, compression);
        }

        public static bool WindowExists(IntPtr hwnd)
        {
            return Windows.WinExists(hwnd);
        }

        private object WinExists(RuntimeData parameters)
        {
            parameters.AssertCount(2);
            return Windows.WinExists(new IntPtr(parameters.GetAt<long>(1)));
        }
    }
}