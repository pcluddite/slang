/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using Tbasic.Errors;
using Tbasic.Runtime;
using System.Globalization;

namespace Tbasic.Types
{
    public partial struct Number
    {
        /// <summary>
        /// Determines if this number has a fractional part
        /// </summary>
        /// <returns></returns>
        public bool HasFraction()
        {
            return Value % 1 != 0;
        }

        /// <summary>
        /// Converts this number to an integer
        /// </summary>
        /// <returns></returns>
        public int ToInt()
        {
            return (int)Value;
        }

        /// <summary>
        /// Converts this number to whichever primitive type seems most appropriate (either int or double)
        /// </summary>
        /// <returns></returns>
        public object ToPrimitive()
        {
            if (HasFraction())
                return Value;
            return ToInt();
        }

        /// <summary>
        /// Determines if an object can be stored in a Number
        /// </summary>
        /// <param name="o"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static bool IsNumber(object o, ExecuterOption opts)
        {
            return TypeUtil.TryConvert<double>(o, out _, opts);
        }

        /// <summary>
        /// Attempts to convert an object to a Number. Returns null if no conversion is possible.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static Number? AsNumber(object o, ExecuterOption opts)
        {
            if (o == null) {
                if (opts.HasFlag(ExecuterOption.NullIsZero)) {
                    return 0;
                }
                else {
                    return null;
                }
            }
            Number? n = o as Number?;
            if (n != null)
                return n;
            if (TypeUtil.TryConvert(o, out double d, opts)) {
                return d;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Converts an object to a Number
        /// </summary>
        public static Number Convert(object o, ExecuterOption opts)
        {
            Number? n = AsNumber(o, opts);
            if (n == null)
                throw ThrowHelper.InvalidTypeInExpression(o?.GetType().Name ?? "null", nameof(Number));
            return n.Value;
        }

        /// <summary>
        /// Implicitly converts a Number to an Integer
        /// </summary>
        /// <param name="n"></param>
        public static implicit operator Number(int n)
        {
            return new Number(n);
        }

        /// <summary>
        /// Converts this Number to an Integer
        /// </summary>
        /// <param name="n"></param>
        public static explicit operator int(Number n)
        {
            if (n.HasFraction())
                throw new InvalidCastException("Number contains a fractional part");
            return (int)n.Value;
        }

        /// <summary>
        /// Converts this Number to a long
        /// </summary>
        /// <param name="n"></param>
        public static explicit operator long(Number n)
        {
            if (n.HasFraction())
                throw new InvalidCastException("Number contains a fractional part");
            return (long)n.Value;
        }

        /// <summary>
        /// Converts this number to a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (HasFraction())
                return Value.ToString();
            return ((long)Value).ToString();
        }
    }
}
