// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Runtime;

namespace Tbasic
{
    internal class SelectBlock : CodeBlock
    {
        public SelectBlock(int index, LineCollection code)
        {
            LoadFromCollection(
                code.ParseBlock(
                    index,
                    c => c.Name.EqualsIgnoreCase("SELECT"),
                    c => c.Text.EqualsIgnoreCase("END SELECT")
                ));
        }

        public override void Execute(TBasic runtime)
        {
            StackData stackdat = new StackData(runtime, Header.Text);
            if (stackdat.ParameterCount < 2) {
                throw ThrowHelper.NoCondition();
            }
            object obj = ExpressionEvaluator.Evaluate(new StringSegment(Header.Text, Header.Name.Length), runtime);
            CodeBlock _default;
            var dict = ToDictionary(runtime, out _default);
            if (obj != null && dict.ContainsKey(obj)) {
                dict[obj].Execute(runtime);
            }
            else if (_default != null) {
                _default.Execute(runtime);
            }
        }

        public Dictionary<object, CodeBlock> ToDictionary(TBasic runtime, out CodeBlock _default)
        {
            Dictionary<object, CodeBlock> dict = new Dictionary<object, CodeBlock>();
            _default = null;
            for (int index = 0; index < Body.Count; index++) {
                CaseBlock caseBlock;
                index = CaseBlock.ParseBlock(index, Body, out caseBlock) - 1;

                if (caseBlock.Header.Name.EqualsIgnoreCase("DEFAULT")) {
                    _default = caseBlock;
                }
                else if (caseBlock.Header.Name.EqualsIgnoreCase("CASE")) {
                    dict.Add(
                        ExpressionEvaluator.Evaluate(caseBlock.Condition, runtime),
                        caseBlock
                       );
                }
                else {
                    throw ThrowHelper.InvalidTypeInExpression(Body[0].Text, "CASE");
                }

            }
            return dict;
        }

        private class CaseBlock : CodeBlock
        {
            public override Line Footer
            {
                get {
                    throw new NotImplementedException();
                }
                set {
                    throw new NotImplementedException();
                }
            }

            public override int Length
            {
                get {
                    return Body.Count + 1;
                }
            }

            public StringSegment Condition { get; private set; }

            private CaseBlock(LineCollection body)
            {
                Header = body[0];
                StackData parms = new StackData(null, Header.Text);
                if (parms.Name.EqualsIgnoreCase("DEFAULT")) {
                    Condition = new StringSegment("default");
                }
                else if (parms.ParameterCount < 2) {
                    throw ThrowHelper.NoCondition();
                }
                else {
                    Condition = new StringSegment(Header.Text, Header.Name.Length);
                }
                body.RemoveAt(0);
                Body = body;
            }

            public static int ParseBlock(int index, LineCollection all, out CaseBlock caseBlock)
            {
                LineCollection blockLines = new LineCollection();

                bool isBlock = false;
                for (; index < all.Count; index++) {
                    if (isBlock) {
                        if (all[index].Name.EqualsIgnoreCase("CASE") ||
                            all[index].Name.EqualsIgnoreCase("DEFAULT")) {
                            break;
                        }
                        else {
                            blockLines.Add(all[index]);
                        }
                    }
                    else if (all[index].Name.EqualsIgnoreCase("CASE") ||
                             all[index].Name.EqualsIgnoreCase("DEFAULT")) {
                        isBlock = true;
                        blockLines.Add(all[index]);
                    }
                }
                if (blockLines.Count > 0) {
                    caseBlock = new CaseBlock(blockLines);
                }
                else {
                    caseBlock = null;
                }
                return index;
            }

            public override void Execute(TBasic exec)
            {
                exec.Execute(Body);
            }
        }
    }
}