﻿/**
 *  TBASIC
 *  Copyright (C) 2016 Timothy Baxendale
 *  
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *  
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *  
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301
 *  USA
 **/
using System;
using Tbasic.Runtime;
using System.Collections.Generic;

namespace Tbasic {
    internal class SelectBlock : CodeBlock {
        public SelectBlock(int index, LineCollection code) {
            LoadFromCollection(
                code.ParseBlock(
                    index,
                    c => c.Name.Equals("SELECT", StringComparison.OrdinalIgnoreCase),
                    c => c.Text.Equals("END SELECT", StringComparison.OrdinalIgnoreCase)
                ));
        }

        public override void Execute(Executer exec) {
            StackFrame parms = new StackFrame(exec, Header.Text);
            if (parms.Count < 2) {
                throw ScriptException.NoCondition();
            }
            object obj = Evaluator.Evaluate(Header.Text.Substring(Header.Name.Length), exec);
            CodeBlock _default;
            var dict = ToDictionary(exec, out _default);
            if (obj != null && dict.ContainsKey(obj)) {
                dict[obj].Execute(exec);
            }
            else if (_default != null) {
                _default.Execute(exec);
            }
        }

        public Dictionary<object, CodeBlock> ToDictionary(Executer exec, out CodeBlock _default) {
            Dictionary<object, CodeBlock> dict = new Dictionary<object, CodeBlock>();
            _default = null;
            for (int index = 0; index < Body.Count; index++) {
                CaseBlock caseBlock;
                index = CaseBlock.ParseBlock(index, Body, out caseBlock) - 1;

                if (caseBlock.Header.Name.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)) {
                    _default = caseBlock;
                }
                else if (caseBlock.Header.Name.Equals("CASE", StringComparison.OrdinalIgnoreCase)) {
                    dict.Add(
                        Evaluator.Evaluate(caseBlock.Condition, exec),
                        caseBlock
                       );
                }
                else {
                    throw ScriptException.InvalidExpression(Body[0].Text, "CASE");
                }

            }
            return dict;
        }

        private class CaseBlock : CodeBlock {
            public override Line Footer {
                get {
                    throw new NotImplementedException();
                }
                set {
                    throw new NotImplementedException();
                }
            }

            public override int Length {
                get {
                    return Body.Count + 1;
                }
            }

            public string Condition { get; private set; }

            private CaseBlock(LineCollection body) {
                Header = body[0];
                StackFrame parms = new StackFrame(null, Header.Text);
                if (parms.Name.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)) {
                    Condition = "default";
                }
                else if (parms.Count < 2) {
                    throw ScriptException.NoCondition();
                }
                else {
                    Condition = Header.Text.Substring(Header.Text.IndexOf(' ') + 1);
                }
                body.RemoveAt(0);
                Body = body;
            }

            public static int ParseBlock(int index, LineCollection all, out CaseBlock caseBlock) {
                LineCollection blockLines = new LineCollection();

                bool isBlock = false;
                for (; index < all.Count; index++) {
                    if (isBlock) {
                        if (all[index].Name.Equals("CASE", StringComparison.OrdinalIgnoreCase) ||
                            all[index].Name.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)) {
                            break;
                        }
                        else {
                            blockLines.Add(all[index]);
                        }
                    }
                    else if (all[index].Name.Equals("CASE", StringComparison.OrdinalIgnoreCase) ||
                             all[index].Name.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)) {
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

            public override void Execute(Executer exec) {
                exec.Execute(Body);
            }
        }
    }
}
