/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License for
 * non-commercial use.
 *
 **/

namespace Tbasic.Operators
{
    /// <summary>
    /// Represents an operator
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// Represents this operator as a string
        /// </summary>
        string OperatorString { get; }
    }
}
