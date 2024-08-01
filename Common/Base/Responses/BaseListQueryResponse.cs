using AutoMapper;
using Common.Models;
using Common.Base.Interfaces.Dtos;
using Common.Base.Interfaces.Models;
using Common.Helpers;

namespace Common.Base.Responses;

public class BaseListQueryResponse<T, TDto, TModel> : ApplicationResponse<T>
    where T : ApplicationResponse
    where TDto : IEntityDto, new()
    where TModel : IEntityModel, new()
{
    protected readonly IMapper Mapper;

    protected BaseListQueryResponse(IMapper mapper)
    {
        Mapper = mapper;
    }

    public IEnumerable<TDto> Dtos { get; set; }

    public T SetData(IEnumerable<TModel> model)
    {
        SetData(StatusCodes.Success);

        Dtos = Mapper.Map<IEnumerable<TDto>>(model);

        return (this as T)!;
    }
}