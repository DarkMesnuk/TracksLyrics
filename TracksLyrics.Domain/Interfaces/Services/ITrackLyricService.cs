using Common.Base.Interfaces;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Domain.Schemas;

namespace TracksLyrics.Domain.Interfaces.Services;

public interface ITrackLyricService : IBaseService<TrackModel, Guid>
{
    Task<bool> SetLyricToTrackAsync(TrackInfoModel trackInfo, ISetLyricToTrackSchema schema);
    Task<TrackModel> ParseAndSaveTrackLyricAsync(TrackInfoModel trackInfo, IParsersService parsersService);
    Task<TrackModel> UpdateLyricsAsync(TrackModel trackLyric);
}