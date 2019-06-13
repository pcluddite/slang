/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;

namespace Tbasic.Types
{
	public partial struct Number 
	{
        /// <summary>
        /// Adds one number to another number
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns>a Number with the resulting value</returns>
        public static Number operator +(Number left, Number right)
        {
            return new Number(left.Value + right.Value);
        }
        /// <summary>
        /// Subtracts one number from another number
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns>a Number with the resulting value</returns>
        public static Number operator -(Number left, Number right)
        {
            return new Number(left.Value - right.Value);
        }
        /// <summary>
        /// Multiplies one number by another number
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns>a Number with the resulting value</returns>
        public static Number operator *(Number left, Number right)
        {
            return new Number(left.Value * right.Value);
        }
        /// <summary>
        /// Divides one number by another number
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns>a Number with the resulting value</returns>
        public static Number operator /(Number left, Number right)
        {
            return new Number(left.Value / right.Value);
        }
        /// <summary>
        /// Performs a modulus operation on one number using another number
        /// </summary>
        /// <param name="left">the left operand</param>
        /// <param name="right">the right operand</param>
        /// <returns>a Number with the resulting value</returns>
        public static Number operator %(Number left, Number right)
        {
            return new Number(left.Value % right.Value);
        }
	}
}

