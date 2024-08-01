using Common.Base.Interfaces;
using TracksLyrics.Domain.Models;

namespace TracksLyrics.Domain.Interfaces.Repositories;

public interface ITrackLyricsRepository : IBaseRepository<TrackLyricModel, int>
{
    Task<TrackLyricModel?> GetByInfoOrDefaultAsync(TrackInfoModel trackInfo);
    Task<TrackLyricModel> UpdateLyricsAsync(TrackLyricModel trackLyric);
}