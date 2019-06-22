/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.IO;
using System.Windows.Forms;
using Tbasic.Errors;
using Tbasic.Runtime;

namespace Tbasic.ScriptHost
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
                    Filter = "Tbasic Script (*.tbs)|*.tbs|Text File (*.txt)|*.txt|All Files (*.*)|*.*"
                };
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                file = dialog.FileName;
            }
            RunScript(file);
        }

        public static void RunScript(string filename)
        {
#if !NO_CATCH
            try {
#endif
                Executor runtime = new Executor();
                runtime.Global.LoadStandardLibrary();
                using (StreamReader fstream = new StreamReader(File.OpenRead(filename))) {
                    runtime.Execute(fstream);
                }
#if !NO_CATCH
            }
            catch (TbasicRuntimeException ex) {
                ShowError(ex.Message);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) {
                ShowError(ex.Message, "Unable to Open Script");
            }
#endif
        }

        private static void ShowError(string msg, string title = "TBASIC Runtime Error")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}