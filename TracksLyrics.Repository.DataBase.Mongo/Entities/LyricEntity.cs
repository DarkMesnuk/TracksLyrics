using Common.Base.Interfaces.Entities;

namespace TracksLyrics.Repository.DataBase.Entities.Mongo;

public class LyricEntity : IMongoEntity<Guid>
{
    public Guid Id { get; set; }

    public string? LyricsJson { get; set; }
    
    public void SetNewUniqueIdValue() => Id = Guid.NewGuid();
}