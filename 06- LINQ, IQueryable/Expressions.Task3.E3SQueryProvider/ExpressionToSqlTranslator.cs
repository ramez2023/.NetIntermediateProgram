using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToSqlTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        private string _logicalOperator = "AND";


        public ExpressionToSqlTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);
            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            switch (node.Method.Name)
            {
                case "Where":
                    if (node.Method.DeclaringType == typeof(Queryable))
                    {
                        var predicate = node.Arguments[1];
                        Visit(predicate);
                        return node;
                    }
                    break;

                case "Equals":
                    Visit(node.Object);
                    _resultStringBuilder.Append("(");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append(")");
                    return node;

                case "Contains":
                    Visit(node.Object);
                    _resultStringBuilder.Append("(*");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append("*)");
                    return node;

                case "StartsWith":
                    Visit(node.Object);
                    _resultStringBuilder.Append("(");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append("*)");
                    return node;

                case "EndsWith":
                    Visit(node.Object);
                    _resultStringBuilder.Append("(*");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append(")");
                    return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (IsComparisonOperation(node.NodeType))
            {
                if (node.Left is MemberExpression left && node.Right is ConstantExpression right)
                {
                    string condition = $"{left.Member.Name} {GetOperator(node.NodeType)} {GetValue(right.Value)}";
                    AppendCondition(condition);
                    return node;
                }
            }

            if (node.NodeType == ExpressionType.AndAlso)
            {
                _logicalOperator = "AND";
                return base.VisitBinary(node);
            }

            if (node.NodeType == ExpressionType.OrElse)
            {
                _logicalOperator = "OR";
                return base.VisitBinary(node);
            }

            return base.VisitBinary(node);
        }

        private bool IsComparisonOperation(ExpressionType nodeType)
        {
            return nodeType == ExpressionType.Equal || nodeType == ExpressionType.GreaterThan || nodeType == ExpressionType.LessThan;
        }

        private string GetOperator(ExpressionType nodeType)
        {
            return nodeType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.LessThan => "<",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThanOrEqual => "<=",
            };
        }

        private void AppendCondition(string condition)
        {
            if (_resultStringBuilder.Length > 0)
            {
                _resultStringBuilder.Append($" {_logicalOperator} ");
            }

            _resultStringBuilder.Append(condition);
        }


        private string GetValue(object value)
        {
            return value switch
            {
                string => $"'{value}'",
                null => "NULL",
                _ => value.ToString()
            };
        }

        #endregion
    }
}
