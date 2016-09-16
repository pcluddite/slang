// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======

namespace Tbasic.Types
{
    /// <summary>
    /// Supported types in Tbasic
    /// </summary>
    public enum TbasicType
    {
        /// <summary>
        /// Represents a number
        /// </summary>
        Number,
        /// <summary>
        /// Represents a true or false value
        /// </summary>
        Boolean,
        /// <summary>
        /// A sequence of characters
        /// </summary>
        String,
        /// <summary>
        /// Represents a function
        /// </summary>
        Function,
        /// <summary>
        /// Represents a value stored as a variable
        /// </summary>
        Variable,
        /// <summary>
        /// Represents an object
        /// </summary>
        Object,
        /// <summary>
        /// Represents an array
        /// </summary>
        Array,
        /// <summary>
        /// Represents a value from an enumeration
        /// </summary>
        EnumValue,
        /// <summary>
        /// A C# type
        /// </summary>
        Native,
        /// <summary>
        /// For internal use only
        /// </summary>
        Evaluator
    }
}
