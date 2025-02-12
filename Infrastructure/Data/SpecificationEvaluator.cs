using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    /*
    alternative for:
    if(!string.IsNullOrEmpty(brand))
            query = query.Where(x => x.Brand == brand);
    */  
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        // Check if there are criteria to filter the query
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); //x => x.Brand == brand
        }

        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if(spec.IsDistinct)
        {
            query = query.Distinct();
        }

        return query;
    }

    // Overloaded method to get a query that returns a different type TResult
    public static IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        if(spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); //x => x.Brand == brand
        }

        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // Cast the query to IQueryable<TResult>
        var selectQuery = query as IQueryable<TResult>;

        // Check if there is a select clause
        if (spec.Select != null)
        {
            // Apply the select clause to the query
            selectQuery = query.Select(spec.Select);
        }

        if(spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        // Return the select query or cast the original query to TResult
        return selectQuery ?? query.Cast<TResult>();
    }
}
