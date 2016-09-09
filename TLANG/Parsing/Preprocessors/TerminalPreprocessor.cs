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

namespace Tint.Parsing
{
    internal class TerminalPreprocessor : DefaultPreprocessor
    {
        protected override Predicate<Line> ClassBegin { get; } = (o => o.Text.EqualsIgnoreCase("class"));
        protected override Predicate<Line> ClassEnd { get; } = (o => o.Text.Equals("}"));

        protected override Predicate<Line> FuncBegin { get; } = (o => o.Text.EqualsIgnoreCase("func"));
        protected override Predicate<Line> FuncEnd { get; } = (o => o.Text.Equals("}"));

        protected override void AddImplicitLet(ref Line line)
        {
            // don't do anything. we don't want an implicit let
        }
    }
}
