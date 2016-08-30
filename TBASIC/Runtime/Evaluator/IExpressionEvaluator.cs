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
    internal interface IExpressionEvaluator
    {
        StringSegment Expression { get; set; }
        object Evaluate();
        TBasic Runtime { get; set; }
    }
}
