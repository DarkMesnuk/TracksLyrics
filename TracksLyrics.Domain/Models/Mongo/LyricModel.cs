using Common.Base.Interfaces.Models;

namespace TracksLyrics.Domain.Models.Mongo;

public class LyricModel : IEntityModel<Guid>
{
    public Guid Id { get; set; }

    public List<string> LyricsJson { get; set; } = new();
}