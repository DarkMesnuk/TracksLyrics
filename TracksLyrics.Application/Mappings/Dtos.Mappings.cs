using AutoMapper;
using TracksLyrics.Application.Dtos;
using TracksLyrics.Domain.Models;
using TracksLyrics.Services.Helpers;

namespace TracksLyrics.Application.Mappings;

public class DtosMappings : Profile
{
    public DtosMappings()
    {
        CreateMap<TrackLyricModel, TrackLyricDto>()
            .ForMember(dest => dest.CombinedLyrics, opt => opt.MapFrom(src => src.CombineLyrics()));
    }
}