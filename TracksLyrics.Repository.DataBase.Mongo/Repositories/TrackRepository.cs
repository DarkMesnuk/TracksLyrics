using AutoMapper;
using Common.Base;
using Common.Exceptions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using TracksLyrics.Domain.Interfaces.Repositories.Mongo;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Repository.DataBase.Entities.Mongo;

namespace TracksLyrics.Repository.DataBase.Mongo.Repositories;

public class TrackRepository(
    IMongoCollection<TrackEntity> collection, 
    ILogger<TrackRepository> logger,
    IMapper mapper
    ) : BaseMongoRepository<TrackEntity, TrackModel, Guid>(logger, collection, mapper), ITrackRepository
{
    public async Task<TrackModel?> GetByInfoOrDefaultAsync(TrackInfoModel trackInfo)
    {
        var filter = Builders<TrackEntity>.Filter.Eq(x => x.Name, trackInfo.Name) &
                     Builders<TrackEntity>.Filter.Eq(x => x.Artist, trackInfo.Artist);

        var entity = await _collection.Find(filter).FirstOrDefaultAsync();

        return entity != null ? MapToModel(entity) : null;
    }

    public async Task<TrackModel> UpdateLyricsAsync(TrackModel trackLyric)
    {
        try
        {
            var filter = Builders<TrackEntity>.Filter.And(
                Builders<TrackEntity>.Filter.Eq(x => x.Name, trackLyric.Name),
                Builders<TrackEntity>.Filter.Eq(x => x.Artist, trackLyric.Artist)
            );
        
            // Mapping lyrics to JSON format
            var lyricEntity = new LyricEntity
            {
                LyricsJson = JsonConvert.SerializeObject(trackLyric.Lyrics)
            };

            var translatedLyricEntity = new TranslatedLyricEntity
            {
                TranslatedLyricsJson = JsonConvert.SerializeObject(trackLyric.TranslatedLyrics)
            };

            var update = Builders<TrackEntity>.Update
                .Set(x => x.Lyric, lyricEntity)
                .Set(x => x.TranslatedLyric, translatedLyricEntity);

            var result = await _collection.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                throw new UpdateFailedException();
            }

            var updatedEntity = await _collection.Find(filter).FirstOrDefaultAsync();

            trackLyric = MapToModel(updatedEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"{typeof(TrackModel).Name} update failed");
            throw new UpdateFailedException();
        }
        return trackLyric;
    }

}