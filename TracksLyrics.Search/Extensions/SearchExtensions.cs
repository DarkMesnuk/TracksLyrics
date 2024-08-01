using TracksLyrics.Domain.Models;

namespace TracksLyrics.Search.Extensions;

public static class SearchExtensions
{
    public static string ClearFeats(this TrackInfoModel track)
    {
        return $"{ClearData(track.Artist)}-{ClearData(track.Name)}";
    }

    public static string ClearData(this string title)
    {
        var result = string.Empty;
        bool isDef = false;
        
        foreach (var item in OtherClear(title).ToLower())
        {
            if (char.IsLetter(item) || char.IsNumber(item))
            {
                result += item;
                isDef = false;
            }
            else
            {
                if(item == '\'')
                    continue;
                
                if(!isDef)
                    result += '-';

                isDef = !isDef;
            }
        }

        result = LastDef(result);

        return result;
    }

    private static string OtherClear(this string title)
    {
        if (title == "Zayde Wølf")
            return "Zayde Wolf";
        
        if (title == "Axwell /\\ Ingrosso")
            return "Axwell Ingrosso";
        
        if (title == "VOILÀ")
            return "VOILA";
        
        if (title == "Måneskin")
            return "Maneskin";

        var split = title.Split("(feat");

        if (split.Length > 1)
            return split[0];


        return title;
    }

    private static string LastDef(this string title)
    {
        var last = title.Last();

        if(!char.IsLetter(last) && !char.IsNumber(last))
            title = title.Remove(title.Length - 1);

        return title;
    }
}