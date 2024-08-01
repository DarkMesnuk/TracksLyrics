using Common.Base.Interfaces.Models;

namespace Common.Base.Interfaces;

public interface IBaseService<TModel, TId>
    where TModel : IEntityModel
{
    Task<TModel?> GetAsync(TId id);
    Task<TModel?> UpdateAsync(TModel model);
    Task<bool> UpdateRangeAsync(IEnumerable<TModel> models);
    Task<bool> DeleteAsync(TId id);
    Task<bool> DeleteAsync(TModel model);
    Task<bool> DeleteRangeAsync(IEnumerable<TModel> models);
}