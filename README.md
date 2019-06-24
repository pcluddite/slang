# The S Language Interpreter (Slang)
A powerful and highly flexible scripting language interpreter written in C#. The Slang Interpreter (or simply "Slang") can be included in any .NET project to create specialized scripting dialects, defining your own operators, code blocks, functions, and syntax.
The name comes from the fact that TBASIC, the default dialect, is built on it, and S comes before T.

This project is composed of several parts:

1. `slang.dll` - the heart of this application, is a library for a highly customizable scripting language interpreter. This library can be linked in any .NET project to create your own dialect of the language.
  Features of the library include:
  - A standard library of functions, macro definitions, code blocks and operators that can be loaded to get started quickly.
  - An API for adding new or redefining existing functions, code blocks and operators
  - Modify functionality for syntax and parsing
2. `tscript.exe` - an executable for running scripts using the standard library using the default dialect TBASIC.
3. `tish.exe` - T Interpreter Shell, runs commands in an interactive shell with TBASIC commands

A syntax file for the default dialect TBASIC is available for Notepad++ located in slang/samples
