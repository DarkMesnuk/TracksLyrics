using TracksLyrics.Application.Dtos;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Services.Helpers;

namespace TracksLyrics.Application.Mappings;

public static class DtosExtensions
{
    public static TrackDto ToDto(this TrackModel model)
    {
        return new TrackDto
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