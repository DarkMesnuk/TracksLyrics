using System.Windows.Controls;
using TracksLyrics.Application.Dtos;

namespace TracksLyrics.WPF.Helpers;

public static class ItemCollectionsExtensions
{
    public static void AddLyrics(this ItemCollection items, TrackLyricDto trackLyric)
    {
        if (trackLyric.IsTranslated)
        {
            for (var i = 0; i < trackLyric.Lyrics?.Count; i++)
                items.AddTranslatedLyric(trackLyric, i);
        }
        else
        {
            foreach (var lyric in trackLyric.Lyrics!)
                items.AddLyric(lyric);
        }
    }
    
    public static void AddLyric(this ItemCollection items, string lyric = "")
    {
        items.Add(" " + lyric);
    }

    private static void AddTranslatedLyric(this ItemCollection items, TrackLyricDto trackLyric, int index)
    {
        items.AddLyric(trackLyric.Lyrics![index]);
        items.AddLyric(trackLyric.TranslatedLyrics![index]);
        items.AddLyric();
    }
}