// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Drawing;
using System.Windows.Forms;
using Tbasic.Win32;
using Tbasic.Errors;
using Forms = System.Windows.Forms;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    /// <summary>
    /// A library used to automate and manipulate key strokes, mouse clicks and other input
    /// </summary>
    public class AutoLibrary : Library
    {
        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public AutoLibrary()
        {
            Add("MouseClick", MouseClick);
            Add("BlockInput", BlockInput);
            Add("MouseMove", MouseMove);
            Add("Send", Send);
            Add("VolumeUp", VolumeUp);
            Add("VolumeDown", VolumeDown);
            Add("VolumeMute", VolumeMute);
        }

        /// <summary>
        /// Mouse buttons
        /// </summary>
        public enum MouseButton : long
        {
            /// <summary>
            /// The left mouse button
            /// </summary>
            Left = MouseEvents.MOUSEEVENTF_LEFTDOWN | MouseEvents.MOUSEEVENTF_LEFTUP,
            /// <summary>
            /// The right mouse button
            /// </summary>
            Right = MouseEvents.MOUSEEVENTF_RIGHTDOWN | MouseEvents.MOUSEEVENTF_RIGHTUP
        }

        /// <summary>
        /// Clicks the mouse a specified number of times
        /// </summary>
        /// <param name="button">the mouse button to press</param>
        /// <param name="x">the final x position of the cursor</param>
        /// <param name="y">the final y position of the cursor</param>
        /// <param name="clicks">the number of clicks</param>
        /// <param name="delay">the delay to cursor motion (higher numbers are slower)</param>
        public static void MouseClick(MouseButton button, int x, int y, int clicks = 1, int delay = 1)
        {
            MouseMove(x, y, delay);
            long action = (long)button;
            for (int i = 0; i < clicks; i++) {
                User32.mouse_event(action, x, y, 0, 0);
            }
        }

        private object MouseClick(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 4) {
                runtime.Add(1);
                runtime.Add(1);
            }
            if (runtime.ParameterCount == 5) {
                runtime.Add(5);
            }
            runtime.AssertCount(6);

            int x = runtime.GetAt<int>(2),
                y = runtime.GetAt<int>(3),
                clicks = runtime.GetAt<int>(4),
                speed = runtime.GetAt<int>(5);

            MouseButton button;
            if (runtime.GetEnum(1, "button", "LEFT", "RIGHT").EqualsIgnoreCase("LEFT")) {
                button = MouseButton.Left;
            }
            else {
                button = MouseButton.Right;
            }
            MouseClick(button, x, y, clicks, speed);
            return null;
        }

        private object MouseMove(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 3) {
                runtime.Add(1);
            }
            runtime.AssertCount(4);

            MouseMove(runtime.GetAt<int>(1),
                      runtime.GetAt<int>(2),
                      runtime.GetAt<int>(3));

            return null;
        }

        /// <summary>
        /// Moves the mouse to a specified position on the screen
        /// </summary>
        /// <param name="endX">the final x position of the cursor</param>
        /// <param name="endY">the final y position of the cursor</param>
        /// <param name="delay">the delay to cursor motion (higher numbers are slower)</param>
        public static void MouseMove(double endX, double endY, int delay = 1)
        {
            if (delay > 0) {
                double startX = Cursor.Position.X,
                       startY = Cursor.Position.Y;

                double direction = startX < endX ? 1 : -1;
                double slope = (endY - startY) / (endX - startX);
                delay = (int)(Math.Sqrt(delay));

                double oldX = startX;
                for (double x = startX; !IsBetween(endX, oldX, x); x += direction) {
                    double y = slope * (x - startX) + startY;
                    int newX = (int)(x + 0.5),
                        newY = (int)(y + 0.5);
                    System.Threading.Thread.Sleep(delay);
                    oldX = x;
                    Cursor.Position = new Point(newX, newY);

                }
            }
            Cursor.Position = new Point((int)endX, (int)endY);
        }

        private static bool IsBetween(double x, double d1, double d2)
        {
            if (d1 < d2) {
                return (x <= d2) && (x >= d1);
            }
            else {
                return (x <= d1) && (x >= d2);
            }
        }

        /// <summary>
        /// Blocks user input
        /// </summary>
        /// <param name="blocked">true will block, false will unblock</param>
        /// <returns>true if the operation succeeded, otherwise false</returns>
        public static bool BlockInput(bool blocked = true)
        {
            return User32.BlockInput(blocked);
        }

        private object BlockInput(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            if (!BlockInput(runtime.GetAt<bool>(1))) {
                runtime.Status = ErrorClient.Forbidden;
            }
            return null;
        }

        /// <summary>
        /// Sends keys to the active window
        /// </summary>
        /// <param name="keys">the formatted key string</param>
        public static void Send(string keys)
        {
            SendKeys.SendWait(keys);
        }

        private object Send(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            Send(runtime.GetAt<string>(1));
            return null;
        }

        /// <summary>
        /// Press the volume up key a given number of times
        /// </summary>
        /// <param name="amnt">number of times to press the key</param>
        public static void VolumeUp(int amnt = 1)
        {
            for (int i = 0; i < amnt; i++) {
                User32.keybd_event((byte)Forms.Keys.VolumeUp, 0, 0, 0);
            }
        }

        private object VolumeUp(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 1) {
                runtime.Add(1);
            }
            runtime.AssertCount(2);
            VolumeUp(runtime.GetAt<int>(1));
            return null;
        }

        /// <summary>
        /// Press the volume down key a given number of times
        /// </summary>
        /// <param name="amnt">number of times to press the key</param>
        public static void VolumeDown(int amnt = 1)
        {
            for (int i = 0; i < amnt; i++) {
                User32.keybd_event((byte)Forms.Keys.VolumeDown, 0, 0, 0);
            }
        }

        private object VolumeDown(RuntimeData runtime)
        {
            if (runtime.ParameterCount == 1) {
                runtime.Add(1);
            }
            runtime.AssertCount(2);
            VolumeDown(runtime.GetAt<int>(1));
            return null;
        }

        /// <summary>
        /// Toggle volume mute
        /// </summary>
        public static void VolumeMute()
        {
            User32.keybd_event((byte)Forms.Keys.VolumeMute, 0, 0, 0);
        }

        private object VolumeMute(RuntimeData runtime)
        {
            runtime.AssertCount(1);
            VolumeMute();
            return null;
        }
    }
}