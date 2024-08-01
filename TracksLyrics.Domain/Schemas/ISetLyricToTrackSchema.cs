namespace TracksLyrics.Domain.Schemas;

public interface ISetLyricToTrackSchema
{
    bool IsTranslate { get; init; }
    List<string> Lyrics { get; init; }
}