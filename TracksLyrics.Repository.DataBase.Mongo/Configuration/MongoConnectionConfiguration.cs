namespace TracksLyrics.Repository.DataBase.Configuration;

public class MongoConnectionConfiguration
{
    public const string ConfigSectionName = "MongoDatabase";

    public string ConnectionString { get; set; }

    public string Name { get; set; }
}