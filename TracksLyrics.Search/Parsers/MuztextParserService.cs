using HtmlAgilityPack;
using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Domain.Models;
using TracksLyrics.Search.Extensions;

namespace TracksLyrics.Search.Parsers;

public class MuztextParserService : IMuztextParserService
{
    public async Task<TrackLyricModel> ParsAsync(string url, TrackInfoModel track)
    {
        var document = await new HtmlWeb().LoadFromWebAsync(url);
        HtmlNodeCollection? mainNodes;
            
        try
        {
            mainNodes = document.DocumentNode.SelectSingleNode("//div[@id='content']").SelectNodes("//tbody");
        }
        catch (Exception)
        {
            mainNodes = null;
        }

        if (mainNodes == null)
        {
            return new TrackLyricModel {
                Name = track.Name,
                Artist = track.Artist,
            };
        }

        var trackLines = mainNodes[0].InnerText.Split('\n').ToList();
        var translatedTrackLines = mainNodes[1].InnerText.Split('\n').ToList();

        return new TrackLyricModel(track.Name, track.Artist,  trackLines, translatedTrackLines);
    }
    
    public Task<TrackLyricModel> ParsAsync(TrackInfoModel track)
    {
        return ParsAsync(CreateUrl(track), track);
    }

    private string CreateUrl(TrackInfoModel track)
        => $"https://uk.muztext.com/lyrics/{track.ClearFeats()}";
}