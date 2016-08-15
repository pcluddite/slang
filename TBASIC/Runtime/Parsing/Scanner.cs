// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.IO;
using System.Text;
using Tbasic.Components;
using Tbasic.Operators;
using Tbasic.Runtime;

namespace Tbasic.Parsing
{
    /// <summary>
    /// Similar idea to java.util.Scanner (don't sue me Oracle)
    /// </summary>
    internal abstract class Scanner : Stream
    {
        protected StringSegment InternalBuffer;

        public override bool CanRead
        {
            get {
                return true;
            }
        }

        public override bool CanSeek
        {
            get {
                return true;
            }
        }

        public override bool CanWrite
        {
            get {
                return false;
            }
        }

        public override long Length
        {
            get {
                return InternalBuffer.Length;
            }
        }

        public override long Position
        {
            get {
                return IntPosition;
            }
            set {
                IntPosition = Convert.ToInt32(value);
            }
        }

        public bool EndOfStream
        {
            get {
                return IntPosition >= InternalBuffer.Length;
            }
        }

        public int IntPosition { get; set; }
        
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

        public virtual string Next()
        {
            return NextSegment().ToString();
        }

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

        public abstract bool NextPositiveInt(out int integer);
        public abstract bool NextPositiveNumber(out Number num);
        public abstract bool NextHexadecimal(out int number);
        public abstract bool Next(string pattern, bool ignoreCase = true);
        public abstract bool NextString(out string parsed);
        public abstract bool NextFunction(Executer exec, out Function func);
        public abstract bool NextVariable(Executer exec, out Variable variable);
        public abstract bool NextIndices(Executer exec, out int[] indices);
        public abstract bool NextBool(out bool b);
        public abstract bool NextBinaryOp(BinOpDictionary _binOps, out BinaryOperator foundOp);
        public abstract bool NextUnaryOp(UnaryOpDictionary _unOps, object last, out UnaryOperator foundOp);

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

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            // no-op
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (Position + offset >= Length)
                return 0;
            return Encoding.Unicode.GetBytes(InternalBuffer.ToString(), Convert.ToInt32(Position), count, buffer, offset);
        }
    }
}
