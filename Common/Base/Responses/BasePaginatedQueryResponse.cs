using AutoMapper;
using Common.Models;
using Common.Schemas;
using Common.Base.Interfaces.Dtos;
using Common.Base.Interfaces.Models;
using Common.Helpers;

namespace Common.Base.Responses;

public abstract class BasePaginatedQueryResponse<T, TDto, TModel>(
    IMapper mapper
    ) : BaseListQueryResponse<T, TDto, TModel>(mapper), IPaginatedResponseSchema<TDto>
    where T : ApplicationResponse
    where TDto : IEntityDto, new()
    where TModel : IEntityModel, new()
{

    public int Count { get; set; }
    public int TotalCount { get; set; }

    public T SetData(IPaginatedResponseSchema<TModel> schema)
    {
        SetData(StatusCodes.Success);

        Count = schema.Count;
        TotalCount = schema.TotalCount;
        Dtos = Mapper.Map<IEnumerable<TDto>>(schema.Dtos);

        return (this as T)!;
    }
}