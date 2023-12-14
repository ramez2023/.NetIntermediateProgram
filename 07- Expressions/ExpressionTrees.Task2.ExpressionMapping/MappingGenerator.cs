using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping;

public class MappingGenerator
{
    public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
    {
        var sourceParam = Expression.Parameter(typeof(TSource));
        var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(Expression.New(typeof(TDestination)), sourceParam);

        return new Mapper<TSource, TDestination>(mapFunction.Compile());
    }

    public Mapper<TSource, TDestination> Generate<TSource, TDestination>(Dictionary<string, string> fieldMappings)
    {
        var sourceParam = Expression.Parameter(typeof(TSource));
        var memberBindings = new List<MemberBinding>();

        foreach (var mapping in fieldMappings)
        {
            var sourceMember = typeof(TSource).GetProperty(mapping.Key);
            var destinationMember = typeof(TDestination).GetProperty(mapping.Value);

            if (sourceMember != null && destinationMember != null)
            {
                var sourceExpression = Expression.Property(sourceParam, sourceMember);
                memberBindings.Add(Expression.Bind(destinationMember, sourceExpression));
            }
        }

        var memberInitExpression = Expression.MemberInit(Expression.New(typeof(TDestination)), memberBindings);
        var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(memberInitExpression, sourceParam);

        return new Mapper<TSource, TDestination>(mapFunction.Compile());
    }

}