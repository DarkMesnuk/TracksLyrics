using Common.Schemas;
using Common.Base.Interfaces.Models;
using Common.Base.Interfaces.Requests;

namespace Common.Base.Interfaces;

public interface IBaseRepository<TModel, TId>
    where TModel : class, IEntityModel<TId>, new()
{
    bool Exists(TId id);
    
    Task<TModel> CreateAsync(TModel model);
    Task<bool> CreateAsync(IEnumerable<TModel> models);
    
    Task<TModel> GetByIdAsync(TId id);
    
    Task<IPaginatedResponseSchema<TModel>> GetAllAsync(IPaginatedRequest schema);
    
    Task<TModel> UpdateAsync(TModel model);
    
    Task<bool> UpdateRangeAsync(IEnumerable<TModel> models);
    
    Task<bool> DeleteAsync(TId id);
    Task<bool> DeleteAsync(TModel model);
    Task<bool> DeleteRangeAsync(IEnumerable<TModel> models);
    Task<bool> DeleteRangeAsync(IEnumerable<TId> ids);
}