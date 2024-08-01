using FluentValidation;
using MediatR;

namespace TracksLyrics.Application.PipelineBehaviors;

internal class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
    ) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return next();
            
        var context = new ValidationContext<TRequest>(request);
        var errors = validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();
            
        if (errors.Count != 0)
            throw new ValidationException(errors);
            
        return next();
    }
}