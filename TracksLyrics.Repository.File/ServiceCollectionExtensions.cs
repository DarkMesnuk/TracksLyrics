using Microsoft.Extensions.DependencyInjection;
using TracksLyrics.Domain.Interfaces.Repositories;
using TracksLyrics.Repository.File.Repositories;

namespace TracksLyrics.Repository.File;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileRepository(this IServiceCollection services)
    {
        return services
            .AddScoped<ITrackLyricsRepository, TrackLyricsFileRepository>();
    }
}
