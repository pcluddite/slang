﻿/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
using System;

namespace Tbasic.Operators
{
    /// <summary>
    /// Represents an operator that takes two operands
    /// </summary>
    public struct BinaryOperator : IOperator, IComparable<BinaryOperator>, IEquatable<BinaryOperator>
    {
        /// <summary>
        /// A delegate that represents the method which processes the operands
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns>the result of the operator</returns>
        public delegate object BinaryOpDelegate(object left, object right);

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString { get; private set; }

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence { get; private set; }

        /// <summary>
        /// Gets the method that processes the operands
        /// </summary>
        public BinaryOpDelegate ExecuteOperator { get; private set; }

        /// <summary>
        /// Creates a new BinaryOperator
        /// </summary>
        /// <param name="strOp">the string representation of the operator</param>
        /// <param name="precedence">the operator precedence. Lower precedence are processed first</param>
        /// <param name="doOp">the method that processes the operands</param>
        public BinaryOperator(string strOp, int precedence, BinaryOpDelegate doOp)
        {
            OperatorString = strOp.ToUpper();
            Precedence = precedence;
            ExecuteOperator = doOp;
        }
        
        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(BinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence;
        }

        /// <summary>
        /// Determines if two operators are equal
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator ==(BinaryOperator first, BinaryOperator second)
        {
            return Equals(first, second);
        }

        /// <summary>
        /// Determines if two operators are not equal
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator !=(BinaryOperator first, BinaryOperator second)
        {
            return !Equals(first, second);
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BinaryOperator? op = obj as BinaryOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ ExecuteOperator.GetHashCode();
        }
    }
}
