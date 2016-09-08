// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
namespace TLang.Types
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
