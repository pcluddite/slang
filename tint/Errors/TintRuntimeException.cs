// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tint.Errors
{
    /// <summary>
    /// Occours when an exception happens at runtime
    /// </summary>
    public class TintRuntimeException : Exception
    {
        /// <summary>
        /// Constructs a runtime exception
        /// </summary>
        /// <param name="msg"></param>
        public TintRuntimeException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Constructs a runtime exception
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public TintRuntimeException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        /// <summary>
        /// Wraps a CLR exception into a TintRuntimeException. If the exception cannot be wrapped, returns null
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static TintRuntimeException WrapException(Exception ex)
        {
            TintRuntimeException runEx = ex as TintRuntimeException;
            if (runEx != null)
                return runEx;
            if (ex is FormatException || ex is ArgumentException)
                return new ScriptParsingException(ex.Message, ex);
            if (ex is NotImplementedException || ex is InvalidOperationException || ex is InvalidCastException)
                return new TintRuntimeException(ex.Message, ex);
            if (ex is OverflowException)
                return new TintRuntimeException(ex.Message, ex);

            FunctionException funcEx = FunctionException.FromException(ex); // last resort, try to stuff it in a FunctionException
            if (funcEx != null)
                return funcEx;

            return null;
        }
    }
}
