using AutoMapper;
using Common.Base;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TracksLyrics.Domain.Interfaces.Repositories;
using TracksLyrics.Domain.Models;
using TracksLyrics.Repository.DataBase.Entities;

namespace TracksLyrics.Repository.DataBase.Repositories;

public class TrackLyricsRepository(
    TracksLyricsContext context, 
    ILogger<TrackLyricsRepository> logger, 
    IMapper mapper
    ) : BaseRepository<TrackLyricEntity, TrackLyricModel, int>(context, logger, mapper), ITrackLyricsRepository
{
    public async Task<TrackLyricModel?> GetByInfoOrDefaultAsync(TrackInfoModel trackInfo)
    {
        var entity = await GetBase.FirstOrDefaultAsync(x => x.Name == trackInfo.Name && x.Artist == trackInfo.Artist);

        return MapToModel(entity!);
    }

    public async Task<TrackLyricModel> UpdateLyricsAsync(TrackLyricModel trackLyric)
    {
        try
        {
            var entity = MapToEntity(trackLyric);

            await GetBase
                .Where(x => x.Name == entity.Name && x.Artist == entity.Artist)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.LyricsJson, entity.LyricsJson)
                    .SetProperty(x => x.TranslatedLyricsJson, entity.TranslatedLyricsJson)
                );

            trackLyric = MapToModel(entity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"{typeof(TrackLyricModel).Name} update failed");
            throw new UpdateFailedException();
        }

        return trackLyric;
    }
}