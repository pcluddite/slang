# T Language Interpreter
A simple and extensible and highly flexible scripting language written in C#.

This project is composed of several parts:

1. `tlang.dll` - the heart of this application, is a library for a highly customizable scripting language interpreter. This library can be linked in any .NET project to create your own dialect of the language.
  Features of the library include:
  - A standard library of functions, macro definitions, code blocks and operators that can be loaded to get started quickly.
  - An API for adding new or redefining existing functions, code blocks and operators
  - Modify functionality for syntax and parsing
2. `TExecuter.exe` - an executable for running scripts using the standard library.
3. `TTerminal.exe` - a console application for running single functions and statements for iteractive testing purposes.

A syntax file for the default BASIC parser is available for Notepad++ located in tlang/samples

The project is licensed under a modified BSD License for **NON-COMMERCIAL USE ONLY** unless a specific source file is labled otherwise. See LICENSE for details.
