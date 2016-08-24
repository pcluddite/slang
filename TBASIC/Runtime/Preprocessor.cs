// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tbasic.Parsing;
using Tbasic.Errors;

namespace Tbasic.Runtime
{
    internal class Preprocessor
    {
        public ICollection<FuncBlock> DefinedFunctions { get; private set; }
        public LineCollection Lines { get; private set; }

        private Preprocessor(TextReader reader)
        {
            IList<FuncBlock> funcs;
            Lines = ScanLines(reader, out funcs);
            DefinedFunctions = funcs;
        }

        public static Preprocessor Preprocess(TextReader reader)
        {
            return new Preprocessor(reader);
        }

        private static LineCollection ScanLines(TextReader reader, out IList<FuncBlock> userFunctions)
        {
            LineCollection allLines = new LineCollection();
            List<FuncBlock> userFuncs = new List<FuncBlock>();

            int lineNumber = 1;
            Line line;
            while ((line = ProcessCodeLine(reader, lineNumber++)) != null) {
                if (string.IsNullOrEmpty(line.Text) || line.Text[0] == ';')
                    continue;

                if (FuncBlock.CheckBegin(line)) {
                    userFuncs.Add(ProcessFuncBlock(reader, line));
                }
                else {
                    allLines.Add(line);
                }
            }
            userFunctions = userFuncs;
            return allLines;
        }

        private static Line ProcessCodeLine(TextReader reader, int lineNumber)
        {
            string linestr = reader.ReadLine()?.Trim();
            if (linestr == null)
                return null;
            if (linestr.Length > 0 && linestr[linestr.Length - 1] == '_') {
                Stack<char> chars = new Stack<char>(linestr);
                do {
                    chars.Pop(); // remove last '_' character
                    linestr = reader.ReadLine()?.Trim();
                    if (linestr == null)
                        throw new EndOfCodeException("Line continuation character '_' cannot end script");
                    foreach (char c in linestr)
                        chars.Push(c);
                }
                while (chars.Peek() == '_');
                linestr = new string(chars.Reverse().ToArray());
            }
            Line line = Line.CreateLineNoTrim(lineNumber, linestr);
            if (linestr.Length > 0 && (line.Name[line.Name.Length - 1] == '$' || line.Name[line.Name.Length - 1] == '$')) {
                line.VisibleName = line.Name;
                line.Text = "LET " + line.Text; // add the word LET if it's an equality, but use the original name as visible name
            }
            return line;
        }

        private static FuncBlock ProcessFuncBlock(TextReader reader, Line current)
        {
            Line header = current;
            LineCollection body = new LineCollection();
            int lineNumber = current.LineNumber + 1;
            while((current = ProcessCodeLine(reader, lineNumber++)) != null && !FuncBlock.CheckEnd(current)) {
                body.Add(current);
            }
            if (current == null)
                throw ThrowHelper.UnterminatedBlock("FUNCTION");
            return new FuncBlock(header, body, current);
        }
    }
}
