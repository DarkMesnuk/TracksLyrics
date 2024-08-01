using TracksLyrics.Domain.Schemas;

namespace TracksLyrics.Web.Requests;

public class SetLyricToCurrentTrackRequest : ISetLyricToTrackSchema
{
    public required bool IsTranslate { get; init; }
    public required List<string> Lyrics { get; init; }
}