using Common.Models;
using MediatR;

namespace Common.Base.Requests.Handlers;

public abstract class BaseHandlerRequest<TResponse> : IRequest<TResponse>
    where TResponse : ApplicationResponse
{
}