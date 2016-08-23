// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Text;
using Tbasic.Runtime;
using Tbasic.Errors;

namespace Tbasic.Libraries
{
    /// <summary>
    /// A library used to write and read to files or the file system
    /// </summary>
    public class FileIOLibrary : Library
    {
        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public FileIOLibrary()
        {
            Add("FileReadAll", FileReadAll);
            Add("FileWriteAll", FileWriteAll);
            Add("FileRecycle", Recycle);
            Add("FileGetAttributes", FileGetAttributes);
            Add("FileSetAttributes", FileSetAttributes);
            Add("FileSetAccessDate", FileSetAccessDate);
            Add("FileSetCreatedDate", FileSetCreatedDate);
            Add("FileSetModifiedDate", FileSetModifiedDate);
            Add("FileExists", FileExists);
            Add("DirExists", DirExists);
            Add("DirGetDirList", DirGetDirList);
            Add("DirGetFileList", DirGetFileList);
            Add("DirCreate", DirCreate);
            Add("DirMove", DirMove);
            Add("DirDelete", DirDelete);
            Add("FileDelete", FileDelete);
            Add("FileCopy", FileCopy);
            Add("FileMove", FileMove);
            Add("Shell", Shell);
        }

        private object DirExists(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return Directory.Exists(runtime.GetAt<string>(1));
        }

        private object FileExists(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return File.Exists(runtime.GetAt<string>(1));
        }

        private object FileMove(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            File.Move(runtime.GetAt<string>(1), runtime.GetAt<string>(2));
            return null;
        }

        private object FileCopy(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            File.Copy(runtime.GetAt<string>(1), runtime.GetAt<string>(2));
            return null;
        }

        private object FileDelete(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            File.Delete(runtime.GetAt<string>(1));
            return null;
        }

        private object DirDelete(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            Directory.Delete(runtime.GetAt<string>(1));
            return null;
        }

        private object DirMove(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            Directory.Move(runtime.GetAt<string>(1), runtime.GetAt<string>(2));
            return null;
        }

        private object DirCreate(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            Directory.CreateDirectory(runtime.GetAt<string>(1));
            return null;
        }

        private object DirGetFileList(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return Directory.GetFiles(runtime.GetAt<string>(1));
        }

        private object DirGetDirList(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return Directory.GetDirectories(runtime.GetAt<string>(1));
        }

        private object FileReadAll(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            return File.ReadAllText(runtime.GetAt<string>(1));
        }

        private object FileWriteAll(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            string path = runtime.GetAt<string>(1);
            object data = runtime.GetAt(2);

            string sData = data as string;
            if (sData != null) {
                File.WriteAllText(path, sData);
                return null;
            }

            string[] saData = data as string[];
            if (saData != null) {
                File.WriteAllLines(path, saData);
                return null;
            }

            byte[] bData = data as byte[];
            if (bData != null) {
                File.WriteAllBytes(path, bData);
                return null;
            }

            runtime.Status = ErrorSuccess.Warnings; // data is written, but not necessarily useful
            File.WriteAllText(path, data + "");
            return null;
        }

        /// <summary>
        /// Moves a file to the recycle bin
        /// </summary>
        /// <param name="path">the path of the file</param>
        public static void Recycle(string path)
        {
            FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }

        private object Recycle(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            Recycle(runtime.GetAt<string>(1));
            return null;
        }

        private object FileGetAttributes(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            string path = runtime.GetAt<string>(1);
            FileAttributes current = File.GetAttributes(path);
            return GetStringFromAttributes(current);
        }

        private object FileSetAttributes(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            string path = runtime.GetAt<string>(1);
            FileAttributes current = File.GetAttributes(path);
            if ((current & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) {
                File.SetAttributes(path, current & ~FileAttributes.ReadOnly);
            }
            FileAttributes attributes = GetAttributesFromString(runtime.GetAt<string>(2));
            File.SetAttributes(path, attributes);
            return null;
        }

        private static string GetStringFromAttributes(FileAttributes attributes)
        {
            StringBuilder sb = new StringBuilder();
            if ((attributes & FileAttributes.Archive) == FileAttributes.Archive) { sb.Append("a"); }
            if ((attributes & FileAttributes.Compressed) == FileAttributes.Compressed) { sb.Append("c"); }
            if ((attributes & FileAttributes.Encrypted) == FileAttributes.Encrypted) { sb.Append("e"); }
            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden) { sb.Append("h"); }
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) { sb.Append("r"); }
            if ((attributes & FileAttributes.System) == FileAttributes.System) { sb.Append("s"); }
            return sb.ToString();
        }

        private static FileAttributes GetAttributesFromString(string attributes)
        {
            FileAttributes result = new FileAttributes();
            foreach (char c in attributes.ToUpper()) {
                switch (c) {
                    case 'A': result = result | FileAttributes.Archive; break;
                    case 'C': result = result | FileAttributes.Compressed; break;
                    case 'E': result = result | FileAttributes.Encrypted; break;
                    case 'H': result = result | FileAttributes.Hidden; break;
                    case 'R': result = result | FileAttributes.ReadOnly; break;
                    case 'S': result = result | FileAttributes.System; break;
                    default:
                        throw new ArgumentException("Invalid attribute '" + c + "'");
                }
            }
            return result;
        }

        private object FileSetAccessDate(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            string path = runtime.GetAt<string>(1);
            try {
                if (File.Exists(path)) {
                    File.SetLastAccessTime(path, DateTime.Parse(runtime.GetAt<string>(2)));
                    return File.GetLastAccessTime(path).ToString();
                }
                else if (Directory.Exists(path)) {
                    Directory.SetLastAccessTime(path, DateTime.Parse(runtime.GetAt<string>(2)));
                    return Directory.GetLastAccessTime(path).ToString();
                }
                throw new FileNotFoundException();
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException) {
                throw new FunctionException(ErrorClient.NotFound, path, ex);
            }
        }

        private object FileSetModifiedDate(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            string path = runtime.GetAt<string>(1);
            try {
                if (File.Exists(path)) {
                    File.SetLastWriteTime(path, DateTime.Parse(runtime.GetAt<string>(2)));
                    return File.GetLastWriteTime(path).ToString();
                }
                else if (Directory.Exists(path)) {
                    Directory.SetLastWriteTime(path, DateTime.Parse(runtime.GetAt<string>(2)));
                    return Directory.GetLastWriteTime(path).ToString();
                }
                throw new FileNotFoundException();
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException) {
                throw new FunctionException(ErrorClient.NotFound, path, ex);
            }
        }

        private object FileSetCreatedDate(RuntimeData runtime)
        {
            runtime.AssertCount(3);
            string path = runtime.GetAt<string>(1);
            try {
                if (File.Exists(path)) {
                    File.SetCreationTime(path, DateTime.Parse(runtime.GetAt<string>(2)));
                    return File.GetCreationTime(path).ToString();
                }
                else if (Directory.Exists(path)) {
                    Directory.SetCreationTime(path, DateTime.Parse(runtime.GetAt<string>(2)));
                    return Directory.GetCreationTime(path).ToString();
                }
                throw new FileNotFoundException();
            }
            catch(Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException) {
                throw new FunctionException(ErrorClient.NotFound, path, ex);
            }
        }

        private object DirectoryGetCurrent(RuntimeData runtime)
        {
            runtime.AssertCount(1);
            return Directory.GetCurrentDirectory();
        }

        private object DirectorySetCurrent(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            Directory.SetCurrentDirectory(runtime.GetAt<string>(1));
            return null;
        }

        /// <summary>
        /// Executes a command in a hidden command prompt window and returns the exit code and output stream
        /// </summary>
        /// <param name="cmd">the command to execute</param>
        /// <param name="output">the data from the output stream</param>
        /// <param name="workingDir">the working directory of the command</param>
        /// <returns>command exit code</returns>
        public static int Shell(string cmd, string workingDir, out string output)
        {
            using (System.Diagnostics.Process console = new System.Diagnostics.Process()) {
                console.StartInfo.FileName = "cmd.exe";
                console.StartInfo.Arguments = "/c " + cmd;
                console.StartInfo.RedirectStandardOutput = true;
                console.StartInfo.RedirectStandardError = true;
                console.StartInfo.UseShellExecute = false;
                console.StartInfo.CreateNoWindow = true;
                console.StartInfo.WorkingDirectory = workingDir;
                console.Start();
                output = console.StandardOutput.ReadToEnd();
                console.WaitForExit();
                return console.ExitCode;
            }
        }

        /// <summary>
        /// Executes a command in a hidden command prompt window and returns the exit code and output stream
        /// </summary>
        /// <param name="cmd">the command to execute</param>
        /// <param name="output">the data from the output stream</param>
        /// <returns>command exit code</returns>
        public static int Shell(string cmd, out string output)
        {
            return Shell(cmd, Directory.GetCurrentDirectory(), out output);
        }

        private object Shell(RuntimeData runtime)
        {
            runtime.AssertCount(2);
            string output;
            runtime.Status = Shell(runtime.GetAt<string>(1), out output);
            return output;
        }
    }
}