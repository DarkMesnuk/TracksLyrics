using Common.Base.Interfaces.Requests;
using Common.Models;

namespace Common.Base.Requests.Handlers;

public abstract class BasePaginatedHandlerRequest<TResponse> : BaseHandlerRequest<TResponse>, IPaginatedRequest
    where TResponse : ApplicationResponse
{
    public int PageNumber { get; init; } = 0;
    
    public int PageSize { get; init; } = 10;
    
    public string? Search { get; init; }
}