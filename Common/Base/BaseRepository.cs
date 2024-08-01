using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Common.Extensions;
using Common.Schemas;
using Common.Base.Interfaces;
using Common.Base.Interfaces.Entities;
using Common.Base.Interfaces.Models;
using Common.Base.Interfaces.Requests;
using Common.Exceptions;

namespace Common.Base;

public abstract class BaseRepository<TEntity, TModel, TId>(DbContext context, ILogger<BaseRepository<TEntity, TModel, TId>> logger, IMapper mapper) : IDisposable, IBaseRepository<TModel, TId>
    where TEntity : class, IEntity<TId>, new()
    where TModel : class, IEntityModel<TId>, new()
{
    protected TEntity MapToEntity(TModel model) => mapper.Map<TEntity>(model);
    
    protected IEnumerable<TEntity> MapToEntity(IEnumerable<TModel> models) => mapper.Map<IEnumerable<TEntity>>(models);
    
    protected TModel MapToModel(TEntity entity) => mapper.Map<TModel>(entity);
    
    protected IEnumerable<TModel> MapToModel(IEnumerable<TEntity> entities) => mapper.Map<IEnumerable<TModel>>(entities);
    protected ICollection<TModel> MapToModel(ICollection<TEntity> entities) => mapper.Map<ICollection<TModel>>(entities);
    
    protected IPaginatedResponseSchema<TModel> MapToModel(IPaginatedResponseSchema<TEntity> schema)
    {
        return new PaginatedResponseSchema<TModel>
        {
            Dtos = MapToModel(schema.Dtos),
            Count = schema.Count,
            TotalCount = schema.TotalCount
        };
    }

    protected DbSet<TEntity> DbSet => context.Set<TEntity>();

    protected IQueryable<TEntity> GetBase => DbSet.AsQueryable()
        .AsNoTracking();
    
    public bool Exists(TId id)
    {
        var model = GetBase.GetById(id);
        return model != null;
    }
    
    protected bool Exists(Expression<Func<TEntity, bool>> func)
    {
        return GetBase.Any(func);
    }
    
    public async Task<TModel> CreateAsync(TModel model)
    {
        try
        {
            var entity = MapToEntity(model);
            var query = DbSet.Add(entity);

            entity = query.Entity;
            
            await context.SaveChangesAsync();
            
            context.Entry(entity).State = EntityState.Detached;
            
            model = MapToModel(entity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} create failed");
            throw new CreationFailedException();
        }

        return model;
    }
    
    public async Task<bool> CreateAsync(IEnumerable<TModel> models)
    {
        try
        {
            var entities = MapToEntity(models);

            DbSet.AddRange(entities);
            
            await context.SaveChangesAsync();
            
            foreach (var entity in entities)
                context.Entry(entity).State = EntityState.Detached;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range creations failed");
            throw new CreationFailedException();
        }

        return true;
    }
    
    public virtual async Task<TModel> GetByIdAsync(TId id)
    {
        var entity = await GetBase.GetByIdAsync(id);

        if (entity.IsNullOrDefault())
            throw new NotFoundException(typeof(TModel).Name, id.ToString());

        return MapToModel(entity);
    }
    
    public virtual async Task<IPaginatedResponseSchema<TModel>> GetAllAsync(IPaginatedRequest schema)
    {
        var entities = await GetBase
            .PageAsync(schema);

        return MapToModel(entities);
    }
    
    protected async virtual Task<TEntity> GetEntityByIdAsync(TId id)
    {
        var entity = await GetBase.GetByIdAsync(id);

        if (entity.IsNullOrDefault())
            throw new NotFoundException(typeof(TModel).Name, id.ToString());

        return entity;
    }
    
    protected async virtual Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> func)
    {
        var entity = await GetBase.GetByFilterAsync(func);

        if (entity.IsNullOrDefault())
            throw new NotFoundException(typeof(TModel).Name);

        return entity;
    }

    public async Task<TModel> UpdateAsync(TModel model)
    {
        try
        {
            var entity = MapToEntity(model);
            
            SetModifiedState(entity);

            var query = DbSet.Update(entity);

            entity = query.Entity;

            await context.SaveChangesAsync();

            model = MapToModel(entity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} update failed");
            throw new UpdateFailedException();
        }

        return model;
    }

    public async Task<bool> UpdateRangeAsync(IEnumerable<TModel> models)
    {
        try
        {
            var entities = MapToEntity(models);
            
            SetModifiedState(entities);

            DbSet.UpdateRange(entities);

            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range update failed");
            throw new UpdateFailedException();
        }

        return true;
    }

    public Task<bool> DeleteAsync(TId id)
    {
        return DeleteAsync(GetBase.GetById(id));
    }

    public Task<bool> DeleteAsync(TModel model)
    {
        return DeleteAsync(model.Id);
    }
    
    protected async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> func)
    {
        try
        {
            var entities = GetBase.Where(func);
            
            SetDeleteState(entities);
            
            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range delete failed");
            throw new DeleteFailedException();
        }

        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<TModel> models)
    {
        try
        {
            var entities = MapToEntity(models);
            
            SetDeleteState(entities);

            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range delete failed");
            throw new DeleteFailedException();
        }

        return true;
    }
    
    public async Task<bool> DeleteRangeAsync(IEnumerable<TId> ids)
    {
        try
        {
            var entities = GetBase.FilterByIds(ids);

            SetDeleteState(entities);
            
            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range delete failed");
            throw new DeleteFailedException();
        }

        return true;
    }

    private async Task<bool> DeleteAsync(TEntity entity)
    {
        try
        {
            SetDeleteState(entity);
            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{nameof(TEntity)}.Id {entity.Id} delete failed");
            throw new DeleteFailedException();
        }

        return true;
    }
    
    protected void SetModifiedState(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            SetModifiedState(entity);
    }

    protected void SetModifiedState(TEntity entity)
    {
        SetDetachedState(entity);
        
        DbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }
    
    protected void SetDeleteState(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            SetDeleteState(entity);
    }

    protected void SetDeleteState(TEntity entity)
    {
        SetDetachedState(entity);
        context.Entry(entity).State = EntityState.Deleted;
    }

    protected void SetDetachedState(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            SetDeleteState(entity);
    }
    
    protected void SetDetachedState(TEntity entity)
    {
        var existingEntity = context.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Id.Equals(entity.Id));
        if (existingEntity != null)
            context.Entry(existingEntity.Entity).State = EntityState.Detached;
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}