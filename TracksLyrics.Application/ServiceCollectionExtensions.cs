using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TracksLyrics.Application.BackGround;
using TracksLyrics.Application.Handlers;
using TracksLyrics.Application.Mappings;
using TracksLyrics.Application.PipelineBehaviors;

namespace TracksLyrics.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHostedService<TransmitterWorker>();
        services.AddAutoMapper(typeof(DtosMappings).Assembly);
        
        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetLyricsCommandHandler).Assembly))
            .AddValidatorsFromAssembly(typeof(GetTrackLyricRequestValidator).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    }
}