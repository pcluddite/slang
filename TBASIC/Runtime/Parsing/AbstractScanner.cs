// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Runtime;
using Tbasic.Types;

namespace Tbasic.Parsing
{
    /// <summary>
    /// Parses text to various language symbols. Similar idea to java.util.Scanner (don't sue me Oracle)
    /// </summary>
    internal abstract class AbstractScanner : IScanner
    {
        protected Tuple<int, string> TokenBuffer = null;

        /// <summary>
        /// The internal buffer for this scanner
        /// </summary>
        protected string InternalBuffer;
        
        /// <summary>
        /// Gets a value indicating whether the end of the stream has been reached
        /// </summary>
        public bool EndOfStream
        {
            get {
                return Position >= InternalBuffer.Length;
            }
        }

        /// <summary>
        /// Gets or sets the current position of the stream as an integer
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets the length of this stream as an integer
        /// </summary>
        public int Length
        {
            get {
                return InternalBuffer.Length;
            }
        }

        public virtual int Current
        {
            get { return CharAt(Position); }
        }

        protected virtual int CharAt(int pos)
        {
            if (EndOfStream)
                return -1;
            return InternalBuffer[pos];
        }
        
        /// <summary>
        /// Skips all leading whitespace
        /// </summary>
        public virtual void SkipWhiteSpace()
        {
            if (EndOfStream)
                return;
            Position = FindNonWhiteSpace();
        }

        protected virtual int FindNonWhiteSpace()
        {
            int pos = Position;
            while (pos < Length && char.IsWhiteSpace(InternalBuffer[pos])) {
                ++pos;
            }
            return pos;
        }

        /// <summary>
        /// Gets the next token from the buffer. If the next token is not buffered, then it will be.
        /// </summary>
        protected string BuffNextWord()
        {
            int start = FindNonWhiteSpace(),
                pos = start;
            if (TokenBuffer == null || TokenBuffer.Item1 != pos) {
                while (pos < Length && !char.IsWhiteSpace(InternalBuffer[pos])) {
                    ++pos;
                }
                if (pos - start > 0) {
                    TokenBuffer = new Tuple<int, string>(start, InternalBuffer.Substring(start, pos - start));
                }
                else {
                    return null;
                }
            }
            return TokenBuffer.Item2;
        }

        /// <summary>
        /// Advances the scanner to the end of the next token
        /// </summary>
        protected void AdvanceScanner(string matchedToken)
        {
            AdvanceScanner(matchedToken.Length);
        }

        /// <summary>
        /// Advances the scanner to the end of the next token
        /// </summary>
        protected void AdvanceScanner(int tokenLen)
        {
            Position = tokenLen + TokenBuffer.Item1;
        }

        /// <summary>
        /// Gets the next token in the buffer
        /// </summary>
        public virtual IEnumerable<char> Next()
        {
            return NextSegment();
        }

        /// <summary>
        /// Gets the next token in the buffer as a IEnumerable&lt;char&gt;
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<char> NextSegment()
        {
            string word = BuffNextWord();
            if (word != null) {
                AdvanceScanner(word);
            }
            return word;
        }
        
        /// <summary>
        /// Matches the next function string
        /// </summary>
        public abstract bool NextFunction(out IEnumerable<char> name, out IList<IEnumerable<char>> args);
        /// <summary>
        /// Matches the next variable string
        /// </summary>
        public abstract bool NextVariable(out IEnumerable<char> name, out IList<IEnumerable<char>> indices);

        internal static bool NextFunctionInternal(IScanner scanner, TBasic exec, out Function func)
        {
            IEnumerable<char> name;
            IList<IEnumerable<char>> args;
            if (scanner.NextFunction(out name, out args)) {
                func = new Function(exec, name.ToString(), args);
            }
            else {
                func = null;
            }
            return (func != null);
        }

        internal static bool NextVariable(IScanner scanner, TBasic exec, out Variable variable)
        {
            IEnumerable<char> name;
            IList<IEnumerable<char>> indices;
            if (scanner.NextVariable(out name, out indices)) {
                variable = new Variable(name.ToString(), indices, exec);
            }
            else {
                variable = null;
            }
            return (variable != null);
        }
        
        /// <summary>
        /// Converts this scanner's buffer to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return InternalBuffer.ToString();
        }

        public abstract bool NextStringOrToken(out IEnumerable<char> token);
        public abstract bool NextNumber(out Number num);
        public abstract bool NextHexadecimal(out long number);
        public abstract bool Next(string pattern, bool ignoreCase = true);
        public abstract bool NextString(out string parsed);
        public abstract bool SkipString();
        public abstract bool NextGroup(out IList<IEnumerable<char>> args);
        public abstract bool SkipGroup();
        public abstract bool NextIndices(out IList<IEnumerable<char>> indices);
        public abstract bool NextBool(out bool b);
        public abstract bool NextBinaryOp(ObjectContext context, out BinaryOperator foundOp);
        public abstract bool NextUnaryOp(ObjectContext context, object last, out UnaryOperator foundOp);
        public abstract bool NextValidIdentifier(out IEnumerable<char> name);

        public abstract IScanner Scan(IEnumerable<char> buffer);

        public virtual IEnumerable<char> Range(int start, int count)
        {
            return InternalBuffer.Substring(start, count);
        }

        public virtual IEnumerable<char> Range(int start)
        {
            return InternalBuffer.Substring(start);
        }

        public void Skip(int count)
        {
            Position += count;
        }
    }
}
