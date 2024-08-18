using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;

namespace TracksLyrics.Domain.Interfaces.Parsers;

public interface IParserService
{
    Task<TrackModel> ParsAsync(string url, TrackInfoModel track);
    Task<TrackModel> ParsAsync(TrackInfoModel track);
}