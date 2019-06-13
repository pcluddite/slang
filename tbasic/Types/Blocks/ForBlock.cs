/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using Tbasic.Lexer;
using Tbasic.Runtime;

namespace Tbasic.Types
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
