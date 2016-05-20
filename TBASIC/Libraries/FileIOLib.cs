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
    public class FileIOLib : Library
    {

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public FileIOLib()
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

        private void DirExists(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            _sframe.Data = Directory.Exists(_sframe.Get<string>(1));
        }

        private void FileExists(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            _sframe.Data = File.Exists(_sframe.Get<string>(1));
        }

        private void FileMove(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            File.Move(_sframe.Get<string>(1), _sframe.Get<string>(2));
        }

        private void FileCopy(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            File.Copy(_sframe.Get<string>(1), _sframe.Get<string>(2));
        }

        private void FileDelete(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            File.Delete(_sframe.Get<string>(1));
        }

        private void DirDelete(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            Directory.Delete(_sframe.Get<string>(1));
        }

        private void DirMove(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            Directory.Move(_sframe.Get<string>(1), _sframe.Get<string>(2));
        }

        private void DirCreate(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            Directory.CreateDirectory(_sframe.Get<string>(1));
        }

        private void DirGetFileList(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            _sframe.Data = Directory.GetFiles(_sframe.Get<string>(1));
        }

        private void DirGetDirList(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            _sframe.Data = Directory.GetDirectories(_sframe.Get<string>(1));
        }

        private void FileReadAll(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            _sframe.Data = File.ReadAllText(_sframe.Get<string>(1));
        }

        private void FileWriteAll(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            string path = _sframe.Get<string>(1);
            object data = _sframe.Get(2);

            string sData = data as string;
            if (sData != null)
                File.WriteAllText(path, sData);

            string[] saData = data as string[];
            if (saData != null)
                File.WriteAllLines(path, saData);

            byte[] bData = data as byte[];
            if (bData != null)
                File.WriteAllBytes(path, bData);

            _sframe.Status = ErrorSuccess.Warnings; // data is written, but not necessarily useful
            File.WriteAllText(path, data + "");
        }

        /// <summary>
        /// Moves a file to the recycle bin
        /// </summary>
        /// <param name="path">the path of the file</param>
        public static void Recycle(string path)
        {
            FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }

        private void Recycle(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            Recycle(_sframe.Get<string>(1));
        }

        private void FileGetAttributes(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            string path = _sframe.Get<string>(1);
            FileAttributes current = File.GetAttributes(path);
            _sframe.Data = GetStringFromAttributes(current);
        }

        private void FileSetAttributes(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            string path = _sframe.Get<string>(1);
            FileAttributes current = File.GetAttributes(path);
            if ((current & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) {
                File.SetAttributes(path, current & ~FileAttributes.ReadOnly);
            }
            FileAttributes attributes = GetAttributesFromString(_sframe.Get<string>(2));
            File.SetAttributes(path, attributes);

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

        private void FileSetAccessDate(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            string path = _sframe.Get<string>(1);
            try {
                if (File.Exists(path)) {
                    File.SetLastAccessTime(path, DateTime.Parse(_sframe.Get<string>(2)));
                    _sframe.Data = File.GetLastAccessTime(path).ToString();
                }
                else if (Directory.Exists(path)) {
                    Directory.SetLastAccessTime(path, DateTime.Parse(_sframe.Get<string>(2)));
                    _sframe.Data = Directory.GetLastAccessTime(path).ToString();
                }
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException) {
                throw new CustomException(ErrorClient.NotFound, path, ex);
            }
        }

        private void FileSetModifiedDate(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            string path = _sframe.Get<string>(1);
            try {
                if (File.Exists(path)) {
                    File.SetLastWriteTime(path, DateTime.Parse(_sframe.Get<string>(2)));
                    _sframe.Data = File.GetLastWriteTime(path).ToString();
                }
                else if (Directory.Exists(path)) {
                    Directory.SetLastWriteTime(path, DateTime.Parse(_sframe.Get<string>(2)));
                    _sframe.Data = Directory.GetLastWriteTime(path).ToString();
                }
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException) {
                throw new CustomException(ErrorClient.NotFound, path, ex);
            }
        }

        private void FileSetCreatedDate(Parameters _sframe)
        {
            _sframe.AssertArgs(3);
            string path = _sframe.Get<string>(1);
            try {
                if (File.Exists(path)) {
                    File.SetCreationTime(path, DateTime.Parse(_sframe.Get<string>(2)));
                    _sframe.Data = File.GetCreationTime(path).ToString();
                }
                else if (Directory.Exists(path)) {
                    Directory.SetCreationTime(path, DateTime.Parse(_sframe.Get<string>(2)));
                    _sframe.Data = Directory.GetCreationTime(path).ToString();
                }
            }
            catch(Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException) {
                throw new CustomException(ErrorClient.NotFound, path, ex);
            }
        }

        private void DirectoryGetCurrent(Parameters _sframe)
        {
            _sframe.AssertArgs(1);
            _sframe.Data = Directory.GetCurrentDirectory();
        }

        private void DirectorySetCurrent(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            Directory.SetCurrentDirectory(_sframe.Get<string>(1));
        }

        /// <summary>
        /// Executes a command in a hidden command prompt window and returns the exit code and output stream
        /// </summary>
        /// <param name="cmd">the command to execute</param>
        /// <param name="output">the data from the output stream</param>
        /// <returns>command exit code</returns>
        public static int Shell(string cmd, out string output)
        {
            using (System.Diagnostics.Process console = new System.Diagnostics.Process()) {
                console.StartInfo.FileName = "cmd.exe";
                console.StartInfo.Arguments = "/c " + cmd;
                console.StartInfo.RedirectStandardOutput = true;
                console.StartInfo.RedirectStandardError = true;
                console.StartInfo.UseShellExecute = false;
                console.StartInfo.CreateNoWindow = true;
                console.Start();
                output = console.StandardOutput.ReadToEnd();
                console.WaitForExit();
                return console.ExitCode;
            }
        }

        private void Shell(Parameters _sframe)
        {
            _sframe.AssertArgs(2);
            string output;
            _sframe.Status = Shell(_sframe.Get<string>(1), out output);
            _sframe.Data = output;
        }
    }
}