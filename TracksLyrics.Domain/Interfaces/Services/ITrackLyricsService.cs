using Common.Base.Interfaces;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Schemas;

namespace TracksLyrics.Domain.Interfaces.Services;

public interface ITrackLyricsService : IBaseService<TrackLyricModel, int>
{
    Task<bool> SetLyricToTrackAsync(TrackInfoModel trackInfo, ISetLyricToTrackSchema schema);
    Task<TrackLyricModel> ParseAndSaveTrackLyricAsync(TrackInfoModel trackInfo, IParsersService parsersService);
    Task<TrackLyricModel> UpdateLyricsAsync(TrackLyricModel trackLyric);
}