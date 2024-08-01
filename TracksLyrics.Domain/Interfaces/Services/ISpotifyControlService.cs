namespace TracksLyrics.Domain.Interfaces.Services;

public interface ISpotifyControlService
{
    Task<bool> NextTrackAsync();
}