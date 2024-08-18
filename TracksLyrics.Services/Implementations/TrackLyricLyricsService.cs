using Common.Base;
using Common.Exceptions;
using Common.Extensions;
using TracksLyrics.Domain.Interfaces.Repositories.Mongo;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Domain.Schemas;

namespace TracksLyrics.Services.Implementations;

public class TrackLyricLyricsService(
    ITrackRepository trackRepository,
    ILyricRepository lyricsRepository,
    ITranslatedLyricRepository translatedLyricRepository
) : BaseService<TrackModel, Guid>(trackRepository), ITrackLyricService
{
    public async Task<bool> SetLyricToTrackAsync(TrackInfoModel trackInfo, ISetLyricToTrackSchema schema)
    {
        var track = await trackRepository.GetByInfoOrDefaultAsync(trackInfo);
        
        track.Lyrics = (await lyricsRepository.GetByIdAsync(track.Id)).LyricsJson;
   
        track.TranslatedLyrics = (await translatedLyricRepository.GetByIdAsync(track.Id)).TranslatedLyricsJson;
        
        if (track.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackModel));

        var lyrics = schema.IsTranslate ? track!.TranslatedLyrics : track!.Lyrics;

        if (lyrics.Count != 0)
            throw new SomethingWentWrongException("Lyrics already exists");

        lyrics.Clear();
        lyrics.AddRange(schema.Lyrics);

        await trackRepository.UpdateAsync(track);
        
        return true;
    }

    public async Task<TrackModel> ParseAndSaveTrackLyricAsync(TrackInfoModel trackInfo, IParsersService parsersService)
    {
        Guid trackLyricId = Guid.Empty;
        var trackLyric = await trackRepository.GetByInfoOrDefaultAsync(trackInfo);
        
        if(trackLyric != null)
            trackLyricId = trackLyric.Id;
        
        if (trackLyric?.Lyrics.Count != 0) 
            return trackLyric!;
    
        trackLyric = await parsersService.ParsOrDefaultAsync(trackInfo);
    
        if (trackLyricId != Guid.Empty)
        {
            trackLyric.Id = trackLyricId;
            trackLyric = await trackRepository.UpdateAsync(trackLyric);   
        }
        else
            trackLyric = await trackRepository.CreateAsync(trackLyric);
        
        return trackLyric;
    }

    public Task<TrackModel> UpdateLyricsAsync(TrackModel trackLyric)
    {
        return trackRepository.UpdateLyricsAsync(trackLyric);
    }
}