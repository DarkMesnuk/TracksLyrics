namespace TracksLyrics.Web.Requests;

public class GetTrackLyricRequest
{
    public string? Artist { get; set; }
    public string? SongName { get; set; }
}