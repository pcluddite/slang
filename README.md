# Tint
A powerful and highly flexible scripting language interpreter written in C#. Tint (or the "T Interpreter") can be included in any .NET project to create specialized dialects, defining your own operators, code blocks, functions, and syntax.

This project is composed of several parts:

1. `tintlib.dll` - the heart of this application, is a library for a highly customizable scripting language interpreter. This library can be linked in any .NET project to create your own dialect of the language.
  Features of the library include:
  - A standard library of functions, macro definitions, code blocks and operators that can be loaded to get started quickly.
  - An API for adding new or redefining existing functions, code blocks and operators
  - Modify functionality for syntax and parsing
2. `tscript.exe` - an executable for running scripts using the standard library.
3. `tish.exe` - T Interpreter Shell, runs m

A syntax file for the default Tint dialect TBASIC is available for Notepad++ located in tint/samples

The project is licensed under a modified BSD License for **NON-COMMERCIAL USE ONLY** unless a specific source file is labled otherwise. See LICENSE for details.
