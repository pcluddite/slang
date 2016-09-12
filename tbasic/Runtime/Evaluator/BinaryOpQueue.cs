// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;
using Tbasic.Types;

namespace Tbasic.Runtime
{
    internal class BinaryOpQueue
    {
        private LinkedList<BinOpNodePair> _oplist = new LinkedList<BinOpNodePair>();

        public BinaryOpQueue(LinkedList<object> expressionlist)
        {
            LinkedListNode<object> i = expressionlist.First;
            while (i != null) {
                Enqueue(new BinOpNodePair(i));
                i = i.Next;
            }
        }

        public bool Enqueue(BinOpNodePair nodePair)
        {
            if (!nodePair.IsValid())
                return false;

            for (var currentNode = _oplist.First; currentNode != null; currentNode = currentNode.Next) {
                if (currentNode.Value.Operator.Precedence > nodePair.Operator.Precedence) {
                    _oplist.AddBefore(currentNode, nodePair);
                    return true;
                }
            }
            _oplist.AddLast(nodePair);
            return true;
        }

        public bool Dequeue(out BinOpNodePair nodePair)
        {
            if (_oplist.Count == 0) {
                nodePair = default(BinOpNodePair);
                return false;
            }
            nodePair = _oplist.First.Value;
            _oplist.RemoveFirst();
            return true;
        }

        public int Count
        {
            get { return _oplist.Count; }
        }
    }
}
