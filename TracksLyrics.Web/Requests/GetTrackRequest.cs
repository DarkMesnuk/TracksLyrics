namespace TracksLyrics.Web.Requests;

public class GetTrackRequest
{
    public required string Artist { get; set; }
    public required string Title { get; set; }
}