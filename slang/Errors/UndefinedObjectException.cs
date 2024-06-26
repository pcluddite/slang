﻿/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;

namespace Slang.Errors
{
    /// <summary>
    /// Thrown when a function, variable or other symbol is not defined within the current scope
    /// </summary>
    public class UndefinedObjectException : TbasicRuntimeException
    {
        /// <summary>
        /// Constructs a new UndefinedObjectException
        /// </summary>
        /// <param name="msg"></param>
        public UndefinedObjectException(string msg) : base(msg)
        {
        }
        
        /// <summary>
        /// Constructs a new UndefinedObjectException
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public UndefinedObjectException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
    }
}
