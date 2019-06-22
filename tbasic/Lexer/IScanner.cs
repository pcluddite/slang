/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Lexer.Tokens;

namespace Tbasic.Lexer
{
    /// <summary>
    /// An inteface for a Tbasic scanner
    /// </summary>
    public interface IScanner
    {
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
        /// Gets the next token in the buffer
        /// </summary>
        IToken Next();
        /// <summary>
        /// Returns a new scanner scanning a different buffer
        /// </summary>
        IScanner Scan(StringStream buffer);
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
