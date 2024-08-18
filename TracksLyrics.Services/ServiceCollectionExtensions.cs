using Microsoft.Extensions.DependencyInjection;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Services.Implementations;

namespace TracksLyrics.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ITrackLyricService, TrackLyricLyricsService>()
            .AddScoped<ITransmitterMusicService, TransmitterMusicService>()
            .AddScoped<IParsersService, ParsersService>()
            .AddScoped<ISpotifyControlService, SpotifyControlService>();
    }
}
