# TBASIC
A simple and extensible scripting language written in C#.

The project is licensed under a modified BSD License for **NON-COMMERCIAL USE ONLY** unless a specific source file is labled otherwise. See LICENSE for details.

This project contains two parts:

1. **tbasic.dll** is a library for a customizable BASIC interpreter. Add this library to your .NET application for a customizable BASIC-style language. Customizations include being able to define your own more complex functions, code blocks, variables, constants and even operators. The library uses a simple api that is documented (possibly poorly) that your project can use.
2. **texecuter.exe** is an executable that loads all standard functions, macros, and code blocks from tbasic.dll, and then runs a script. The standard library contains some common user input and output functions as well as functions that facilitate keyboard and mouse manipulation, reading and writing to files the registry, terminating and starting processes, and more.

A syntax file is available for Notepad++ in the "samples" directory.
