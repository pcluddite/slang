// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using TLang.Parsing;
using TLang.Runtime;

namespace TLang.Types
{
    internal class ForBlock : CodeBlock
    {
        public ForBlock(int index, LineCollection code)
        {
            LoadFromCollection(
                code.ParseBlock(
                    index,
                    c => c.Name.EqualsIgnoreCase("FOR"),
                    c => c.Name.EqualsIgnoreCase("NEXT")
                ));
        }

        public override void Execute(TRuntime exec)
        {
            throw new NotImplementedException();
        }
    }
}
