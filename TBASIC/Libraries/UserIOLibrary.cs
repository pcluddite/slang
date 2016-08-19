// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Microsoft.VisualBasic;
using System;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;
using Tbasic.Errors;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    /// <summary>
    /// A library for basic user input and output operations
    /// </summary>
    public class UserIOLibrary : Library
    {
        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public UserIOLibrary()
        {
            Add("TrayTip", TrayTip);
            Add("MsgBox", MsgBox);
            Add("Say", Say);
            Add("Input", Input);
            Add("StdRead", ConsoleRead);
            Add("StdReadLine", ConsoleReadLine);
            Add("StdReadKey", ConsoleReadKey);
            Add("StdWrite", ConsoleWrite);
            Add("StdWriteLine", ConsoleWriteline);
            Add("StdPause", ConsolePause);
        }

        private object ConsoleWriteline(RuntimeData _sframe)
        {
            _sframe.AssertCount(2);
            Console.WriteLine(_sframe.GetAt(1));
            return null;
        }

        private object ConsoleWrite(RuntimeData _sframe)
        {
            _sframe.AssertCount(2);
            Console.Write(_sframe.GetAt(1));
            return null;
        }

        private object ConsoleRead(RuntimeData _sframe)
        {
            _sframe.AssertCount(1);
            return Console.Read();
        }

        private object ConsoleReadLine(RuntimeData _sframe)
        {
            _sframe.AssertCount(1);
            return Console.ReadLine();
        }

        private object ConsoleReadKey(RuntimeData _sframe)
        {
            _sframe.AssertCount(1);
            return Console.ReadKey().KeyChar;
        }

        private object ConsolePause(RuntimeData _sframe)
        {
            _sframe.AssertCount(1);
            return Console.ReadKey(true).KeyChar;
        }

        /// <summary>
        /// Prompts the user to input data
        /// </summary>
        /// <param name="prompt">the text for the message prompt</param>
        /// <param name="title">the title of the message box</param>
        /// <param name="defaultResponse">the default response</param>
        /// <param name="x">the x position of the window</param>
        /// <param name="y">the y position of the window</param>
        /// <returns></returns>
        public static string InputBox(string prompt, string title = "", string defaultResponse = "", int x = -1, int y = -1)
        {
            return Interaction.InputBox(prompt, title, defaultResponse, x, y);
        }

        private object Input(RuntimeData _sframe)
        {
            if (_sframe.ParameterCount == 2) {
                _sframe.AddRange("TBASIC", -1, -1);
            }
            if (_sframe.ParameterCount == 3) {
                _sframe.AddRange(-1, -1);
            }
            if (_sframe.ParameterCount == 4) {
                _sframe.AddRange(-1);
            }
            _sframe.AssertCount(5);

            int x = _sframe.GetAt<int>(3),
                y = _sframe.GetAt<int>(4);

            string resp = InputBox(_sframe.GetAt<string>(1), _sframe.GetAt<string>(2), "", x, y);

            if (string.IsNullOrEmpty(resp)) { 
                _sframe.Status = ErrorSuccess.NoContent; // -1 no input 2/24
                return null;
            }
            else {
                return resp;
            }
        }

        /// <summary>
        /// Creates a notification balloon
        /// </summary>
        /// <param name="text">the text of the balloon</param>
        /// <param name="title">the title of the balloon</param>
        /// <param name="icon">the balloon icon</param>
        /// <param name="timeout">the length of time the balloon should be shown (this may not be honored by the OS)</param>
        public static void TrayTip(string text, string title = "", ToolTipIcon icon = ToolTipIcon.None, int timeout = 5000)
        {
            Thread t = new Thread(MakeTrayTip);
            t.Start(new object[] { timeout, icon, text, title });
        }

        private object TrayTip(RuntimeData _sframe)
        {
            if (_sframe.ParameterCount == 2) {
                _sframe.Add(""); // title
                _sframe.Add(0); // icon
                _sframe.Add(5000); // timeout
            }
            else if (_sframe.ParameterCount == 3) {
                _sframe.Add(0); // icon
                _sframe.Add(5000); // timeout
            }
            else if (_sframe.ParameterCount == 4) {
                _sframe.Add(5000); // timeout
            }
            _sframe.AssertCount(5);
            TrayTip(text: _sframe.GetAt<string>(1), title: _sframe.GetAt<string>(2), icon: _sframe.GetAt<ToolTipIcon>(3), timeout: _sframe.GetAt<int>(4));
            return null;
        }

        private static void MakeTrayTip(object param)
        {
            try {
                object[] cmd = (object[])param;
                using (NotifyIcon tray = new NotifyIcon()) {
                    tray.Icon = Properties.Resources.blank;
                    tray.Visible = true;
                    int timeout = (int)cmd[0];
                    ToolTipIcon icon;
                    switch ((int)cmd[1]) {
                        case 1: icon = ToolTipIcon.Info; break;
                        case 2: icon = ToolTipIcon.Warning; break;
                        case 3: icon = ToolTipIcon.Error; break;
                        default: icon = ToolTipIcon.None; break;
                    }
                    tray.ShowBalloonTip(timeout, (string)cmd[3], (string)cmd[2], icon);
                    Thread.Sleep(timeout);
                    tray.Visible = false;
                }
            }
            catch {
            }
        }

        /// <summary>
        /// Creates a message box
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="buttons"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string MsgBox(object prompt, int buttons = (int)MsgBoxStyle.ApplicationModal, object title = null)
        {
            return Interaction.MsgBox(prompt, (MsgBoxStyle)buttons, title).ToString();
        }

        private object MsgBox(RuntimeData _sframe)
        {
            if (_sframe.ParameterCount == 3) {
                _sframe.Add("");
            }
            _sframe.AssertCount(4);

            int flag = _sframe.GetAt<int>(1);
            string text = _sframe.GetAt<string>(2),
                   title = _sframe.GetAt<string>(3);

            return MsgBox(buttons: flag, prompt: text, title: title);
        }

        /// <summary>
        /// Converts text to synthesized speech
        /// </summary>
        /// <param name="text">the text to speak</param>
        public static void Say(string text)
        {
            Thread t = new Thread(Say);
            t.Start(text);
        }

        private object Say(RuntimeData _sframe)
        {
            _sframe.AssertCount(2);
            Say(_sframe.GetAt<string>(1));
            return null;
        }

        private static void Say(object text)
        {
            try {
                using (SpeechSynthesizer ss = new SpeechSynthesizer()) {
                    ss.Speak(text.ToString());
                }
            }
            catch {
            }
        }
    }
}
