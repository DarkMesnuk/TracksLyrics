using AutoMapper;
using Common.Base;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TracksLyrics.Domain.Interfaces.Repositories.Mongo;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Repository.DataBase.Entities.Mongo;

namespace TracksLyrics.Repository.DataBase.Mongo.Repositories;

public class LyricRepository(
    IMongoCollection<LyricEntity> collection, 
    ILogger<LyricRepository> logger,
    IMapper mapper
) : BaseMongoRepository<LyricEntity, LyricModel, Guid>(logger, collection, mapper), ILyricRepository
{
}