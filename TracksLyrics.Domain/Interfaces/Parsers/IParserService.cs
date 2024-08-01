using TracksLyrics.Domain.Models;

namespace TracksLyrics.Domain.Interfaces.Parsers;

public interface IParserService
{
    Task<TrackLyricModel> ParsAsync(string url, TrackInfoModel track);
    Task<TrackLyricModel> ParsAsync(TrackInfoModel track);
}