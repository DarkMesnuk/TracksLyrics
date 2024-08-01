using Common.Base.Interfaces;
using Common.Base.Interfaces.Models;

namespace Common.Base;

public abstract class BaseService<TModel, TId>(IBaseRepository<TModel, TId> baseRepository) : IBaseService<TModel, TId>
    where TModel : class, IEntityModel<TId>, new()
{
    protected readonly IBaseRepository<TModel, TId> BaseRepository = baseRepository;

    public virtual Task<TModel?> GetAsync(TId id)
    {
        return BaseRepository.GetByIdAsync(id);
    }

    public Task<TModel?> UpdateAsync(TModel model)
    {
        return BaseRepository.UpdateAsync(model);
    }

    public Task<bool> UpdateRangeAsync(IEnumerable<TModel> models)
    {
        return BaseRepository.UpdateRangeAsync(models);
    }

    public Task<bool> DeleteAsync(TId id)
    {
        return BaseRepository.DeleteAsync(id);
    }

    public Task<bool> DeleteAsync(TModel model)
    {
        return BaseRepository.DeleteAsync(model);
    }

    public Task<bool> DeleteRangeAsync(IEnumerable<TModel> models)
    {
        return BaseRepository.DeleteRangeAsync(models);
    }
}