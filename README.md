# TBASIC
A simple and extensible scripting language written in C#.

The project is licensed under the BSD Simplified License for NON-COMMERCIAL USE ONLY unless a specific source file is labled otherwise. See LICENSE for details.

This project contains two parts:

1. tbasic.dll is a library for a customizable BASIC interpreter. Add this library to yours for a customizable BASIC-style language. Customizations include being able to define your own more complex functions, code blocks, variables, constants and even operators. The library uses a simple api that is documented (possibly poorly) that your project can use.
2. texecuter.exe is an executable that loads the standard library and runs a script to get started right away.

A syntax file is available for Notepad++ in the "samples" directory.

***Special thanks to:***
- **drdandle** for registry operations located in RegistryUtilities.cs (http://www.codeproject.com/Articles/16343/Copy-and-Rename-Registry-Keys)
