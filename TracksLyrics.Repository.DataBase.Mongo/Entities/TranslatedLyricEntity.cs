using Common.Base.Interfaces.Entities;

namespace TracksLyrics.Repository.DataBase.Entities.Mongo;

public class TranslatedLyricEntity : IMongoEntity<Guid>
{
    public Guid Id { get; set; }

    public string? TranslatedLyricsJson { get; set; }
    
    public void SetNewUniqueIdValue() => Id = Guid.NewGuid();
}