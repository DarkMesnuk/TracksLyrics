using TracksLyrics.Domain.Models;

namespace TracksLyrics.Domain.Interfaces.Services;

public interface IParsersService
{
    Task<TrackLyricModel> ParsOrDefaultAsync(TrackInfoModel trackInfo);
}