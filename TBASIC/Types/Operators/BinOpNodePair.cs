// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;

namespace Tbasic.Types
{
    internal struct BinOpNodePair
    {
        private LinkedListNode<object> node;
        private BinaryOperator op;

        public BinaryOperator Operator
        {
            get {
                return op;
            }
        }

        public LinkedListNode<object> Node
        {
            get {
                return node;
            }
        }

        public BinOpNodePair(LinkedListNode<object> node)
        {
            this.node = node;
            var binop = node.Value as BinaryOperator?;
            if (binop == null) {
                op = default(BinaryOperator);
            }
            else {
                op = binop.Value;
            }
        }

        public bool IsValid()
        {
            return op != default(BinaryOperator);
        }
    }
}
