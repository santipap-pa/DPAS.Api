using System.Linq.Dynamic.Core;
using DPAS.Api.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DPAS.Api.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyDynamicOrdering<T>(this IQueryable<T> query, string orderByQuery)
    {
        if (string.IsNullOrWhiteSpace(orderByQuery))
        {
            return query;
        }

        return query.OrderBy(orderByQuery);
    }

    public static IQueryable<T> ApplyDynamicFiltering<T>(this IQueryable<T> query, string filterExpression, object[] values)
    {
        if (string.IsNullOrWhiteSpace(filterExpression))
        {
            return query;
        }

        return query.Where(filterExpression, values);
    }

    public static async Task<PaginatedResultModel<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResultModel<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}