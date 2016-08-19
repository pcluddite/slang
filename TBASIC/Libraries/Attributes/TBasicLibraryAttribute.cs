// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic.Libraries
{
    /// <summary>
    /// Indicates that this class is a library with tbasic functions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class TBasicLibraryAttribute : Attribute
    {
    }
}
