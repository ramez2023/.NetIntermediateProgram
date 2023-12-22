using System;
using System.Linq;
using System.Linq.Expressions;
using WEBAPI.Common.Enums;

namespace WEBAPI.Common.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> OrderByCustom<TEntity>(this IQueryable<TEntity> items, string sortBy, SortDirection direction)
    {
        var type = typeof(TEntity);
        var expression2 = Expression.Parameter(type, "t");
        var property = type.GetProperty(sortBy);

        var expression1 = Expression.MakeMemberAccess(expression2, property);

        var lambda = Expression.Lambda(expression1, expression2);
        
        var result = Expression.Call(
            typeof(Queryable),
            direction == SortDirection.Desc ? "OrderByDescending" : "OrderBy",
            new Type[] { type, property.PropertyType },
            items.Expression,
            Expression.Quote(lambda));

        return items.Provider.CreateQuery<TEntity>(result);
    }
}