// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Components;
using Tbasic.Runtime;

namespace Tbasic.Libraries.Standard
{
    /// <summary>
    /// A library containing several mathmatical functions
    /// </summary>
    public class MathLibrary
    {
        private static Random rand = new Random();

        internal static void AddUntaggedDelegates(TBasicLibrary lib)
        {
            lib.Add<double, double>("ABS", Math.Abs);
            lib.Add<double, double>("SIN", Math.Sin);
            lib.Add<double, double>("ASIN", Math.Asin);
            lib.Add<double, double>("SINH", Math.Sinh);
            lib.Add<double, double>("COS", Math.Cos);
            lib.Add<double, double>("ACOS", Math.Acos);
            lib.Add<double, double>("COSH", Math.Cosh);
            lib.Add<double, double>("TAN", Math.Tan);
            lib.Add<double, double>("ATAN", Math.Atan);
            lib.Add<double, double>("TANH", Math.Tanh);
            lib.Add<double, double>("LOG", Math.Log10);
            lib.Add<double, double>("LN", Math.Log);
            lib.Add<double, double>("SQRT", Math.Sqrt);
            lib.Add<double, int, double>("ROUND", Math.Round);
            lib.Add<double, double, double>("POW", Math.Pow);
        }

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public MathLibrary(ObjectContext context)
        {
            context.SetConstant("@PI", Math.PI); // pi
            context.SetConstant("@E", Math.E); // euler's number
        }

        /// <summary>
        /// Calculates a root with a given index
        /// </summary>
        /// <param name="radicand">the number under the radix</param>
        /// <param name="index">the degree of the root (e.g. 2 is square root, 3 is cube root, etc)</param>
        /// <returns></returns>
        [TBasicInstruction("ROOT", typeof(Func<double, double, double>), TBasicCallType.Function, RequiredParameters = 2)]
        public static double Root(double radicand, double index)
        {
            return Math.Pow(radicand, 1.0d / index);
        }

        /// <summary>
        /// Returns a pseudo-random double between 0 and 1
        /// </summary>
        /// <returns>a pseudo-random double between 0 and 1</returns>
        public static double Random()
        {
            return rand.NextDouble();
        }

        /// <summary>
        /// Returns a pseudo-random double between 0 and a max value
        /// </summary>
        /// <param name="max">the exclusive upper bound</param>
        /// <returns></returns>
        public static double Random(int max)
        {
            return Random() * max;
        }

        /// <summary>
        /// Returns a pseudo-random double between a specified upper and lower bound
        /// </summary>
        /// <param name="lowerBound">the inclusive lower bound</param>
        /// <param name="upperBound">the exclusive upper bound</param>
        /// <returns></returns>
        public static double Random(int lowerBound, int upperBound)
        {
            return Random(upperBound - lowerBound) + lowerBound;
        }

        private object Random(RuntimeData stackFrame)
        {
            stackFrame.AssertCount(atLeast: 1, atMost: 3);
            if (stackFrame.ParameterCount == 1) {
                return Random();
            }
            else if (stackFrame.ParameterCount == 2) {
                return Random(stackFrame.GetAt<int>(1));
            }
            else {
                return Random(stackFrame.GetAt<int>(1), stackFrame.GetAt<int>(2));
            }
        }

        /// <summary>
        /// Returns the integer part of a double value
        /// </summary>
        /// <param name="d">the double to truncate</param>
        /// <returns>the truncated double</returns>
        [TBasicInstruction("IPART", typeof(Func<double, int>), TBasicCallType.Function, RequiredParameters = 1)]
        public static int iPart(double d)
        {
            return (int)d;
        }

        /// <summary>
        /// Returns the fractional part of a double value
        /// </summary>
        /// <param name="d">the double to truncate</param>
        /// <returns>the truncated double</returns>
        [TBasicInstruction("FPART", typeof(Func<double, double>), TBasicCallType.Function, RequiredParameters = 1)]
        public static double fPart(double d)
        {
            return d - (int)d;
        }

        /// <summary>
        /// Evaluates a mathmatical expression
        /// </summary>
        /// <param name="expr">the expression to evaluate</param>
        /// <returns>the evaluated expression</returns>
        [TBasicInstruction("EVAL", typeof(Func<string, object>), TBasicCallType.Function, RequiredParameters = 1)]
        public static object Eval(string expr)
        {
            Executer e = new Executer(); // local execution
            e.Global.LoadStandardOperators();
            //e.Global.AddLibrary(new MathLibrary(e.Global)); // only allow math libs
            e.Global.SetFunction("eval", null); // that's a no-no
            return Evaluator.Evaluate(new StringSegment(expr), e);
        }
    }
}