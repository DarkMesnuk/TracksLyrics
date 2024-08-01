using Common.Base.Interfaces.Models;

namespace TracksLyrics.Domain.Models;

public class TrackLyricModel : IEntityModel<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
        
    public string Artist { get; set; } = string.Empty;

    public List<string> Lyrics { get; set; } = new();

    public List<string> TranslatedLyrics { get; set; } = new();

    public bool IsTranslated => TranslatedLyrics.Any();

    public bool IsFinded => Lyrics.Any();

    public TrackLyricModel() 
    {
    }

    public TrackLyricModel(string name, string artist, List<string> lyrics) : this()
    {
        Name = name;
        Artist = artist;
        Lyrics = lyrics;
    }

    public TrackLyricModel(string name, string artist, List<string> lyrics, List<string> translatedLyrics) : this(name, artist, lyrics)
    {
        TranslatedLyrics = translatedLyrics;
    }
    
    public override string ToString()
    {
        return Name + " - " + Artist;
    }
}