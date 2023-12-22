using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Common.ViewModels;

namespace WEBAPI.Common.Extensions;

public static class PaginationExtensions
{
    public static async Task<PagedList<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, 
        int number, int size, Expression<Func<TSource, TResult>> target)
    {
        var pagination = (number == 0 || size == 0) ? null : new PaginationVm { PageSize = size, PageNumber = number };

        var pagedList = new PagedList<TResult>(query.Count(), pagination);

        if (pagination != null)
            query = query.Skip(pagedList.SkipCount).Take(pagedList.TakeCount);

        pagedList.List = await query.Select(target).ToListAsync();
        return pagedList;
    }

    public static PagedList<TResult> Paginate<TSource, TResult>(this IQueryable<TSource> query, 
        int number, int size, Expression<Func<TSource, TResult>> target)
    {

        var pagination = (number == 0 || size == 0) ? null : new PaginationVm { PageSize = size, PageNumber = number };

        var pagedList = new PagedList<TResult>(query.Count(), pagination);

        if (pagination != null)
            query = query.Skip(pagedList.SkipCount).Take(pagedList.TakeCount);

        pagedList.List = query.Select(target).ToList();
        return pagedList;
    }
}