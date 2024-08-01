using AutoMapper;
using Common.Models;
using Common.Base.Interfaces.Dtos;
using Common.Base.Interfaces.Models;
using Common.Helpers;

namespace Common.Base.Responses;

public class BaseQueryResponse<T, TDto, TModel>(IMapper mapper) : ApplicationResponse<T>
    where T : ApplicationResponse
    where TDto : IEntityDto, new()
    where TModel : IEntityModel, new()
{
    public TDto Dto { get; protected set; }
    
    public T SetData(TModel model)
    {
        SetData(StatusCodes.Success);
        
        Dto = mapper.Map<TDto>(model);

        return (this as T)!;
    }
}

public class BaseQueryResponse<T, TDto> : ApplicationResponse<T>
    where T : ApplicationResponse
{
    public TDto Dto { get; protected set; }

    public T SetData(TDto dto)
    {
        SetData(StatusCodes.Success);
        Dto = dto;
        return (this as T)!;
    }
}