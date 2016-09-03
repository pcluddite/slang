// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;
using Tbasic.Runtime;
using Tbasic.Types;

namespace Tbasic.Parsing
{
    /// <summary>
    /// An inteface for a TBasic scanner
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
        /// Skips all leading whitespace
        /// </summary>
        void SkipWhiteSpace();
        /// <summary>
        /// Gets the next token in the buffer as a string
        /// </summary>
        IEnumerable<char> Next();
        /// <summary>
        /// Parses the next escaped string or token
        /// </summary>
        bool NextStringOrToken(out IEnumerable<char> token);
        /// <summary>
        /// Gets the next unsigned number in the buffer
        /// </summary>
        bool NextNumber(out Number num);
        /// <summary>
        /// Gets the next hexadecimal value in the buffer
        /// </summary>
        bool NextHexadecimal(out long number);
        /// <summary>
        /// Matches the next string in the buffer
        /// </summary>
        bool Next(string pattern, bool ignoreCase = true);
        /// <summary>
        /// When implemented in a derived class, matches the next string in the buffer
        /// </summary>
        bool NextString(out string parsed);
        /// <summary>
        /// When implemented in a derived class, sets the position of the stream to the last index of the string
        /// </summary>
        bool SkipString();
        /// <summary>
        /// When implemented in a derived class, matches the next group in the buffer.
        /// </summary>
        bool NextGroup(out IList<IEnumerable<char>> args);
        /// <summary>
        /// When implemented in a derived class, sets the position of the stream to the last index of the group
        /// </summary>
        bool SkipGroup();
        /// <summary>
        /// Matches the next indices for an array
        /// </summary>
        bool NextIndices(out IList<IEnumerable<char>> indices);
        /// <summary>
        /// Matches the next boolean
        /// </summary>
        bool NextBool(out bool b);
        /// <summary>
        /// Matches the next binary operator
        /// </summary>
        bool NextBinaryOp(ObjectContext context, out BinaryOperator foundOp);
        /// <summary>
        /// Matches the next unary operator
        /// </summary>
        bool NextUnaryOp(ObjectContext context, object last, out UnaryOperator foundOp);
        /// <summary>
        /// Matches the next function string
        /// </summary>
        bool NextFunction(out IEnumerable<char> name, out IList<IEnumerable<char>> args);
        /// <summary>
        /// Matches the next variable string
        /// </summary>
        bool NextVariable(out IEnumerable<char> name, out IList<IEnumerable<char>> indices);
        /// <summary>
        /// Matches the next set of characters that are acceptable in an identifier (such as a variable or function)
        /// </summary>
        bool NextValidIdentifier(out IEnumerable<char> name);
        /// <summary>
        /// Returns a new scanner scanning a different buffer
        /// </summary>
        IScanner Scan(IEnumerable<char> buffer);
        /// <summary>
        /// Takes a range of characters from the buffer
        /// </summary>
        IEnumerable<char> Range(int start, int count);
        /// <summary>
        /// Takes a range of characters from the buffer
        /// </summary>
        IEnumerable<char> Range(int start);
        /// <summary>
        /// Advances the scanner a given number of characters
        /// </summary>
        void Skip(int count);
    }
}
