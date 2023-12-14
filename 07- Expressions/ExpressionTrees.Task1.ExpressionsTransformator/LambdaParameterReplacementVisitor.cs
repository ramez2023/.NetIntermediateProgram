using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer;

public class LambdaParameterReplacementVisitor : ExpressionVisitor
{
    private readonly Dictionary<string, object> _parameterValues;

    public LambdaParameterReplacementVisitor(Dictionary<string, object> parameterValues)
    {
        _parameterValues = parameterValues ?? throw new ArgumentNullException(nameof(parameterValues));
    }

    public Expression<Func<TResult>> Transform<TResult>(LambdaExpression sourceExpression)
    {
        if (sourceExpression == null)
            throw new ArgumentNullException(nameof(sourceExpression));

        var transformedBody = Visit(sourceExpression.Body);
        return Expression.Lambda<Func<TResult>>(transformedBody);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_parameterValues.TryGetValue(node.Name, out var value))
            return Expression.Constant(value, node.Type);

        return base.VisitParameter(node);
    }

}