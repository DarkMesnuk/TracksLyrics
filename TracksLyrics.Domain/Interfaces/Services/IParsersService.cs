using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;

namespace TracksLyrics.Domain.Interfaces.Services;

public interface IParsersService
{
    Task<TrackModel> ParsOrDefaultAsync(TrackInfoModel trackInfo);
}