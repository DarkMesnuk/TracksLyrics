using TracksLyrics.Application.Dtos;
using TracksLyrics.Domain.Models;
using TracksLyrics.Services.Helpers;

namespace TracksLyrics.Application.Mappings;

public static class DtosExtensions
{
    public static TrackLyricDto ToDto(this TrackLyricModel model)
    {
        return new TrackLyricDto
        {
            Name = model.Name,
            Artist = model.Artist,
            Lyrics = model.Lyrics,
            TranslatedLyrics = model.TranslatedLyrics,
            IsTranslated = model.IsTranslated,
            CombinedLyrics = model.CombineLyrics() 
        };
    }
}