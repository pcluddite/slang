# TBASIC
A simple and extensible and highly flexible scripting language written in C#.

This project is composed of several parts:

1. `tbasic.dll` - the heart of this application, is a library for a highly customizable BASIC interpreter. This library can be linked in any .NET project to create your own dialect of the language.
  Features of the library include:
  - A standard library of functions, macro definitions, code blocks and operators that can be loaded to get started quickly.
  - An API for adding new or redefining existing functions, code blocks and operators
  - The ability to control certain aspects of the parser.
2. `texecuter.exe` - an executable for running scripts using the standard library.
3. `TBasic Terminal.exe` - a console application for running single functions and statements for iteractive testing purposes.

A syntax file for Notepad++ as well as some example scripts in the "samples" directory.

The project is licensed under a modified BSD License for **NON-COMMERCIAL USE ONLY** unless a specific source file is labled otherwise. See LICENSE for details.
