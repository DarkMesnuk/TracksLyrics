using Common.Base.Interfaces.Dtos;

namespace TracksLyrics.Application.Dtos;

public class TrackDto : IEntityDto
{
    public string Name { get; set; }
    public string Artist { get; set; }
    public bool IsTranslated { get; set; }
    public List<string>? CombinedLyrics { get; set; }
    public List<string>? Lyrics { get; set; }
    public List<string>? TranslatedLyrics { get; set; }
}