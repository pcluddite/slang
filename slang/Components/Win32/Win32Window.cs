﻿/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Runtime.InteropServices;

namespace Slang.Win32
{
    internal class Win32Window : IDisposable
    {

        private const int ERROR_CLASS_ALREADY_EXISTS = 1410;

        public bool Disposed { get; private set; }
        public IntPtr Handle { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!Disposed) {
                if (disposing) {
                    // Dispose managed resources
                }

                // Dispose unmanaged resources
                if (Handle != IntPtr.Zero) {
                    User32.DestroyWindow(Handle);
                    Handle = IntPtr.Zero;
                }

            }
        }

        public Win32Window(string class_name)
        {
            if (class_name == null) {
                throw new ArgumentNullException(nameof(class_name));
            }
            if (class_name == string.Empty) {
                throw new ArgumentException("class name is empty string", "class_name");
            }

            m_wnd_proc_delegate = CustomWndProc;

            // Create WNDCLASS
            WNDCLASS wind_class = new WNDCLASS();
            wind_class.lpszClassName = class_name;
            wind_class.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(m_wnd_proc_delegate);

            ushort class_atom = User32.RegisterClassW(ref wind_class);

            int last_error = Marshal.GetLastWin32Error();

            if (class_atom == 0 && last_error != ERROR_CLASS_ALREADY_EXISTS) {
                throw new Exception("Could not register. Class already exists");
            }

            // Create window
            Handle = User32.CreateWindowExW(
                0,
                class_name,
                string.Empty,
                0,
                0,
                0,
                0,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            );
        }

        public bool Show()
        {
            return Show(1);
        }

        public bool Show(uint show)
        {
            return User32.ShowWindow(Handle, show);
        }

        public bool Destroy()
        {
            return User32.DestroyWindow(Handle);
        }

        public IntPtr CustomWndProc(uint msg, IntPtr wParam, IntPtr lParam)
        {
            return User32.DefWindowProcW(Handle, msg, wParam, lParam);
        }

        private static IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return User32.DefWindowProcW(hWnd, msg, wParam, lParam);
        }

        private WndProc m_wnd_proc_delegate;
    }
}