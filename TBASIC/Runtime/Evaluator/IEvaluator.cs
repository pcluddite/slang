// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Tbasic.Components;

namespace Tbasic.Runtime
{
    /// <summary>
    /// An expression evaluator
    /// </summary>
    internal interface IEvaluator
    {
        StringSegment Expression { get; set; }
        object Evaluate();
        TBasic CurrentExecution { get; set; }
    }
}
