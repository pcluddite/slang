// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tbasic.Components;
using Tbasic.Operators;
using Tbasic.Runtime;
using System.Linq;

namespace Tbasic.Parsing
{
    /// <summary>
    /// A delegate for creating scanner objects
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public delegate Scanner CreateScannerDelegate(StringSegment buffer);

    /// <summary>
    /// Parses text to various language symbols. Similar idea to java.util.Scanner (don't sue me Oracle)
    /// </summary>
    public abstract class Scanner : Stream
    {
        /// <summary>
        /// Gets the default BASIC scanner
        /// </summary>
        public static readonly CreateScannerDelegate Default = (buff => new DefaultScanner(buff));

        /// <summary>
        /// Gets or sets the internal buffer for this scanner
        /// </summary>
        protected StringSegment InternalBuffer;

        /// <summary>
        /// Gets whether or not data can be read from this stream
        /// </summary>
        public override bool CanRead
        {
            get {
                return true;
            }
        }

        /// <summary>
        /// Gets whether this stream can seek
        /// </summary>
        public override bool CanSeek
        {
            get {
                return true;
            }
        }

        /// <summary>
        /// Gets whether this stream can write
        /// </summary>
        public override bool CanWrite
        {
            get {
                return false;
            }
        }

        /// <summary>
        /// Gets the length of this stream
        /// </summary>
        public override long Length
        {
            get {
                return InternalBuffer.Length;
            }
        }

        /// <summary>
        /// Gets or sets this stream's current position
        /// </summary>
        public override long Position
        {
            get {
                return IntPosition;
            }
            set {
                IntPosition = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the end of the stream has been reached
        /// </summary>
        public bool EndOfStream
        {
            get {
                return IntPosition >= InternalBuffer.Length;
            }
        }

        /// <summary>
        /// Gets or sets the current position of the stream as an integer
        /// </summary>
        public int IntPosition { get; set; }
        
        /// <summary>
        /// Skips all leading whitespace
        /// </summary>
        protected virtual void SkipWhiteSpace()
        {
            if (EndOfStream)
                return;
            if (char.IsWhiteSpace(InternalBuffer[IntPosition])) {
                do {
                    ++IntPosition;
                }
                while (char.IsWhiteSpace(InternalBuffer[IntPosition]));
            }
        }

        /// <summary>
        /// Gets the next string in the buffer
        /// </summary>
        /// <returns></returns>
        public virtual string Next()
        {
            return NextSegment().ToString();
        }

        /// <summary>
        /// Gets the next StringSegment in the buffer
        /// </summary>
        /// <returns></returns>
        internal virtual StringSegment NextSegment()
        {
            SkipWhiteSpace();

            int last = IntPosition;
            while (!EndOfStream && !char.IsWhiteSpace(InternalBuffer[last]))
                ++last;

            StringSegment seg = InternalBuffer.Subsegment(IntPosition, last - IntPosition + 1);
            IntPosition = last; // advance the stream
            return seg;
        }
        
        /// <summary>
        /// Gets the next unsigned number in the buffer
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public abstract bool NextUnsignedNumber(out Number num);
        /// <summary>
        /// Gets the next hexadecimal value in the buffer
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public abstract bool NextHexadecimal(out long number);
        /// <summary>
        /// Matches the next string in the buffer
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public abstract bool Next(string pattern, bool ignoreCase = true);
        /// <summary>
        /// Matches the next C-style string in the buffer
        /// </summary>
        /// <param name="parsed"></param>
        /// <returns></returns>
        public abstract bool NextString(out string parsed);
        /// <summary>
        /// Matches the next indices for an array
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public abstract bool NextIndices(Executer exec, out int[] indices);
        /// <summary>
        /// Matches the next boolean
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public abstract bool NextBool(out bool b);
        /// <summary>
        /// Matches the next binary operator
        /// </summary>
        /// <param name="context"></param>
        /// <param name="foundOp"></param>
        /// <returns></returns>
        public abstract bool NextBinaryOp(ObjectContext context, out BinaryOperator foundOp);
        /// <summary>
        /// Matches the next unary operator
        /// </summary>
        /// <param name="context"></param>
        /// <param name="last"></param>
        /// <param name="foundOp"></param>
        /// <returns></returns>
        public abstract bool NextUnaryOp(ObjectContext context, object last, out UnaryOperator foundOp);
        /// <summary>
        /// Matches the next function string
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="func"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public abstract bool NextFunction(Executer exec, out StringSegment name, out StringSegment func, out IList<object> args);
        /// <summary>
        /// Matches the next variable string
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="variable"></param>
        /// <param name="name"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public abstract bool NextVariable(Executer exec, out StringSegment variable, out StringSegment name, out int[] indices);

        internal bool NextFunctionInternal(Executer exec, out Function func)
        {
            StringSegment funcstr;
            StringSegment name;
            IList<object> args;
            if (!NextFunction(exec, out name, out funcstr, out args)) {
                func = null;
                return false;
            }
            func = new Function(funcstr, exec, name, args);
            return true;
        }

        internal bool NextVariableInternal(Executer exec, out Variable variable)
        {
            StringSegment varstr;
            StringSegment name;
            int[] indices;
            if (!NextVariable(exec, out varstr, out name, out indices)) {
                variable = null;
                return false;
            }
            variable = new Variable(varstr, name, indices, exec);
            return true;
        }

        /// <summary>
        /// Sets the position to a given offset, then returns the new position
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin) {
                case SeekOrigin.Begin:
                    Position = offset;
                    return Position;
                case SeekOrigin.Current:
                    Position += offset;
                    return Position;
                case SeekOrigin.End:
                    Position = Length - 1 + offset;
                    return Position;
            }
            return 0;
        }

        /// <summary>
        /// Sets the length of this stream (not supported)
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes to this stream (not supported)
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Flushes this stream (no-op)
        /// </summary>
        public override void Flush()
        {
            // no-op
        }

        /// <summary>
        /// Reads a section of this stream as a byte array
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (Position + offset >= Length)
                return 0;
            return Encoding.Unicode.GetBytes(InternalBuffer.ToString(), IntPosition, count, buffer, offset);
        }
    }
}
