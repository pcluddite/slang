/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
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
