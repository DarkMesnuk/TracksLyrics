using Npgsql;
using TracksLyrics.Repository.DataBase.Configuration;

namespace TracksLyrics.Web.Configuration;

public static class ConfigurationExtensions
{
    public static string GetPostgreSqlConnectionString(this IConfiguration configuration)
    {
        var config = configuration.GetPostgreDbConnectionConfiguration();
        
        NpgsqlConnectionStringBuilder builder = new ()
        {
            Host = config.Host,
            Port = config.Port,
            Database = config.Name,
            Username = config.User,
            Password = config.Password,
            CommandTimeout = config.CommandTimeout
        };

        if (config.Pooling)
        {
            builder.Pooling = config.Pooling;
            builder.MinPoolSize = config.MinPoolSize;
            builder.MaxPoolSize = config.MaxPoolSize;
            builder.ConnectionIdleLifetime = config.ConnectionIdleLifetime;
            builder.ConnectionPruningInterval = config.ConnectionPruningInterval;
        }

        return builder.ToString();
    }

    public static PostgreSqlConnectionConfiguration? GetPostgreDbConnectionConfiguration(this IConfiguration configuration) =>
        configuration.GetSection("PostgreDatabase").Get<PostgreSqlConnectionConfiguration>();
    
    public static MongoConnectionConfiguration? GetMongoConnectionConfiguration(this IConfiguration configuration) =>
        configuration.GetSection("MongoDatabase").Get<MongoConnectionConfiguration>();

    public static int GetDbContextPoolSize(this IConfiguration configuration) => 
        configuration.GetValue("Database:MaxPoolSize", 124);
}