namespace TracksLyrics.Repository.DataBase.Configuration;

public class PostgreSqlConnectionConfiguration
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public bool IntegratedSecurity { get; set; }
    public int CommandTimeout { get; set; }

    public bool Pooling { get; set; } = false;
    public int MinPoolSize { get; set; } = 0;
    public int MaxPoolSize { get; set; } = 100;
    public int ConnectionIdleLifetime { get; set; } = 300;
    public int ConnectionPruningInterval { get; set; } = 10;
}