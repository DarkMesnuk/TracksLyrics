using AutoMapper;
using Newtonsoft.Json;
using TracksLyrics.Domain.Models;
using TracksLyrics.Repository.DataBase.Entities;

namespace TracksLyrics.Repository.DataBase.Mappers;

public class MappingRepositoryProfile : Profile
{
    public MappingRepositoryProfile()
    {
        CreateMap<TrackLyricEntity, TrackLyricModel>()
            .ForMember(x => x.Lyrics, expression => expression.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.LyricsJson ?? string.Empty)))
            .ForMember(x => x.TranslatedLyrics, expression => expression.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.TranslatedLyricsJson ?? string.Empty)));
        
        CreateMap<TrackLyricModel, TrackLyricEntity>()
            .ForMember(x => x.LyricsJson, expression => expression.MapFrom(x => JsonConvert.SerializeObject(x.Lyrics)))
            .ForMember(x => x.TranslatedLyricsJson, expression => expression.MapFrom(x => JsonConvert.SerializeObject(x.TranslatedLyrics)));
    }
}