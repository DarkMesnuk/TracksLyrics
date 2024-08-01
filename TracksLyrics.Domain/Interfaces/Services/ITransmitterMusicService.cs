using TracksLyrics.Domain.Models;

namespace TracksLyrics.Domain.Interfaces.Services;

public interface ITransmitterMusicService : IDisposable
{
    void GetCurrentPlayingTrack(Action<TrackInfoModel?> trackAction);
}