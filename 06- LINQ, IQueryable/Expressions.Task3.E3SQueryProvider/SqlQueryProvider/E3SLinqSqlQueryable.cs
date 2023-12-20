using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.SqlQueryProvider;

public class E3SLinqSqlQueryable<TElement> : IQueryable<TElement>
{
    private readonly E3SLinqSqlProvider _provider;
    private readonly Expression _expression;

    public E3SLinqSqlQueryable(E3SLinqSqlProvider provider, Expression expression)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    public Type ElementType => typeof(TElement);

    public Expression Expression => _expression;

    public IQueryProvider Provider => _provider;

    public IEnumerator<TElement> GetEnumerator()
    {
        // Use the provider's Execute method to execute the query and return the results
        return _provider.Execute<IEnumerable<TElement>>(_expression).GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}