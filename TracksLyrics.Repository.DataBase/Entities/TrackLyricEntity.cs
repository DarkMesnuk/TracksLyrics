using Common.Base.Interfaces.Entities;

namespace TracksLyrics.Repository.DataBase.Entities;

public class TrackLyricEntity : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string? LyricsJson { get; set; }
    public string? TranslatedLyricsJson { get; set; }
}