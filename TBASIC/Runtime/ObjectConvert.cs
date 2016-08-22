﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic.Runtime
{
    internal static class ObjectConvert
    {
        internal static bool TryConvert<T>(object obj, out T result, ExecuterOption opts)
        {
            return TryConvert(obj, out result, opts.HasFlag(ExecuterOption.Strict), !opts.HasFlag(ExecuterOption.EnforceStrings));
        }

        internal static bool TryConvert(object obj, Type type, out object result, ExecuterOption opts)
        {
            return TryConvert(obj, type, out result, opts.HasFlag(ExecuterOption.Strict), !opts.HasFlag(ExecuterOption.EnforceStrings));
        }

        /// <summary>
        /// Tries to convert an object to a given type. First this tries to do a straight up cast. If that doesn't work and strict is turned off, it will try to be converted with IConvertible. If the object is a string and parseStrings is turned on, it will try to parse that string.
        /// </summary>
        /// <typeparam name="T">the type to convert to</typeparam>
        /// <param name="obj">the object to convert</param>
        /// <param name="result">the result of the conversion</param>
        /// <param name="strict">wether the type should attempt to be converted</param>
        /// <param name="parseStrings">determines whether strings should try to be converted</param>
        /// <returns></returns>
        internal static bool TryConvert<T>(object obj, out T result, bool strict, bool parseStrings)
        {
            if (obj is T) {
                result = (T)obj;
                return true;
            }
            else {
                if (!strict)
                    return TryConvertNonStrict(obj, out result, parseStrings);
                result = default(T);
                return false;
            }
        }

        internal static bool TryConvert(object obj, Type type, out object result, bool strict, bool parseStrings)
        {
            if (type.IsAssignableFrom(obj.GetType())) {
                result = obj;
                return true;
            }
            else {
                if (!strict)
                    return TryConvertNonStrict(obj, type, out result, parseStrings);
                result = null;
                return false;
            }
        }

        private static bool TryConvertNonStrict<T>(object obj, out T result, bool parseStrings = true)
        {
            result = default(T);
            if (parseStrings) {
                string str = obj as string; // maybe we can convert it from a string?
                if (str != null) {
                    obj = ConvertFromString(str);
                    if (obj == null)
                        return false;
                    return TryConvert(obj, out result, strict: false, parseStrings: false); // it's a good old fashion type now. try again.
                }
            }
            if (obj is IConvertible) {
                try {
                    result = (T)Convert.ChangeType(obj, typeof(T));
                    return true;
                }
                catch(InvalidCastException) {
                    if (typeof(T).IsEnum) {
                        Number? n = obj as Number?;
                        if (n != null) {
                            return TryConvert((int)n, out result, strict: true, parseStrings: false); // if we don't turn on strict, we'll have infinite recursion 8/22/16
                        }
                    }
                    return false;
                }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException) {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        private static bool TryConvertNonStrict(object obj, Type type, out object result, bool parseStrings = true)
        {
            result = null;
            if (parseStrings) {
                string str = obj as string; // maybe we can convert it from a string?
                if (str != null) {
                    obj = ConvertFromString(str);
                    if (obj == null)
                        return false;
                    return TryConvert(obj, out result, strict: false, parseStrings: false); // it's a good old fashion type now. try again.
                }
            }
            if (obj is IConvertible) {
                try {
                    result = Convert.ChangeType(obj, type);
                    return true;
                }
                catch (Exception ex) when (ex is FormatException || ex is InvalidCastException || ex is OverflowException) {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        internal static object ConvertFromString(string str)
        {
            // now we've just got to parse the supported types until we find a match...
            Number n;
            if (Number.TryParse(str, out n))
                return n;
            bool b;
            if (bool.TryParse(str, out b))
                return b;
            return null;
        }
    }
}