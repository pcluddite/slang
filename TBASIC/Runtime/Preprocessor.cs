// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Tbasic.Parsing;
using Tbasic.Errors;
using Tbasic.Components;

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

        private static int ProcessFuncBlock(TextReader reader, Line current, out FuncBlock block)
        {
            Line header = current;
            LineCollection body = new LineCollection();
            int lineNumber = current.LineNumber + 1;
            while((current = ProcessCodeLine(reader, lineNumber++)) != null && !FuncBlock.CheckEnd(current)) {
                body.Add(current);
            }
            if (current == null)
                throw ThrowHelper.UnterminatedBlock("FUNCTION");
            block = new FuncBlock(header, body, current);
            return lineNumber;
        }

        private static readonly Predicate<Line> ClassBegin = (o => o.Name.EqualsIgnoreCase("CLASS"));
        private static readonly Predicate<Line> ClassEnd = (o => o.Text.EqualsIgnoreCase("END CLASS"));

        private int ProcessClassBlock(TextReader reader, Line current, out TClass tclass)
        {
            int nline = current.LineNumber + 1;
            Scanner scanner = runtime.ScannerDelegate(new StringSegment(current.Text));
            scanner.Next("CLASS");
            scanner.SkipWhiteSpace();
            StringSegment classname;
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
                    tclass.SetFunction(func.Template.Name, func.CreateDelegate());
                }
                else if (current.Name.EqualsIgnoreCase(tclass.Name)) {
                    current.Text = "FUNCTION " + current.Text; // this is just to satisfy the parser. Try to fix later. 8/26/16
                    FuncBlock ctor;
                    nline = ProcessFuncBlock(reader, current, out ctor);
                    tclass.SetFunction("<>ctor", ctor.CreateDelegate());
                }
                else {
                    throw new UnexpectedTokenExceptiopn(current.Text);
                }
            }

            if (current == null)
                throw ThrowHelper.UnterminatedBlock("CLASS");
            
            return nline;
        }
    }
}
