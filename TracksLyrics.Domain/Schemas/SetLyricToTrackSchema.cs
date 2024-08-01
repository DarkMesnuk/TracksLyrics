namespace TracksLyrics.Domain.Schemas;

public class SetLyricToTrackSchema : ISetLyricToTrackSchema
{
    public required bool IsTranslate { get; init; }
    public required List<string> Lyrics { get; init; }
}