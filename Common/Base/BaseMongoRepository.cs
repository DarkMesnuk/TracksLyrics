using AutoMapper;
using Common.Base.Interfaces;
using Common.Base.Interfaces.Entities;
using Common.Base.Interfaces.Models;
using Common.Base.Interfaces.Requests;
using Common.Exceptions;
using Common.Extensions;
using Common.Schemas;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Common.Base;

public abstract class BaseMongoRepository<TEntity, TModel, TId>(
    ILogger<BaseMongoRepository<TEntity, TModel, TId>> logger, 
    IMongoCollection<TEntity> collection, 
    IMapper mapper
    ) :
    IDisposable,
    IBaseRepository<TModel, TId>
    where TEntity : class, IMongoEntity<TId>, new()
    where TModel : class, IEntityModel<TId>, new()
{
    protected readonly IMongoCollection<TEntity> _collection = collection;
    
    public void Dispose() => GC.SuppressFinalize((object) this);

    protected IMongoQueryable<TEntity> GetBase => MongoQueryable.OfType<TEntity>(this._collection.AsQueryable<TEntity>(new AggregateOptions()));

    public bool Exists(TId id) => (object) this.GetBase.GetById<TEntity, TId>(id) != null;
    
    protected TEntity? MapToEntity(TModel model) => mapper.Map<TEntity>(model);
    
    protected IEnumerable<TEntity>? MapToEntity(IEnumerable<TModel> models) => mapper.Map<IEnumerable<TEntity>>(models);
    
    protected TModel? MapToModel(TEntity entity) => mapper.Map<TModel>(entity);
    
    protected IEnumerable<TModel>? MapToModel(IEnumerable<TEntity> entities) => mapper.Map<IEnumerable<TModel>>(entities);
    
    protected IPaginatedResponseSchema<TModel> MapToModel(IPaginatedResponseSchema<TEntity> schema)
    {
        return new PaginatedResponseSchema<TModel>
        {
            Dtos = MapToModel(schema.Dtos),
            Count = schema.Count,
            TotalCount = schema.TotalCount
        };
    }

    public async Task<TModel> CreateAsync(TModel model)
    {
        var entity = MapToEntity(model);
        var options = new InsertOneOptions();

        entity.SetNewUniqueIdValue();

        await _collection.InsertOneAsync(entity, options);
            
        return MapToModel(entity);
    }

    public async Task<bool> CreateAsync(IEnumerable<TModel> models)
    {
        try
        {
        var entities = MapToEntity(models);

        foreach (var entity in entities)
            entity.SetNewUniqueIdValue();
        
        await _collection.InsertManyAsync(entities);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range creations failed");
            throw new CreationFailedException();
        }

        return true;
    }

    public async Task<TModel> GetByIdAsync(TId id)
    {
        var entity = await GetEntityByIdAsync(id);
        return MapToModel(entity);
    }
    
    protected async Task<TEntity?> GetEntityByIdAsync(TId id)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
        
        var entity = await _collection.Find(filter).FirstOrDefaultAsync();

        if (entity.IsNullOrDefault())
            throw new NotFoundException(typeof(TModel).Name, id.ToString());

        return entity;
    }

    public async Task<IPaginatedResponseSchema<TModel>> GetAllAsync(IPaginatedRequest schema)
    {
        var entities = GetBase.Page(schema);

        return MapToModel(entities);
    }

    public async Task<TModel> UpdateAsync(TModel model)
    {
        try
        {
            var entity = MapToEntity(model);
            
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            await _collection.FindOneAndReplaceAsync(filter!, entity);

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

            foreach (var entity in entities)
            {
                var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
                await _collection.FindOneAndReplaceAsync(filter!, entity);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range update failed");
            throw new UpdateFailedException();
        }

        return true;
    }

    public async Task<bool> DeleteAsync(TId id)
    {
        try
        {
            var options = new DeleteOptions();
            
            var result = await _collection.DeleteOneAsync(entity => entity.Id.Equals(id), options);
            
            return result is { IsAcknowledged: true, DeletedCount: 1 };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range delete failed");
            throw new DeleteFailedException();
        }
    }

    public async Task<bool> DeleteAsync(TModel model)
    {
        var entity = await GetEntityByIdAsync(model.Id);
        
        return await DeleteAsync(model.Id);
    }

    public Task<bool> DeleteRangeAsync(IEnumerable<TModel> models)
    {
        var entities = MapToEntity(models);
        return DeleteRangeAsync(entities);
    }
    
    private Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        
            var ids = entities.Select(x => x.Id);
            return DeleteRangeAsync(ids);
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<TId> ids)
    {
        try
        {
            var options = new DeleteOptions();
            var result = await _collection.DeleteManyAsync(x => ids.Contains(x.Id), options);
            
            return result is { IsAcknowledged: true } && result.DeletedCount == ids.Count();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TModel).Name} range delete failed");
            throw new DeleteFailedException();
        }
    }
}