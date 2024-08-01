using Common.Base;
using Common.Exceptions;
using Common.Extensions;
using TracksLyrics.Domain.Interfaces.Repositories;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Schemas;

namespace TracksLyrics.Services.Implementations;

public class TrackLyricsService(
    ITrackLyricsRepository trackLyricsRepository
) : BaseService<TrackLyricModel, int>(trackLyricsRepository), ITrackLyricsService
{
    public async Task<bool> SetLyricToTrackAsync(TrackInfoModel trackInfo, ISetLyricToTrackSchema schema)
    {
        var trackLyric = await trackLyricsRepository.GetByInfoOrDefaultAsync(trackInfo);
        
        if (trackLyric.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackLyricModel));

        var lyrics = schema.IsTranslate ? trackLyric!.TranslatedLyrics : trackLyric!.Lyrics;

        if (lyrics.Count != 0)
            throw new SomethingWentWrongException("Lyrics already exists");

        lyrics.Clear();
        lyrics.AddRange(schema.Lyrics);

        await trackLyricsRepository.UpdateAsync(trackLyric);
        
        return true;
    }
    
    public async Task<TrackLyricModel> ParseAndSaveTrackLyricAsync(TrackInfoModel trackInfo, IParsersService parsersService)
    {
        var trackLyricId = 0;
        var trackLyric = await trackLyricsRepository.GetByInfoOrDefaultAsync(trackInfo);
        
        if(trackLyric != null)
            trackLyricId = trackLyric.Id;
        
        if (trackLyricId != 0 && trackLyric?.Lyrics.Count != 0) 
            return trackLyric!;

        trackLyric = await parsersService.ParsOrDefaultAsync(trackInfo);

        if (trackLyricId != 0)
        {
            trackLyric.Id = trackLyricId;
            trackLyric = await trackLyricsRepository.UpdateAsync(trackLyric);   
        }
        else
            trackLyric = await trackLyricsRepository.CreateAsync(trackLyric);
        
        return trackLyric;
    }

    public Task<TrackLyricModel> UpdateLyricsAsync(TrackLyricModel trackLyric)
    {
        return trackLyricsRepository.UpdateLyricsAsync(trackLyric);
    }
}