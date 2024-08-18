using Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TracksLyrics.Domain.Interfaces.Repositories.Mongo;
using TracksLyrics.Repository.DataBase.Configuration;
using TracksLyrics.Repository.DataBase.Entities.Mongo;
using TracksLyrics.Repository.DataBase.Mongo.Repositories;

namespace TracksLyrics.Repository.DataBase.Mongo;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, MongoConnectionConfiguration configuration)
    {
        //services.AddAutoMapper(typeof(MappingRepositoryProfile).Assembly);

        services.AddSingleton<IMongoClient>(_ => new MongoClient(configuration.ConnectionString))
            .AddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(configuration.Name));
        
        services.AddMongoCollection<LyricEntity>("TrackLyrics");
        services.AddMongoCollection<TrackEntity>("Tracks");
        services.AddMongoCollection<TranslatedLyricEntity>("TranslatedLyrics");

        services.AddScoped<ITrackRepository, TrackRepository>();
        services.AddScoped<ILyricRepository, LyricRepository>();
        services.AddScoped<ITranslatedLyricRepository, TranslatedLyricRepository>();
        
        return services;
    }
}