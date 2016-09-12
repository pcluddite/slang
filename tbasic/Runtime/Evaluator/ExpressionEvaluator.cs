// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Types;
using System.Linq;

namespace Tbasic.Runtime
{
    /// <summary>
    /// General purpose evaluator for functions, variables, math, booleans, etc.
    /// </summary>
    internal partial class ExpressionEvaluator : IExpressionEvaluator
    {
        private object[] results = null;
        private IEnumerable<char> _expression = string.Empty;
        
        public ExpressionEvaluator(TRuntime runtime)
        {
            Runtime = runtime;
        }
        
        public ExpressionEvaluator(IEnumerable<char> expression, TRuntime exec)
        {
            Runtime = exec;
            Expression = expression;
        }

        #region Properties
        
        public IEnumerable<char> Expression
        {
            get { return _expression; }
            set {
                _expression = value;
                results = null;
            }
        }

        public ObjectContext CurrentContext
        {
            get {
                return Runtime.Context;
            }
            set {
                throw new NotImplementedException(); // this shouldn't be set
            }
        }

        public TRuntime Runtime { get; set; }

        public bool Parsed
        {
            get {
                return (results != null);
            }
            set {
                if (Parsed && !value) {
                    results = null;
                }
            }
        }

        #endregion

        public object Evaluate(IEnumerable<char> expr)
        {
            Expression = expr;
            return Evaluate();
        }

        public object Evaluate()
        {
            object[] results = EvaluateAll();

            if (results.Length == 1) {
                return results[0];
            }
            else {
                return results;
            }
        }

        private object[] EvaluateAll()
        {
            if (Parsed) {
                return results;
            }
            else if (Expression == null || Expression.ToString() == string.Empty) {
                return (results = new object[0]);
            }
            else {
                IScanner scanner = Runtime.Scanner.Scan(_expression);
                LinkedList<object> tokens = new LinkedList<object>();
                List<object> lResults = new List<object>();

                while (!scanner.EndOfStream) {
                    if (NextToken(tokens, scanner)) {
                        lResults.Add(ConvertToSimpleType(EvaluateList(tokens), Runtime.Options));
                        tokens = new LinkedList<object>();
                    }
                }

                lResults.Add(ConvertToSimpleType(EvaluateList(tokens), Runtime.Options));
                return (results = lResults.ToArray());
            }
        }

        public bool EvaluateBool()
        {
            return Convert.ToBoolean(Evaluate());
        }
        
        private bool NextToken(LinkedList<object> tokens, IScanner scanner)
        {
            int startIndex = scanner.Position;

            // check group
            if (scanner.Next("(")) {
                return AddObjectToExprList("(", startIndex, scanner, tokens);
            }

            // check variable
            Variable variable;
            if (DefaultScanner.NextVariable(scanner, Runtime, out variable)) {
                return AddObjectToExprList(variable, startIndex, scanner, tokens);
            }

            // check unary op
            UnaryOperator unaryOp;
            if (scanner.NextUnaryOp(CurrentContext, tokens.Last?.Value, out unaryOp)) {
                return AddObjectToExprList(unaryOp, startIndex, scanner, tokens);
            }

            // check binary operator
            BinaryOperator binOp;
            if (scanner.NextBinaryOp(CurrentContext, out binOp)) {
                return AddObjectToExprList(binOp, startIndex, scanner, tokens);
            }

            // check hexadecimal
            long hex;
            if (scanner.NextHexadecimal(out hex)) {
                return AddObjectToExprList(hex, startIndex, scanner, tokens);
            }

            // check boolean
            bool b;
            if (scanner.NextBool(out b)) {
                return AddObjectToExprList(b, startIndex, scanner, tokens);
            }

            // check numeric
            Number num;
            if (scanner.NextNumber(out num)) {
                return AddObjectToExprList(num, startIndex, scanner, tokens);
            }

            // check null
            if (scanner.NextNull()) {
                return AddObjectToExprList(null, startIndex, scanner, tokens);
            }

            // check string
            string str_parsed;
            if (scanner.NextString(out str_parsed)) {
                return AddObjectToExprList(str_parsed, startIndex, scanner, tokens);
            }

            // check function
            Function func;
            if (DefaultScanner.NextFunctionInternal(scanner, Runtime, out func)) {
                return AddObjectToExprList(func, startIndex, scanner, tokens);
            }

            if (scanner.NextComment()) {
                scanner.Position = scanner.Length; // comments go to the end of the line
                return false;
            }

            if (scanner.NextExpressionBreak()) {
                return true;
            }

            // couldn't be parsed

            if (CurrentContext.FindFunctionContext(_expression.ToString()) == null) {
                throw new InvalidTokenException(scanner.Next()?.ToString());
            }
            else {
                throw new FormatException("Poorly formed function call");
            }
        }

        private bool AddObjectToExprList(object val, int startIndex, IScanner scanner, LinkedList<object> tokens)
        {
            if (Equals(val, "(")) {
                scanner.Position = startIndex;
                scanner.SkipWhiteSpace();
                scanner.SkipGroup();

                ExpressionEvaluator eval = new ExpressionEvaluator(
                    scanner.Read(startIndex + 1, scanner.Position - startIndex - 2),
                    Runtime // share the wealth
                );
                tokens.AddLast(eval);
            }
            else {
                tokens.AddLast(val);
            }
            scanner.SkipWhiteSpace();
            return false;
        }
        
        private object EvaluateList(LinkedList<object> _tokens)
        {
            LinkedList<object> list = new LinkedList<object>(_tokens);

            // evaluate unary operators

            LinkedListNode<object> x = list.First;
            while (x != null) {
                UnaryOperator? op = x.Value as UnaryOperator?;
                if (op != null) {
                    var node = x.Next;
                    x.Value = PerformUnaryOp(Runtime, op.Value, node?.Value);
                    if (node != null)
                        list.Remove(node);
                }
                x = x.Next;
            }

            // queue and evaluate binary operators
            BinaryOpQueue opqueue = new BinaryOpQueue(list);
            
            x = list.First?.Next; // skip the first operand
            while (x != null) {
                BinaryOperator? op = x.Value as BinaryOperator?;
                if (op == null) {
                    throw ThrowHelper.MissingBinaryOp(x.Previous.Value, x.Value);
                }
                else {
                    if (x.Next == null)
                        throw new ArgumentException("Expression cannot end in a binary operation [" + x.Value + "]");
                }
                x = x.Next.Next; // skip the operand
            }

            BinOpNodePair nodePair;
            while (opqueue.Dequeue(out nodePair)) {
                nodePair.Node.Value = PerformBinaryOp(
                    Runtime,
                    nodePair.Operator,
                    nodePair.Node.Previous?.Value,
                    nodePair.Node.Next?.Value
                    );
                if (nodePair.Node.Next != null)
                    list.Remove(nodePair.Node.Next);
                if (nodePair.Node.Previous != null)
                    list.Remove(nodePair.Node.Previous);
            }

            IExpressionEvaluator expr = list.First?.Value as IExpressionEvaluator;
            if (expr == null) {
                return list.First?.Value;
            }
            else {
                return expr.Evaluate();
            }
        }
        
        #region static methods

        /// <summary>
        /// Static version of the Expression Evaluator
        /// </summary>
        /// <param name="expressionString">expression to be evaluated</param>
        /// <param name="exec">the current execution</param>
        /// <returns></returns>
        public static object Evaluate(IEnumerable<char> expressionString, TRuntime exec)
        {
            ExpressionEvaluator expression = new ExpressionEvaluator(expressionString, exec);
            return expression.Evaluate();
        }

        public static object PerformUnaryOp(TRuntime runtime, UnaryOperator op, object operand)
        {
            if (op.EvaluateOperand) {
                IExpressionEvaluator tempv = operand as IExpressionEvaluator;
                if (tempv != null)
                    operand = tempv.Evaluate();
            }

            try {
                return op.ExecuteOperator(runtime, operand);
            }
            catch(InvalidCastException) {
                throw new ArgumentException("Unary operand cannot be " + operand.GetType().Name);
            }
        }

        /// <summary>
        /// Performs a binary operation
        /// </summary>
        public static object PerformBinaryOp(TRuntime runtime, BinaryOperator op, object left, object right)
        {
            if (op.EvaulatedOperand.HasFlag(BinaryOperator.OperandPosition.Left)) {
                IExpressionEvaluator tv = left as IExpressionEvaluator;
                if (tv != null)
                    left = tv.Evaluate();
            }

            try {
                if (op.EvaulatedOperand.HasFlag(BinaryOperator.OperandPosition.Right)) {
                    IExpressionEvaluator tv;
                    switch (op.OperatorString) { // short circuit evaluation 1/6/16
                        case "AND":
                            if (Convert.ToBoolean(left, CultureInfo.CurrentCulture)) {
                                tv = right as IExpressionEvaluator;
                                if (tv != null) {
                                    right = tv.Evaluate();
                                }
                                if (Convert.ToBoolean(right, CultureInfo.CurrentCulture)) {
                                    return true;
                                }
                            }
                            return false;
                        case "OR":
                            if (Convert.ToBoolean(left, CultureInfo.CurrentCulture)) {
                                return true;
                            }
                            else {
                                tv = right as IExpressionEvaluator;
                                if (tv != null) {
                                    right = tv.Evaluate();
                                }
                                if (Convert.ToBoolean(right, CultureInfo.CurrentCulture)) {
                                    return true;
                                }
                            }
                            return false;
                    }

                    tv = right as IExpressionEvaluator;
                    if (tv != null)
                        right = tv.Evaluate();
                }
                
                return op.ExecuteOperator(runtime, left, right);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is FormatException || ex is ArgumentException || ex is OverflowException) {
                throw new FormatException(string.Format(
                        "Operator [{0}] cannot be applied to objects of type '{1}' and '{2}'",
                        op.OperatorString, GetTypeName(right), GetTypeName(left)
                    ));
            }
        }

        public static string GetTypeName(object value)
        {
            Type t = value.GetType();
            if (t.IsArray) {
                return "array";
            }
            else {
                return t.Name.ToLower();
            }
        }

        public static string GetStringRepresentation(object obj, bool escapeStrings = false)
        {
            if (obj == null)
                return "";
            
            string str_obj = obj as string;
            if (str_obj != null) {
                if (escapeStrings) {
                    return ToEscapedString(str_obj);
                }
                else {
                    return str_obj;
                }
            }
            else if (obj.GetType().IsArray) {
                StringBuilder sb = new StringBuilder("{ ");
                object[] _aObj = (object[])obj;
                if (_aObj.Length > 0) {
                    for (int i = 0; i < _aObj.Length - 1; i++) {
                        sb.AppendFormat("{0}, ", GetStringRepresentation(_aObj[i], escapeStrings: true));
                    }
                    sb.AppendFormat("{0} ", GetStringRepresentation(_aObj[_aObj.Length - 1], escapeStrings: true));
                }
                sb.Append("}");
                return sb.ToString();
            }
            return obj.ToString();
        }

        private static string ToEscapedString(string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('\"');
            for (int index = 0; index < str.Length; index++) {
                char c = str[index];
                switch (c) {
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\"': sb.Append("\\\""); break;
                    case '\'': sb.Append("\\'"); break;
                    default:
                        if (c < ' ') {
                            sb.Append("\\u");
                            sb.Append(Convert.ToString(c, 16).PadLeft(4, '0'));
                        }
                        else {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append('\"');
            return sb.ToString();
        }

        public static object ConvertToSimpleType(object _oObj, ExecuterOption opts)
        {
            if (_oObj == null) {
                if (opts.HasFlag(ExecuterOption.NullIsZero)) {
                    return 0;
                }
                else {
                    return null;
                }
            }

            if (_oObj is bool)
                return _oObj;

            Number? _nObj = Number.AsNumber(_oObj, opts);
            if (_nObj != null) {
                return _nObj.Value;
            }
            
            IntPtr? _pObj = _oObj as IntPtr?;
            if (_pObj != null) {
                return ConvertToSimpleType(_pObj.Value);
            }

            return _oObj;
        }

        public static object ConvertToSimpleType(IntPtr ptr)
        {
            if (IntPtr.Size == sizeof(long)) { // 64-bit
                return ptr.ToInt64();
            }
            else { // 32-bit
                return ptr.ToInt32();
            }
        }

        #endregion

        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}
