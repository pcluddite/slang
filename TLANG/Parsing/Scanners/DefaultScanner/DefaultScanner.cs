// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TLang.Runtime;
using TLang.Types;
using TLang.Components;

namespace TLang.Parsing
{
    /// <summary>
    /// The default implementation of Scanner. This can be extended and modified for custom implementations.
    /// </summary>
    public partial class DefaultScanner : IScanner
    {
        /// <summary>
        /// A regular expression for matching numbers
        /// </summary>
        protected static readonly Regex rxNumeric = new Regex(@"^([+-]?\d+(\.\d+)?([Ee][+-]?\d+)?)", RegexOptions.Compiled);
        /// <summary>
        /// A regular expression for matching hexadecimal
        /// </summary>
        protected static readonly Regex rxHex = new Regex(@"^(0x([0-9a-fA-F]+))", RegexOptions.Compiled);
        /// <summary>
        /// A regular expression for matching identifiers (function, variable, class and other names)
        /// </summary>
        protected static readonly Regex rxId = new Regex(@"^((_|[a-zA-Z])\w+)", RegexOptions.Compiled);

        /// <summary>
        /// The buffered word. The first item is the index of the word, the second is the word itself.
        /// </summary>
        private ValueTuple<int, string> WordBuffer = new ValueTuple<int, string>(int.MinValue, default(string));
        /// <summary>
        /// The next buffered index of the next non whitespace character. The first item is the index of the stream when the character was buffered. The second is the index of the character.
        /// </summary>
        private ValueTuple<int, int> NonwsBuffer = new ValueTuple<int, int>(int.MinValue, default(int));

        /// <summary>
        /// Gets the string that separates expressions
        /// </summary>
        protected virtual string ExpressionBreak { get; } = ";";
        /// <summary>
        /// Gets the string that represents a null value
        /// </summary>
        protected virtual string NullString { get; } = "null";
        /// <summary>
        /// Gets the string that represents the beginning of a comment
        /// </summary>
        protected virtual string Comment { get; } = "#";

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

        /// <summary>
        /// Gets the character at the current position. If the position is out of bounds, -1 is returned
        /// </summary>
        public int Current
        {
            get { return CharAt(Position); }
        }

        /// <summary>
        /// Initializes a DefaultScanner with a given string buffer
        /// </summary>
        /// <param name="buffer"></param>
        public DefaultScanner(string buffer)
        {
            InternalBuffer = buffer;
        }

        /// <summary>
        /// Initializes a DefaultScanner without a buffer
        /// </summary>
        protected DefaultScanner()
        {
        }

        /// <summary>
        /// Gets a character at a given position. If the position is out of bounds, -1 is returned
        /// </summary>
        public int CharAt(int pos)
        {
            if (pos < 0 || pos >= InternalBuffer.Length)
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

        /// <summary>
        /// Finds the next instance of a non-whitespace character without advancing the reader
        /// </summary>
        protected int FindNonWhiteSpace()
        {
            if (Position != NonwsBuffer.Item1) {
                int pos = Position;
                while (pos < Length && char.IsWhiteSpace(InternalBuffer[pos])) {
                    ++pos;
                }
                NonwsBuffer = new ValueTuple<int, int>(Position, pos);
            }
            return NonwsBuffer.Item2;
        }

        /// <summary>
        /// Gets the next set of characters until the first instance of whitespace or the end of the is reached.
        /// </summary>
        protected string BuffNextWord()
        {
            int start = FindNonWhiteSpace(),
                pos = start;
            if (WordBuffer.Item1 != pos) {
                while (pos < Length && !char.IsWhiteSpace(InternalBuffer[pos])) {
                    ++pos;
                }
                if (pos - start > 0) {
                    WordBuffer = new ValueTuple<int, string>(start, InternalBuffer.Substring(start, pos - start));
                }
                else {
                    return null;
                }
            }
            return WordBuffer.Item2;
        }

        /// <summary>
        /// Advances the scanner to the end of the next token parsed from the buffered word
        /// </summary>
        protected void AdvanceScanner(string matchedToken)
        {
            AdvanceScanner(matchedToken.Length);
        }

        /// <summary>
        /// Advances the scanner to the end of the next token parsed from the buffered word
        /// </summary>
        protected void AdvanceScanner(int tokenLen)
        {
            Position = tokenLen + WordBuffer.Item1;
        }

        /// <summary>
        /// Gets the next word in the buffer
        /// </summary>
        public virtual IEnumerable<char> Next()
        {
            string word = BuffNextWord();
            if (word != null) {
                AdvanceScanner(word);
            }
            return word;
        }

        /// <summary>
        /// Tries to match the next line breaking character
        /// </summary>
        public virtual bool NextExpressionBreak()
        {
            return Next(ExpressionBreak, ignoreCase: true);
        }

        /// <summary>
        /// Tries to match the next token as a comment character
        /// </summary>
        /// <returns></returns>
        public bool NextComment()
        {
            return Next(Comment, ignoreCase: true);
        }

        /// <summary>
        /// Tries to match the next null value
        /// </summary>
        /// <returns></returns>
        public virtual bool NextNull()
        {
            return Next(NullString, ignoreCase: true);
        }

        /// <summary>
        /// Tries to match a number from the buffer and advances the reader to the end of that number
        /// </summary>
        public virtual bool NextNumber(out Number num)
        {
            Match m = rxNumeric.Match(BuffNextWord() ?? string.Empty);
            if (m.Success && Number.TryParse(m.Value, out num)) {
                AdvanceScanner(m.Value);
                return true;
            }
            else {
                num = default(Number);
                return false;
            }
        }

        /// <summary>
        /// Tries to match a hex number from the buffer and advances the reader to the end of that number
        /// </summary>
        public virtual bool NextHexadecimal(out long hex)
        {
            Match m = rxHex.Match(BuffNextWord() ?? string.Empty);
            if (m.Success) {
                hex = Convert.ToInt64(m.Value, 16);
                AdvanceScanner(m.Value);
                return true;
            }
            else {
                hex = default(long);
                return false;
            }
        }

        /// <summary>
        /// Tries to match a set of characeters from the buffer and advances the reader to the end of those characters
        /// </summary>
        public virtual bool Next(string str, bool ignoreCase)
        {
            string word = BuffNextWord();
            if (!string.IsNullOrEmpty(word) && word.StartsWith(str, ignoreCase, CultureInfo.CurrentCulture)) {
                AdvanceScanner(str);
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Tries to match a word or a quoted string from the buffer and advances the reader to the end of that string or word
        /// </summary>
        public virtual bool NextWordOrString(out IEnumerable<char> token)
        {
            token = null;
            int pos = FindNonWhiteSpace();
            if (pos >= Length)
                return false;
            string sztoken = null;
            if (IsQuote(CharAt(pos))) {
                int end = IndexString(InternalBuffer, pos) + 1;
                token = sztoken = InternalBuffer.Substring(pos, end - pos);
                Position = pos + sztoken.Length;
            }
            else {
                token = sztoken = BuffNextWord();
                AdvanceScanner(sztoken);
            }
            return (sztoken != null);
        }

        /// <summary>
        /// Tries to match a quoted and escaped string from the buffer
        /// </summary>
        public virtual bool NextString(out string parsed)
        {
            int pos = FindNonWhiteSpace();
            if (pos >= Length || !IsQuote(InternalBuffer[pos])) {
                parsed = null;
            }
            else {
                Position = ReadString(InternalBuffer, pos, out parsed) + 1;
            }
            return (parsed != null);
        }

        /// <summary>
        /// Advances the buffer to the end of a quoted string without parsing it
        /// </summary>
        public virtual bool SkipString()
        {
            int pos = FindNonWhiteSpace();
            if (pos >= Length || !IsQuote(InternalBuffer[pos])) {
                return false;
            }
            else {
                Position = IndexString(InternalBuffer, pos) + 1;
                return true;
            }
        }

        /// <summary>
        /// Checks if a character is an opening group character
        /// </summary>
        protected static bool IsGroupChar(int c)
        {
            return (c == '(' || c == '[');
        }

        /// <summary>
        /// Checks if a character is a valid quote character
        /// </summary>
        protected static bool IsQuote(int c)
        {
            return (c == '\"' || c == '\'');
        }

        /// <summary>
        /// Tries to match a group from the buffer and advances the reader to the end of that group
        /// </summary>
        public virtual bool NextGroup(out IList<IEnumerable<char>> args)
        {
            int start = FindNonWhiteSpace();
            int pos = GetGroup(start, out args);
            if (pos != start) {
                Position = pos;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to match a number from the buffer without advancing the reader
        /// </summary>
        protected virtual int GetGroup(int pos, out IList<IEnumerable<char>> args)
        {
            if (pos >= Length || !IsGroupChar(InternalBuffer[pos])) {
                args = null;
            }
            else {
                pos = ReadGroup(InternalBuffer, pos, out args) + 1;
            }
            return pos;
        }

        /// <summary>
        /// Advances the buffer to the end of a group without advancing it
        /// </summary>
        public virtual bool SkipGroup()
        {
            int pos = FindNonWhiteSpace();
            if (pos >= Length || !IsGroupChar(InternalBuffer[pos])) {
                return false;
            }
            else {
                Position = IndexGroup(InternalBuffer, pos) + 1;
                return true;
            }
        }

        /// <summary>
        /// Tries to match an identifier from the buffer and advances the reader to the end of that identifier
        /// </summary>
        public virtual bool NextValidIdentifier(out IEnumerable<char> name)
        {
            Match m = rxId.Match(BuffNextWord() ?? string.Empty);
            if (m.Success) {
                name = m.Value;
                AdvanceScanner(m.Length);
            }
            else {
                name = null;
            }
            return (name != null);
        }

        /// <summary>
        /// Tries to match a function from the buffer and advances the reader to the end of that function
        /// </summary>
        public virtual bool NextFunction(out IEnumerable<char> name, out IList<IEnumerable<char>> args)
        {
            int start = Position;
            if (NextValidIdentifier(out name) && NextGroup(out args)) {
                return true;
            }
            else {
                Position = start;
                args = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to match a variable from the buffer and advances the reader to the end of that variable
        /// </summary>
        public virtual bool NextVariable(out IEnumerable<char> name, out IList<IEnumerable<char>> indices)
        {
            name = null; indices = null;
            string word = BuffNextWord();
            if (string.IsNullOrEmpty(word))
                return false;
            if (word[0] == '@') {
                return NextMacro(out name, out indices);
            }
            Match m = rxId.Match(word);
            if (m.Success && CharAt(WordBuffer.Item1 + m.Length) == '$') {
                name = InternalBuffer.Substring(WordBuffer.Item1, m.Length + 1);
                AdvanceScanner(m.Length + 1);
                NextIndices(out indices);
            }
            return (name != null);
        }

        /// <summary>
        /// This assumes that the first char has already been checked to be an '@'!
        /// </summary>
        private bool NextMacro(out IEnumerable<char> name, out IList<IEnumerable<char>> indices)
        {
            name = null; indices = null;
            int start = Position;
            SkipWhiteSpace();
            Position += 1;
            string word = BuffNextWord();
            if (string.IsNullOrEmpty(word))
                return false;
            Match m = rxId.Match(word);
            if (m.Success) {
                name = InternalBuffer.Substring(start, word.Length + 1);
                AdvanceScanner(word.Length + 1);
                NextIndices(out indices);
                return true;
            }
            else {
                Position = start;
                return false;
            }
        }

        /// <summary>
        /// Tries to match a group of indices from the buffer and advances the reader to the end of that group
        /// </summary>
        public virtual bool NextIndices(out IList<IEnumerable<char>> indices)
        {
            int pos = Position;
            SkipWhiteSpace();
            if (Current == '[' && NextGroup(out indices)) {
                return true;
            }
            else {
                Position = pos;
                indices = null;
                return false;
            }
        }

        internal static bool NextFunctionInternal(IScanner scanner, TRuntime exec, out Function func)
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

        internal static bool NextVariable(IScanner scanner, TRuntime exec, out Variable variable)
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
        /// Tries to match boolean value from the buffer and advances the reader to the end of that boolean
        /// </summary>
        public virtual bool NextBool(out bool b)
        {
            string word = BuffNextWord();
            if (string.IsNullOrEmpty(word))
                return b = false;

            if (word.StartsWith(bool.TrueString, StringComparison.CurrentCultureIgnoreCase)) {
                AdvanceScanner(bool.TrueString);
                return b = true;
            }
            else if (word.StartsWith(bool.FalseString, StringComparison.CurrentCultureIgnoreCase)) {
                AdvanceScanner(bool.FalseString);
                b = false;
                return true;
            }
            return b = false;
        }
        
        /// <summary>
        /// Creates a new scanner with a given buffer
        /// </summary>
        public virtual IScanner Scan(IEnumerable<char> buffer)
        {
            return new DefaultScanner(buffer.ToString());
        }

        /// <summary>
        /// Reads a number of characters from the buffer
        /// </summary>
        public virtual IEnumerable<char> Read(int start, int count)
        {
            return StringSegment.Create(InternalBuffer, start, count);
        }
        
        /// <summary>
        /// Advances the reader a number of characters
        /// </summary>
        public void Skip(int count)
        {
            Position += count;
        }

        /// <summary>
        /// Converts this scanner's buffer to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return InternalBuffer;
        }
    }
}
