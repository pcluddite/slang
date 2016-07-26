﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic.Operators
{
    /// <summary>
    /// Represents an operator that takes one operand
    /// </summary>
    public struct UnaryOperator : IOperator, IEquatable<UnaryOperator>
    {
        /// <summary>
        /// Side that the operator uses as its operand
        /// </summary>
        public enum OperandSide
        {
            /// <summary>
            /// The operand is on the left
            /// </summary>
            Left,
            /// <summary>
            /// The operand is on the right
            /// </summary>
            Right
        }

        /// <summary>
        /// A delegate that represents the method which processes the operand. 
        /// </summary>
        /// <param name="value">the operand</param>
        /// <returns>the result of the operator</returns>
        public delegate object UnaryOpDelegate(object value);

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString { get; private set; }

        /// <summary>
        /// Gets the method that processes the operands
        /// </summary>
        public UnaryOpDelegate ExecuteOperator { get; private set; }

        /// <summary>
        /// Gets the side that she operand should be on
        /// </summary>
        public OperandSide Side { get; private set; }

        /// <summary>
        /// Creates a new UnaryOperator
        /// </summary>
        /// <param name="strOp">the string representation of the operator</param>
        /// <param name="doOp">the method that processes the operand</param>
        /// <param name="side">the side that the operand is on</param>
        public UnaryOperator(string strOp, UnaryOpDelegate doOp, OperandSide side = OperandSide.Right)
        {
            OperatorString = strOp;
            ExecuteOperator = doOp;
            Side = side;
        }

        /// <summary>
        /// Determines if two UnaryOperators are equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(UnaryOperator other)
        {
            return OperatorString == other.OperatorString && ExecuteOperator == other.ExecuteOperator && Side == other.Side;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            UnaryOperator? op = obj as UnaryOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Determines if two operators are equal
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator ==(UnaryOperator first, UnaryOperator second)
        {
            return Equals(first, second);
        }

        /// <summary>
        /// Determines if two operators are not equal
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator !=(UnaryOperator first, UnaryOperator second)
        {
            return !Equals(first, second);
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ ExecuteOperator.GetHashCode() ^ Side.GetHashCode();
        }
    }
}
