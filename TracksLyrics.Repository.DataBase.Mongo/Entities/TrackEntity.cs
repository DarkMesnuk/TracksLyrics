using Common.Base.Interfaces.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace TracksLyrics.Repository.DataBase.Entities.Mongo;

public class TrackEntity : IMongoEntity<Guid>
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Artist { get; set; }
    
    [BsonIgnore]
    public TranslatedLyricEntity? TranslatedLyric { get; set; }

    [BsonIgnore]
    public LyricEntity? Lyric { get; set; }
    
    public void SetNewUniqueIdValue() => Id = Guid.NewGuid();
}