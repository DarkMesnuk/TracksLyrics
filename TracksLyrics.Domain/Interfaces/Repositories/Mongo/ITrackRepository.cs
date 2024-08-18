using Common.Base.Interfaces;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;

namespace TracksLyrics.Domain.Interfaces.Repositories.Mongo;

public interface ITrackRepository : IBaseRepository<TrackModel, Guid>
{
    Task<TrackModel?> GetByInfoOrDefaultAsync(TrackInfoModel trackInfo);
    Task<TrackModel> UpdateLyricsAsync(TrackModel trackLyric);
}