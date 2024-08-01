using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Domain.Models;
using TracksLyrics.Search.Parsers;

namespace TracksLyrics.Search.Tests;

public class MusixmatchParserTests
{
    private readonly IMusixmatchParserService _musixmatchParserService = new MusixmatchParserService();

    [Fact]
    public async Task ParseOriginalLyricsAsync()
    {
        var track = new TrackInfoModel{ Name = "Rise", Artist = "the phantoms"};
        var trackLyricModel = await _musixmatchParserService.ParsAsync(track);

        Assert.NotEmpty(trackLyricModel.Lyrics);
        Assert.Equal("the phantoms", trackLyricModel.Artist);
        Assert.Equal("Rise", trackLyricModel.Name);
    }

    [Fact]
    public async Task ParseTranslationLyricsAsync()
    {
        var track = new TrackInfoModel{ Name = "Monster", Artist = "Skillet"};
        var trackLyricModel = await _musixmatchParserService.ParsAsync(track);

        Assert.NotEmpty(trackLyricModel.Lyrics);
        Assert.Equal("Skillet", trackLyricModel.Artist);
        Assert.Equal("Monster", trackLyricModel.Name);
    }
    
    //the-phantoms/rise
    
    //Skillet/Monster
}