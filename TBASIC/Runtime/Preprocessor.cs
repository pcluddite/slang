// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Types;

namespace Tbasic.Runtime
{
    internal class Preprocessor
    {
        public const char CommentChar = ';';
        public const char ContinueChar = '_';

        public ICollection<FuncBlock> Functions { get; } = new List<FuncBlock>();
        public ICollection<TClass> Types { get; } = new List<TClass>();
        public LineCollection Lines { get; } = new LineCollection();

        private TBasic runtime;

        private Preprocessor(TextReader reader, TBasic runtime)
        {
            this.runtime = runtime;
            ScanLines(reader);
            this.runtime = null; // don't hold onto it on my account 8/26/16
        }

        public static Preprocessor Preprocess(TextReader reader, TBasic runtime)
        {
            return new Preprocessor(reader, runtime);
        }

        private void ScanLines(TextReader reader)
        {
            int lineNumber = 1;
            Line line;
            while ((line = ProcessCodeLine(reader, lineNumber++)) != null) {
                if (string.IsNullOrEmpty(line.Text) || line.Text[0] == CommentChar)
                    continue;

                if (FuncBlock.CheckBegin(line)) {
                    FuncBlock func;
                    lineNumber = ProcessFuncBlock(reader, line, out func);
                    Functions.Add(func);
                }
                else if (ClassBegin(line)) {
                    TClass tclass;
                    lineNumber = ProcessClassBlock(reader, line, out tclass);
                    Types.Add(tclass);
                }
                else {
                    Lines.Add(line);
                }
            }
        }

        private static Line ProcessCodeLine(TextReader reader, int lineNumber)
        {
            string linestr = reader.ReadLine()?.Trim();
            if (linestr == null)
                return null;
            if (linestr.Length > 0 && linestr[linestr.Length - 1] == ContinueChar) {
                Stack<char> chars = new Stack<char>(linestr);
                do {
                    chars.Pop(); // remove last '_' character
                    linestr = reader.ReadLine()?.Trim();
                    if (linestr == null)
                        throw new EndOfCodeException($"Line continuation character '{ContinueChar}' cannot end script");
                    foreach (char c in linestr)
                        chars.Push(c);
                }
                while (chars.Peek() == ContinueChar);
                linestr = new string(chars.Reverse().ToArray());
            }
            Line line = Line.CreateLineNoTrim(lineNumber, linestr);
            if (linestr.Length > 0 && (line.Name[line.Name.Length - 1] == '$' || line.Name[line.Name.Length - 1] == '$')) {
                line.VisibleName = line.Name;
                line.Text = "LET " + line.Text; // add the word LET if it's an equality, but use the original name as visible name
            }
            return line;
        }

        private int ProcessFuncBlock(TextReader reader, Line current, out FuncBlock block)
        {
            Line header = current;
            LineCollection body = new LineCollection();
            int lineNumber = current.LineNumber + 1;
            while((current = ProcessCodeLine(reader, lineNumber++)) != null && !FuncBlock.CheckEnd(current)) {
                body.Add(current);
            }
            if (current == null)
                throw ThrowHelper.UnterminatedBlock("FUNCTION");
            block = new FuncBlock(GetPrototype(header), header, body, current);
            return lineNumber;
        }

        private StackData GetPrototype(Line header)
        {
            IScanner scanner = runtime.Scanner.Scan(header.Text);
            scanner.Next("FUNCTION");
            scanner.SkipWhiteSpace();
            IEnumerable<char> funcname;
            if (!scanner.NextValidIdentifier(out funcname))
                throw new InvalidDefinitionException("Name contains invalid characters or was not present", "function");
            IList<IEnumerable<char>> args;
            scanner.NextGroup(out args);
            StackData stackdat = new StackData(null, args.TB_ToStrings());
            stackdat.Name = funcname.ToString();
            return stackdat;
        }

        private static readonly Predicate<Line> ClassBegin = (o => o.Name.EqualsIgnoreCase("CLASS"));
        private static readonly Predicate<Line> ClassEnd = (o => o.Text.EqualsIgnoreCase("END CLASS"));

        private int ProcessClassBlock(TextReader reader, Line current, out TClass tclass)
        {
            int nline = current.LineNumber + 1;
            IScanner scanner = runtime.Scanner.Scan(current.Text);
            scanner.Next("CLASS");
            scanner.SkipWhiteSpace();
            IEnumerable<char> classname;
            if (!scanner.NextValidIdentifier(out classname))
                throw new InvalidDefinitionException("Name contains invalid characters or was not present", "class");

            tclass = new TClass(classname.ToString(), runtime.Global);

            while ((current = ProcessCodeLine(reader, nline++)) != null && !ClassEnd(current)) {
                if (string.IsNullOrEmpty(current.Text) || current.Text[0] == CommentChar)
                    continue;

                if (current.Name.EqualsIgnoreCase("DIM")) {
                    tclass.Constructor.Add(current);
                }
                else if (FuncBlock.CheckBegin(current)) {
                    FuncBlock func;
                    nline = ProcessFuncBlock(reader, current, out func);
                    tclass.SetFunction(func.Prototype.Name, func.CreateDelegate());
                }
                else if (current.Name.EqualsIgnoreCase(tclass.Name)) {
                    current.Text = "FUNCTION " + current.Text; // this is just to satisfy the parser. Try to fix later. 8/26/16
                    FuncBlock ctor;
                    nline = ProcessFuncBlock(reader, current, out ctor);
                    tclass.SetFunction("<>ctor", ctor.CreateDelegate());
                }
                else {
                    throw new InvalidTokenExceptiopn(current.Text);
                }
            }

            if (current == null)
                throw ThrowHelper.UnterminatedBlock("CLASS");
            
            return nline;
        }
    }
}
