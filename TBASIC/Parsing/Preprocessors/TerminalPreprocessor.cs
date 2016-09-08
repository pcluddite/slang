// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TLang.Parsing
{
    internal class TerminalPreprocessor : DefaultPreprocessor
    {
        protected override Predicate<Line> ClassBegin { get; } = (o => o.Text.Equals("class {"));
        protected override Predicate<Line> ClassEnd { get; } = (o => o.Text.Equals("}"));

        protected override Line ProcessCodeLine(TextReader reader, int lineNumber)
        {
            return base.ProcessCodeLine(reader, lineNumber);
        }
    }
}
