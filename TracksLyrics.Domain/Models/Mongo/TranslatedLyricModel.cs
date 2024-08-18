using Common.Base.Interfaces.Models;

namespace TracksLyrics.Domain.Models.Mongo;

public class TranslatedLyricModel : IEntityModel<Guid>
{
    public Guid Id { get; set; }

    public List<string> TranslatedLyricsJson { get; set; } = new();
}