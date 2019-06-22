/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Slang.Runtime;
using System.Windows.Forms;
using Slang.Errors;

namespace Slang.Tbasic
{
    internal static class ScriptHost
    {
        public static string PromptOpenScript()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "Open Script",
                FileName = string.Empty,
                Multiselect = false,
                Filter = "Tbasic Script (*.tbs)|*.tbs|Text File (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return null;
            return dialog.FileName;
        }

        public static void RunScript(string filename)
        {
            try {
                Executor runtime = new Executor();
                runtime.Global.LoadStandardLibrary();
                using (StreamReader fstream = new StreamReader(File.OpenRead(filename))) {
                    runtime.Execute(fstream);
                }
            }
            catch (FileNotFoundException ex) {
                throw new FunctionException(ErrorClient.NotFound, ex);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) {
                throw new FunctionException(ErrorClient.Locked, ex);
            }
        }
    }
}
