using AutoMapper;
using Common.Base;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TracksLyrics.Domain.Interfaces.Repositories.Mongo;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Repository.DataBase.Entities.Mongo;

namespace TracksLyrics.Repository.DataBase.Mongo.Repositories;

public class TranslatedLyricRepository(
    IMongoCollection<TranslatedLyricEntity> collection, 
    ILogger<TranslatedLyricRepository> logger,
    IMapper mapper
) : BaseMongoRepository<TranslatedLyricEntity, TranslatedLyricModel, Guid>(logger, collection, mapper), ITranslatedLyricRepository
{
    
}