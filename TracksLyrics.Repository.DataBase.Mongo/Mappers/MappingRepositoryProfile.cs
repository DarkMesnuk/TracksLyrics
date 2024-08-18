using AutoMapper;
using Newtonsoft.Json;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Repository.DataBase.Entities.Mongo;

namespace TracksLyrics.Repository.DataBase.Mongo.Mappers;

public class MappingRepositoryProfile : Profile
{
    public MappingRepositoryProfile()
    {
        CreateMap<TrackEntity, TrackModel>()
            .ForMember(x => x.Lyrics, expression => expression.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Lyric.LyricsJson ?? string.Empty)))
            .ForMember(x => x.TranslatedLyrics, expression => expression.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.TranslatedLyric.TranslatedLyricsJson ?? string.Empty)));

        CreateMap<LyricEntity, LyricModel>()
            .ForMember(x => x.LyricsJson, expression => expression.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.LyricsJson ?? string.Empty)));
        
        CreateMap<TranslatedLyricEntity, TranslatedLyricModel>()
            .ForMember(x => x.TranslatedLyricsJson, expression => expression.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.TranslatedLyricsJson ?? string.Empty)));
        
        }
}