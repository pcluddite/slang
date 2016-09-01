// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic.Types
{
    /// <summary>
    /// Interface for numeric types
    /// </summary>
    public interface INumber : IComparable<INumber>, IEquatable<INumber>, IConvertible, IComparable
    {
        /// <summary>
        /// Converts this object to the primitive it represents
        /// </summary>
        object ToPrimitive();
        /// <summary>
        /// Returns a bool indicating whether or not this number has a fractional part
        /// </summary>
        /// <returns></returns>
        bool HasFraction();
        /// <summary>
        /// Converts this number to an int
        /// </summary>
        int ForceToInt();
        /// <summary>
        /// Returns a bool indicating whether or not this number is zero
        /// </summary>
        bool IsZero();

        /// <summary>
        /// Adds a number to this one
        /// </summary>
        INumber Add(INumber other);
        /// <summary>
        /// Subtracts a number from this one
        /// </summary>
        INumber Subtract(INumber other);
        /// <summary>
        /// Multiplies a number with this one
        /// </summary>
        INumber Multiply(INumber other);
        /// <summary>
        /// Divides this number by another
        /// </summary>
        INumber Divide(INumber other);
        /// <summary>
        /// Performs a modulus operation
        /// </summary>
        INumber Mod(INumber other);

        /// <summary>
        /// Determines if another number is greater than this one
        /// </summary>
        bool GreaterThan(INumber other);
        /// <summary>
        /// Determines if another number is less than this one
        /// </summary>
        bool LessThan(INumber other);
        /// <summary>
        /// Determines if another number is greater than or equal to this one
        /// </summary>
        bool GreaterThanOrEqualTo(INumber other);
        /// <summary>
        /// Determines if another number is less than or equal to this one
        /// </summary>
        bool LessThanOrEqualTo(INumber other);
    }
}
