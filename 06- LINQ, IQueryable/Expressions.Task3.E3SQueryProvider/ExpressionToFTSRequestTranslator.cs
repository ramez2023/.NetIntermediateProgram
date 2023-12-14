using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Nodes;
using Expressions.Task3.E3SQueryProvider.Models.Request;
using Newtonsoft.Json;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        readonly FtsQueryRequest _ftsQueryRequest;
        public ExpressionToFtsRequestTranslator()
        {
            _ftsQueryRequest = new FtsQueryRequest();
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        public string TranslateToFts(Expression exp)
        {
            Visit(exp);
            return JsonConvert.SerializeObject(_ftsQueryRequest.Statements); ;
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
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType != ExpressionType.MemberAccess && node.Right.NodeType != ExpressionType.MemberAccess)
                        throw new NotSupportedException($"One operand should be property or field: {node.NodeType}");

                    if (node.Right.NodeType != ExpressionType.Constant && node.Left.NodeType != ExpressionType.Constant)
                        throw new NotSupportedException($"One operand should be constant: {node.NodeType}");

                    Visit(node.Left.NodeType == ExpressionType.MemberAccess ? node.Left : node.Right);
                    _resultStringBuilder.Append("(");
                    Visit(node.Right.NodeType == ExpressionType.Constant ? node.Right : node.Left);
                    _resultStringBuilder.Append(")");
                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    if (!string.IsNullOrEmpty(_resultStringBuilder.ToString()))
                    {
                        _ftsQueryRequest.Statements.Add(new Statement { Query = _resultStringBuilder.ToString() });
                        _resultStringBuilder.Clear();
                    }
                    Visit(node.Right);
                    if (!string.IsNullOrEmpty(_resultStringBuilder.ToString()))
                    {
                        _ftsQueryRequest.Statements.Add(new Statement { Query = _resultStringBuilder.ToString() });
                        _resultStringBuilder.Clear();
                    }
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
