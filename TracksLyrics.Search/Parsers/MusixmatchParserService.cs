using HtmlAgilityPack;
using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Domain.Models;
using TracksLyrics.Search.Extensions;

namespace TracksLyrics.Search.Parsers;

public class MusixmatchParserService : IMusixmatchParserService
{
    public Task<TrackLyricModel> ParsAsync(string url, TrackInfoModel track)
    {
        // if (url.Contains("/translation/ukrainian"))
        //     return ParsTranslationAsync(url, track);
        // else
            return ParsOriginalAsync(url, track);
    }

    private async Task<TrackLyricModel> ParsOriginalAsync(string url, TrackInfoModel track)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(url);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(response);

        var lyricNodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='lyrics__content__ok']");
        var translationNodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='lyrics__content__ok translation']");

        if (lyricNodes != null)
        {
            foreach (var node in lyricNodes)
            {
                Console.WriteLine(node.InnerText.Trim());
            }
        }
        else
        {
            Console.WriteLine("No lyrics found.");
        }

        if (translationNodes != null)
        {
            Console.WriteLine("\nTranslation:");
            foreach (var node in translationNodes)
            {
                Console.WriteLine(node.InnerText.Trim());
            }
        }
        else
        {
            Console.WriteLine("No translation found.");
        }
        
        return new TrackLyricModel(track.Name, track.Artist, new List<string>());
    }

    private async Task<TrackLyricModel> ParsTranslationAsync(string url, TrackInfoModel track)
    {
       
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(url);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(response);

        var lyricNodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='lyrics__content__ok']");
        var translationNodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='lyrics__content__ok translation']");

        if (lyricNodes != null)
        {
            foreach (var node in lyricNodes)
            {
                Console.WriteLine(node.InnerText.Trim());
            }
        }
        else
        {
            Console.WriteLine("No lyrics found.");
        }

        if (translationNodes != null)
        {
            Console.WriteLine("\nTranslation:");
            foreach (var node in translationNodes)
            {
                Console.WriteLine(node.InnerText.Trim());
            }
        }
        else
        {
            Console.WriteLine("No translation found.");
        }
        
        return new TrackLyricModel(track.Name, track.Artist, new List<string>());
    }


    private static string ExtractText(string html)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        return htmlDocument.DocumentNode.InnerText;
    }
    
    public async Task<TrackLyricModel> ParsAsync(TrackInfoModel track)
    {
        var trackLyrics = await ParsTranslationAsync(CreateUrlWithTranslation(track), track);
        
        // if(!trackLyrics.IsFinded)
        //     trackLyrics = await ParsOriginalAsync(CreateUrl(track), track);
        //
        return trackLyrics;
    }
    
    // public Task<TrackLyricModel> ParsAsync(TrackInfoModel track)
    // {
    //     return ParsOriginalAsync(CreateUrl(track), track);
    // }
    
    private string CreateUrl(TrackInfoModel track)
        => $"https://www.musixmatch.com/lyrics/{track.Artist.ClearData()}/{track.Name.ClearData()}";
    
    private string CreateUrlWithTranslation(TrackInfoModel track)
        => CreateUrl(track) + " /translation/ukrainian";

    private List<string> ParseLyrics(string html)
    {
        html = html.Replace("&#x27;", "`");
        return html.Split("<br>").ToList();
    }
}