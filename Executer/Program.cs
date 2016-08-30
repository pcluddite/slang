// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.IO;
using System.Windows.Forms;
using Tbasic.Runtime;
using Tbasic.Errors;

namespace Tbasic.Runner
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string file = null;
            if (args.Length > 0) {
                file = args[0];
            }
            else {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Title = "Open Script",
                    FileName = string.Empty,
                    Multiselect = false,
                    Filter = "TBASIC Script (*.tba)|*.tba|Text File (*.txt)|*.txt|All Files (*.*)|*.*"
                };
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                file = dialog.FileName;
            }
            RunScript(file);
        }

        public static void RunScript(string filename)
        {
            try {
                TBasic exec = new TBasic();
                exec.Global.LoadStandardLibrary();
                using (StreamReader fstream = new StreamReader(File.OpenRead(filename))) {
                    exec.Execute(fstream);
                }
            }
            catch (TbasicRuntimeException ex) {
                ShowError(ex.Message);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) {
                ShowError(ex.Message, "Unable to Open Script");
            }
        }

        private static void ShowError(string msg, string title = "TBASIC Runtime Error")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}