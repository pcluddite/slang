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
            if (args.Length > 0 && File.Exists(args[0])) {
                file = args[0];
            }
            if (file == null) {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Open";
                dialog.FileName = "";
                dialog.Multiselect = false;
                dialog.Filter = "Tbasic Script (*.tba)|*.tba|All Files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    file = dialog.FileName;
                }
                else {
                    return;
                }
            }

            try {
                Executer exec = new Executer();
                exec.Global.LoadStandardLibrary();
                exec.Execute(File.ReadAllLines(file));
            }
            catch (ScriptParsingException ex) {
                MessageBox.Show(ex.Message, "Tbasic Script Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}