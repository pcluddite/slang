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
using Tbasic.Runtime;
using Tbasic.Types;

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
                return IntLength;
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
        /// Gets the length of this stream as an integer
        /// </summary>
        public int IntLength
        {
            get {
                return InternalBuffer.Length;
            }
        }
        
        /// <summary>
        /// Skips all leading whitespace
        /// </summary>
        public virtual void SkipWhiteSpace()
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
        /// Gets the next token in the buffer as a string
        /// </summary>
        /// <returns></returns>
        public virtual string Next()
        {
            return NextSegment().ToString();
        }

        /// <summary>
        /// Parses the next escaped string or token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public abstract bool NextStringOrToken(out StringSegment token);

        /// <summary>
        /// Gets the next token in the buffer as a StringSegment
        /// </summary>
        /// <returns></returns>
        public virtual StringSegment NextSegment()
        {
            SkipWhiteSpace();

            int last = IntPosition;

            while (last < InternalBuffer.Length && !char.IsWhiteSpace(InternalBuffer[last])) {
                ++last;
            }

            StringSegment seg = InternalBuffer.Subsegment(IntPosition, last - IntPosition);
            IntPosition = last; // advance the stream
            return seg;
        }
        
        /// <summary>
        /// Gets the next unsigned number in the buffer
        /// </summary>
        /// <param name="num"></param>
        /// <param name="fast">whether or not this is a fast number</param>
        /// <returns></returns>
        public abstract bool NextUnsignedNumber(out INumber num, bool fast);
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
        /// When implemented in a derived class, matches the next string in the buffer
        /// </summary>
        public abstract bool NextString(out string parsed);
        /// <summary>
        /// When implemented in a derived class, sets the position of the stream to the last index of the string
        /// </summary>
        public abstract bool SkipString();
        /// <summary>
        /// When implemented in a derived class, matches the next group in the buffer.
        /// </summary>
        public abstract bool NextGroup(out IList<StringSegment> args);
        /// <summary>
        /// When implemented in a derived class, sets the position of the stream to the last index of the group
        /// </summary>
        public abstract bool SkipGroup();
        /// <summary>
        /// Matches the next indices for an array
        /// </summary>
        public abstract bool NextIndices(out StringSegment[] indices);
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
        /// <param name="func"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public abstract bool NextFunction(out StringSegment name, out StringSegment func, out IList<StringSegment> args);
        /// <summary>
        /// Matches the next variable string
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="name"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public abstract bool NextVariable(out StringSegment variable, out StringSegment name, out StringSegment[] indices);
        /// <summary>
        /// Matches the next set of characters that are acceptable in an identifier (such as a variable or function)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool NextValidIdentifier(out StringSegment name);

        internal bool NextFunctionInternal(TBasic exec, out Function func)
        {
            StringSegment funcstr;
            StringSegment name;
            IList<StringSegment> args;
            if (!NextFunction(out name, out funcstr, out args)) {
                func = null;
                return false;
            }
            func = new Function(funcstr, exec, name, args);
            return true;
        }

        internal bool NextVariableInternal(TBasic exec, out Variable variable)
        {
            StringSegment varstr;
            StringSegment name;
            StringSegment[] indices;
            if (!NextVariable(out varstr, out name, out indices)) {
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
            return Encoding.Unicode.GetBytes(InternalBuffer.ToString(), IntPosition, count, buffer, offset);
        }

        /// <summary>
        /// Reads a string segment from the buffer
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public StringSegment Read(int offset, int count)
        {
            return InternalBuffer.Subsegment(offset, count);
        }

        /// <summary>
        /// Converts this scanner's buffer to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return InternalBuffer.ToString();
        }

        /// <summary>
        /// Converts this scanner's buffer to a string segment
        /// </summary>
        /// <returns></returns>
        public StringSegment ToSegment()
        {
            return new StringSegment(InternalBuffer.FullString, InternalBuffer.Offset, InternalBuffer.Length);
        }
    }
}
