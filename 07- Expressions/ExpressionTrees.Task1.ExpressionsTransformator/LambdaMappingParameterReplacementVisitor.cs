using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer;

public class LambdaMappingParameterReplacementVisitor : ExpressionVisitor
{
    private readonly Dictionary<string, object> _parameterValues;
    private readonly Dictionary<string, string> _fieldMappings;

    public LambdaMappingParameterReplacementVisitor(Dictionary<string, string> fieldMappings)
    {
        _fieldMappings = fieldMappings ?? throw new ArgumentNullException(nameof(fieldMappings));
    }

    public Expression<Func<TDestination>> Transform<TSource, TDestination>(Expression<Func<TSource>> sourceExpression)
    {
        var transformedBody = Visit(sourceExpression.Body);
        return Expression.Lambda<Func<TDestination>>(transformedBody);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_parameterValues.TryGetValue(node.Name, out var value))
            return Expression.Constant(value, node.Type);

        return base.VisitParameter(node);
    }
}