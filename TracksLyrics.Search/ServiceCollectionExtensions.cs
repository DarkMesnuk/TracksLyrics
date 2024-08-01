using Microsoft.Extensions.DependencyInjection;
using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Search.Parsers;

namespace TracksLyrics.Search;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
    {
        return services
            .AddScoped<IGeniusParserService, GeniusParserService>()
            .AddScoped<IMuztextParserService, MuztextParserService>()
            .AddScoped<IMusixmatchParserService, MusixmatchParserService>();
    }
}
