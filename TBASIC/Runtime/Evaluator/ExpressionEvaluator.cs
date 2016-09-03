// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Types;

namespace Tbasic.Runtime
{
    /// <summary>
    /// General purpose evaluator for functions, variables, math, booleans, etc.
    /// </summary>
    internal partial class ExpressionEvaluator : IExpressionEvaluator
    {
        private LinkedList<object> _tokens = new LinkedList<object>();
        private IEnumerable<char> _expression = string.Empty;
        private bool _parsed;
        
        public ExpressionEvaluator(TBasic runtime)
        {
            Runtime = runtime;
        }
        
        public ExpressionEvaluator(IEnumerable<char> expression, TBasic exec)
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
                _parsed = false;
                _tokens.Clear();
            }
        }

        public ObjectContext RuntimeContext
        {
            get {
                return Runtime.Context;
            }
        }

        public TBasic Runtime { get; set; }

        public bool Evaluated
        {
            get {
                return !_parsed;
            }
            set {
                if (_parsed && value) {
                    _tokens.Clear();
                    _parsed = !value;
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
            if (Expression == null || Expression.ToString() == string.Empty) 
                return 0;
            
            if (!_parsed) {
                IScanner scanner = Runtime.Scanner.Scan(_expression);
                while (!scanner.EndOfStream)
                    NextToken(scanner);
                _parsed = true;
            }

            return ConvertToSimpleType(EvaluateList(), Runtime.Options);
        }

        public bool EvaluateBool()
        {
            return Convert.ToBoolean(Evaluate());
        }
        
        private int NextToken(IScanner scanner)
        {
            int startIndex = scanner.Position;

            // check group
            if (scanner.Next("(")) {
                return AddObjectToExprList("(", startIndex, scanner);
            }

            // check string
            string str_parsed;
            if (scanner.NextString(out str_parsed)) {
                return AddObjectToExprList(str_parsed, startIndex, scanner);
            }

            // check unary op
            UnaryOperator unaryOp;
            if (scanner.NextUnaryOp(RuntimeContext, _tokens.Last?.Value, out unaryOp)) {
                return AddObjectToExprList(unaryOp, startIndex, scanner);
            }

            // check function
            Function func;
            if (AbstractScanner.NextFunctionInternal(scanner, Runtime, out func)) {
                return AddObjectToExprList(func, startIndex, scanner);
            }

            // check null
            if (scanner.Next("null")) {
                return AddObjectToExprList("null", startIndex, scanner);
            }

            // check variable
            Variable variable;
            if (AbstractScanner.NextVariable(scanner, Runtime, out variable)) {
                return AddObjectToExprList(variable, startIndex, scanner);
            }

            // check hexadecimal
            long hex;
            if (scanner.NextHexadecimal(out hex)) {
                return AddObjectToExprList(hex, startIndex, scanner);
            }

            // check boolean
            bool b;
            if (scanner.NextBool(out b)) {
                return AddObjectToExprList(b, startIndex, scanner);
            }

            // check numeric
            Number num;
            if (scanner.NextNumber(out num)) {
                return AddObjectToExprList(num, startIndex, scanner);
            }

            // check binary operator
            BinaryOperator binOp;
            if (scanner.NextBinaryOp(RuntimeContext, out binOp)) {
                return AddObjectToExprList(binOp, startIndex, scanner);
            }

            // couldn't be parsed

            if (RuntimeContext.FindFunctionContext(_expression.ToString()) == null) {
                throw new InvalidTokenExceptiopn(scanner.Next()?.ToString());
            }
            else {
                throw new FormatException("Poorly formed function call");
            }
        }

        private int AddObjectToExprList(object val, int startIndex, IScanner scanner)
        {
            if (Equals(val, "(")) {
                scanner.Position = startIndex;
                scanner.SkipWhiteSpace();
                scanner.SkipGroup();

                ExpressionEvaluator eval = new ExpressionEvaluator(
                    scanner.Range(startIndex + 1, scanner.Position - startIndex - 2),
                    Runtime // share the wealth
                );
                _tokens.AddLast(eval);
            }
            else {
                _tokens.AddLast(val);
            }
            scanner.SkipWhiteSpace();
            return scanner.Position;
        }
        
        private object EvaluateList()
        {
            LinkedList<object> list = new LinkedList<object>(_tokens);

            // evaluate unary operators

            LinkedListNode<object> x = list.First;
            while (x != null) {
                UnaryOperator? op = x.Value as UnaryOperator?;
                if (op != null) {
                    var node = op.Value.Side == UnaryOperator.OperandSide.Right ? x.Next : x.Previous;
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

            IExpressionEvaluator expr = list.First.Value as IExpressionEvaluator;
            if (expr == null) {
                return list.First.Value;
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
        public static object Evaluate(IEnumerable<char> expressionString, TBasic exec)
        {
            ExpressionEvaluator expression = new ExpressionEvaluator(expressionString, exec);
            return expression.Evaluate();
        }

        public static object PerformUnaryOp(TBasic runtime, UnaryOperator op, object operand)
        {
            IExpressionEvaluator tempv = operand as IExpressionEvaluator;
            if (tempv != null)
                operand = tempv.Evaluate();

            try {
                return op.ExecuteOperator(runtime, operand);
            }
            catch(InvalidCastException) when (operand is IOperator) {
                throw new ArgumentException("Unary operand cannot be " + operand.GetType().Name);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is FormatException || ex is ArgumentException || ex is OverflowException) {
                throw new ArgumentException("Unary operator '" + op.OperatorString + "' not defined.");
            }
        }

        /// <summary>
        /// Performs a binary operation
        /// </summary>
        public static object PerformBinaryOp(TBasic runtime, BinaryOperator op, object left, object right)
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
                return 0;
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
