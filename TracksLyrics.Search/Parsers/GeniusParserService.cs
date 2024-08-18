using HtmlAgilityPack;
using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;
using TracksLyrics.Search.Extensions;

namespace TracksLyrics.Search.Parsers;

public class GeniusParserService : IGeniusParserService
{
    public async Task<TrackModel> ParsAsync(string url, TrackInfoModel track)
    {
        var document = await new HtmlWeb().LoadFromWebAsync(url);
        HtmlNodeCollection? mainNodes;
            
        try
        {
            //mainNodes = document.DocumentNode.SelectNodes("//div[@class='Lyrics__Container-sc-1ynbvzw-5 Dzxov']");
            mainNodes = document.DocumentNode.SelectNodes("//div[@class='Lyrics__Container-sc-1ynbvzw-1 kUgSbL']");
        }
        catch (Exception)
        {
            mainNodes = null;
        }

        if (mainNodes == null)
        {
            return new TrackModel {
                Name = track.Name,
                Artist = track.Artist
            };
        }

        var lyrics = mainNodes.SelectMany(x => ParseLyrics(x.InnerHtml)).Select(ExtractText).ToList();

        return new TrackModel(track.Name, track.Artist, lyrics);
    }
    
    private static string ExtractText(string html)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        return htmlDocument.DocumentNode.InnerText;
    }
    
    public Task<TrackModel> ParsAsync(TrackInfoModel track)
    {
        return ParsAsync(CreateUrl(track), track);
    }
    
    private string CreateUrl(TrackInfoModel track)
        => $"https://genius.com/{track.ClearFeats()}-lyrics";

    private List<string> ParseLyrics(string html)
    {
        html = html.Replace("&#x27;", "`");
        return html.Split("<br>").ToList();
    }
}