// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    /// <summary>
    /// A library containing several mathmatical functions
    /// </summary>
    public class MathLibrary : Library
    {
        private static Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public MathLibrary(ObjectContext context)
        {
            Add<double, double>("ABS", Math.Abs);
            Add<double, double>("SIN", Math.Sin);
            Add<double, double>("ASIN", Math.Asin);
            Add<double, double>("SINH", Math.Sinh);
            Add<double, double>("COS", Math.Cos);
            Add<double, double>("ACOS", Math.Acos);
            Add<double, double>("COSH", Math.Cosh);
            Add<double, double>("TAN", Math.Tan);
            Add<double, double>("ATAN", Math.Atan);
            Add<double, double>("TANH", Math.Tanh);
            Add<double, double>("LOG", Math.Log10);
            Add<double, double>("LN", Math.Log);
            Add<double, double>("SQRT", Math.Sqrt);
            Add<double, double>("FPART", fPart);
            Add<double, int>("IPART", iPart);
            Add<string, object>("EVAL", Eval);
            Add<double, int, double>("ROUND", Math.Round);
            Add<double, double, double>("POW", Math.Pow);
            Add<double, double, double>("ROOT", Root);
            Add("RANDOM", Random);
            context.SetConstant("@PI", Math.PI); // pi
            context.SetConstant("@E", Math.E); // euler's number
        }

        /// <summary>
        /// Calculates a root with a given index
        /// </summary>
        /// <param name="radicand">the number under the radix</param>
        /// <param name="index">the degree of the root (e.g. 2 is square root, 3 is cube root, etc)</param>
        /// <returns></returns>
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

        private object Random(FuncData stackFrame)
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
        public static int iPart(double d)
        {
            return (int)d;
        }

        /// <summary>
        /// Returns the fractional part of a double value
        /// </summary>
        /// <param name="d">the double to truncate</param>
        /// <returns>the truncated double</returns>
        public static double fPart(double d)
        {
            return d - (int)d;
        }

        /// <summary>
        /// Evaluates a mathmatical expression
        /// </summary>
        /// <param name="expr">the expression to evaluate</param>
        /// <returns>the evaluated expression</returns>
        public static object Eval(string expr)
        {
            Executer e = new Executer(); // local execution
            e.Global.LoadStandardOperators();
            e.Global.AddLibrary(new MathLibrary(e.Global)); // only allow math libs
            e.Global.SetFunction("eval", null); // that's a no-no
            return Evaluator.Evaluate(new StringSegment(expr), e);
        }
    }
}