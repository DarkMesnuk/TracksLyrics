using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Domain.Interfaces.Repositories;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Repository.DataBase.Mongo.Repositories;
using TracksLyrics.Repository.File.Repositories;
using TracksLyrics.Search.Parsers;
using TracksLyrics.Services.Implementations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace TracksLyrics.WPF.Settings;

public class Di
{
    public Di()
    {
        MuztextParserService = new MuztextParserService();
        GeniusParserService = new GeniusParserService();
        MusixmatchParserService = new MusixmatchParserService();
        TrackLyricsRepository = new TrackLyricsFileRepository();
        TransmitterMusicService = new TransmitterMusicService();
        //TrackLyricService = new TrackLyricLyricsService(TrackRepository, LyricRepository, TranslatedLyricRepository);
        ParsersService = new ParsersService(MuztextParserService, GeniusParserService, MusixmatchParserService);
    }

    public static ITrackLyricsRepository TrackLyricsRepository { get; private set; }
    public static IMuztextParserService MuztextParserService { get; private set; }
    public static IGeniusParserService GeniusParserService { get; private set; }
    public static IMusixmatchParserService MusixmatchParserService  { get; private set; }
    public static ITransmitterMusicService TransmitterMusicService { get; private set; }
    public static ITrackLyricService TrackLyricService { get; private set; }
    public static IParsersService ParsersService { get; private set; }
}