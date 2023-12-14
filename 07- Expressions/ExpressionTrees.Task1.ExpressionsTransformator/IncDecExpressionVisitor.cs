using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer;

public class IncDecExpressionVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _variable;

    public IncDecExpressionVisitor(ParameterExpression variable)
    {
        _variable = variable ?? throw new ArgumentNullException(nameof(variable));
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (IsIncrementOrDecrement(node))
        {
            return node.NodeType switch
            {
                ExpressionType.Add => Expression.Increment(_variable),
                ExpressionType.Subtract => Expression.Decrement(_variable),
                _ => base.VisitBinary(Expression.MakeBinary(node.NodeType, _variable, Expression.Constant(1)))
            };
        }

        return base.VisitBinary(node);
    }

    private bool IsIncrementOrDecrement(BinaryExpression node)
    {
        return IsVariable(node.Left) && IsOneConstant(node.Right);
    }

    private bool IsVariable(Expression expression)
    {
        return expression is ParameterExpression parameter && parameter == _variable;
    }

    private bool IsOneConstant(Expression expression)
    {
        return expression is ConstantExpression { Value: int and 1 };
    }
}