// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Parsing;
using Tbasic.Runtime;

namespace Tbasic
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

        public override void Execute(Executer exec)
        {
            throw new NotImplementedException();
        }
    }
}
