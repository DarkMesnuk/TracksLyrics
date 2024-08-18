using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TracksLyrics.Domain.Interfaces.Repositories;
using TracksLyrics.Repository.DataBase.Mappers;
using TracksLyrics.Repository.DataBase.Repositories;

namespace TracksLyrics.Repository.DataBase;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreDatabase(this IServiceCollection services, string connectionString, int poolSize)
    {
        services.AddEntityFrameworkNpgsql()
            .AddDbContextPool<TracksLyricsContext>(
                options => options.UseNpgsql(
                    connectionString, 
                    sqlOptions => 
                        sqlOptions.MigrationsAssembly(typeof(TracksLyricsContext).GetTypeInfo().Assembly.GetName().Name)), 
                poolSize);

        services.AddAutoMapper(typeof(MappingRepositoryProfile).Assembly);

        services.AddScoped<ITrackLyricsRepository, TrackLyricsRepository>();
        
        return services;
    }
}