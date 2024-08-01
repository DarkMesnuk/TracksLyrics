using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Common.Schemas;
using Common.Base.Interfaces.Entities;
using Common.Base.Interfaces.Requests;

namespace Common.Extensions;

public static class RepositoryExtension
{
    public static IPaginatedResponseSchema<E> Page<E>(this IQueryable<E> query, IPaginatedRequest schema)
        where E : class, IEntity, new()
    {
        var totalCount = query.Count();

        var entities = query.Skip(schema.PageNumber * schema.PageSize)
            .Take(schema.PageSize)
            .ToList();

        var response = new PaginatedResponseSchema<E>
        {
            TotalCount = totalCount,
            Count = entities.Count,
            Dtos = entities,
        };

        return response;
    }
    
    public static async Task<IPaginatedResponseSchema<E>> PageAsync<E>(this IQueryable<E> query, IPaginatedRequest schema)
        where E : class, IEntity, new()
    {
        var totalCount = query.Count();
        
        List<E> entities;
        
        if(schema.PageSize == -1)
            entities = await query.ToListAsync();
        else
            entities = await query.Skip(schema.PageNumber * schema.PageSize)
                .Take(schema.PageSize).ToListAsync();

        var response = new PaginatedResponseSchema<E>
        {
            TotalCount = totalCount,
            Count = entities.Count,
            Dtos = entities,
        };

        return response;
    }

    public static E? GetById<E, ET>(this IQueryable<E> query, ET id)
        where E : class, IEntity<ET>, new()
    {
        return query.FirstOrDefault(e => Equals(e.Id, id));
    }

    public static Task<E?> GetByIdAsync<E, ET>(this IQueryable<E> query, ET id)
        where E : class, IEntity<ET>, new()
    {
        return query.FirstOrDefaultAsync(e => Equals(e.Id, id));
    }

    public static Task<E?> GetByFilterAsync<E>(this IQueryable<E> query, Expression<Func<E, bool>> func)
        where E : class, IEntity, new()
    {
        return query.FirstOrDefaultAsync(func);
    }

    public static IQueryable<E> FilterByIds<E, ET>(this IQueryable<E> query, IEnumerable<ET> ids)
        where E : class, IEntity<ET>, new()
    {
        return query.Where(e => ids.Contains(e.Id));
    }
}