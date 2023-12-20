using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider.SqlQueryProvider
{
    public class ExpressionToSqlTranslator : ExpressionVisitor
    {
        private readonly StringBuilder _whereBuilder;
        private readonly Type _itemType;
        private string _logicalOperator = "AND";


        public ExpressionToSqlTranslator(Type itemType)
        {
            _itemType = itemType;
            _whereBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);
            return _whereBuilder.Length == 0
                ? $"SELECT {GetSelectClause()} FROM {GetTableName()}"
                : $"SELECT {GetSelectClause()} FROM {GetTableName()} WHERE {_whereBuilder}";
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Where" && node.Method.DeclaringType == typeof(Queryable))
            {
                var predicate = node.Arguments[1];
                Visit(predicate);
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

        #endregion



        #region Helper
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
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append($" {_logicalOperator} ");
            }

            _whereBuilder.Append(condition);
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

        private string GetTableName()
        {
            return _itemType.Name;
        }
        private string GetSelectClause()
        {
            var properties = _itemType.GetProperties().Select(property => property.Name);
            return string.Join(", ", properties);
        }
        #endregion
    }
}
