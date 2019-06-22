﻿/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Lexer.Tokens;
using Tbasic.Runtime;

namespace Tbasic.Lexer
{
    /// <summary>
    /// An inteface for a Tbasic scanner
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Gets the current scope
        /// </summary>
        ObjectContext Scope { get; }
        /// <summary>
        /// Gets or sets the current position of the scanner
        /// </summary>
        int Position { get; set; }
        /// <summary>
        /// Gets the length of the buffer
        /// </summary>
        int Length { get; }
        /// <summary>
        /// Gets a value indicating whether the end of the scanner has been reached
        /// </summary>
        bool EndOfStream { get; }
        /// <summary>
        /// Converts the current stream into tokens
        /// </summary>
        /// <returns></returns>
        IToken[] Tokenize();
        /// <summary>
        /// Gets the next token in the buffer
        /// </summary>
        IToken Next();
        /// <summary>
        /// Returns a new scanner scanning a different buffer
        /// </summary>
        IScanner Scan(StringStream stream);
        /// <summary>
        /// Advances the scanner a given number of characters
        /// </summary>
        void Skip(int count);
        /// <summary>
        /// Registers a token factory for parsing tokens
        /// </summary>
        void RegisterToken<T>() where T : ITokenFactory;
    }
}
