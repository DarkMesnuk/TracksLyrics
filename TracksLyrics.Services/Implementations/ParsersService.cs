using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;

namespace TracksLyrics.Services.Implementations;

public class ParsersService(
    IMuztextParserService muztextParserService,
    IGeniusParserService geniusParserService,
    IMusixmatchParserService musixmatchParserService
) : IParsersService
{
    public async Task<TrackModel> ParsOrDefaultAsync(TrackInfoModel trackInfo)
    {
        // var trackLyricMus = await musixmatchParserService.ParsAsync(trackInfo);
        //
        // if (trackLyricMus.IsTranslated)
        //     return trackLyricMus;
        
        var trackLyric = await muztextParserService.ParsAsync(trackInfo);
            
        if(trackLyric.IsFinded)
            return trackLyric;
        
        // if(trackLyricMus.IsFinded)
        //     return trackLyricMus;

        trackLyric = await geniusParserService.ParsAsync(trackInfo);

        return trackLyric;
    }
}