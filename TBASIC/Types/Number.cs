// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tbasic.Runtime;

namespace Tbasic.Types
{
    /// <summary>
    /// This is a crutch class as we transition from using Number
    /// </summary>
    internal static class Number
    {
        /// <summary>
        /// Tries to parse a string as a number
        /// </summary>
        public static bool TryParse(string s, out INumber result, ExecuterOption opts)
        {
            if (!opts.HasFlag(ExecuterOption.PreciseNumbers)) {
                FastNumber fn;
                if (FastNumber.TryParse(s, out fn)) {
                    result = fn;
                    return true;
                }
                else {
                    result = null;
                    return false;
                }
            }
            else {
                PreciseNumber pn;
                if (PreciseNumber.TryParse(s, out pn)) {
                    result = pn;
                    return true;
                }
                else {
                    result = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Parses a string as a number
        /// </summary>
        public static INumber Parse(string s, bool fast)
        {
            if (fast) {
                return FastNumber.Parse(s);
            }
            else {
                return PreciseNumber.Parse(s);
            }
        }
        
        public static INumber AsNumber(object o, ExecuterOption opts)
        {
            if (!opts.HasFlag(ExecuterOption.PreciseNumbers)) {
                return FastNumber.AsFastNumber(o, opts);
            }
            else {
                return PreciseNumber.AsPreciseNumber(o, opts);
            }
        }

        public static int GetSize(ExecuterOption options)
        {
            if (options.HasFlag(ExecuterOption.PreciseNumbers)) {
                return PreciseNumber.SIZE;
            }
            else {
                return FastNumber.SIZE;
            }
        }

        public static INumber Convert(object o, ExecuterOption opts)
        {
            if (!opts.HasFlag(ExecuterOption.PreciseNumbers)) {
                return FastNumber.AsFastNumber(o, opts);
            }
            else {
                return PreciseNumber.AsPreciseNumber(o, opts);
            }
        }
    }
}
