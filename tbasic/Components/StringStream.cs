/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Tbasic.Components
{
    public class StringStream : Stream
    {
        internal StringSegment Value { get; private set; }
        private int pos = 0;

        public StringStream(string str)
        {
            Value = new StringSegment(str);
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => Value.Length;

        public int Peek()
        {
            if (pos >= Value.Length)
                return -1;
            return Value[pos];
        }

        public override long Position { get => pos; set => pos = (int)value; }

        public override void Flush()
        {
        }

        public int Read()
        {
            if (pos >= Value.Length)
                return -1;
            return Value[pos++];
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset >= (uint)buffer.Length)
                throw new IndexOutOfRangeException();
            Contract.EndContractBlock();

            int size = 0;
            if (pos >= Value.Length)
                return -1;

            for(int c;
                offset < count && (c = Read()) > -1;
                offset += sizeof(char), ++size)
            {
                char cur = (char)c;
                unsafe {
                    buffer[offset] = *(byte*)&cur;
                    buffer[offset + 1] = *((byte*)&cur + 1);
                }
            }

            return size;
        }

        public string ReadWord()
        {
            int c;
            while ((c = Read()) != -1 && char.IsWhiteSpace((char)c)) ;
            if (c == -1)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            do {
                sb.Append((char)c);
            }
            while ((c = Read()) != -1 && !char.IsWhiteSpace((char)c));
            return sb.ToString();
        }

        public unsafe int Read(char[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset >= (uint)buffer.Length)
                throw new IndexOutOfRangeException();
            Contract.EndContractBlock();

            int nIdx = pos, nLen = Value.Length;
            string fullStr = Value.FullString;

            if (pos >= nLen)
                return -1;

            fixed (char* lpFull = fullStr, lpBuff = buffer) {
                char* lpStr = &lpFull[pos];
                for (; nIdx < count && nIdx < nLen; ++nIdx)
                    lpBuff[offset++] = lpStr[nIdx];
            }
            pos = nIdx - 1;
            return nIdx;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            StringSegment val = Value;
            int newPos;
            switch(origin) {
                case SeekOrigin.Begin: newPos = (int)offset; break;
                case SeekOrigin.Current: newPos = pos + (int)offset; break;
                case SeekOrigin.End: newPos = pos + (int)offset; break;
                default:
                    throw new ArgumentException(nameof(origin));
            }
            if ((uint)newPos >= (uint)val.Length)
                throw new IndexOutOfRangeException();
            return pos = newPos;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
