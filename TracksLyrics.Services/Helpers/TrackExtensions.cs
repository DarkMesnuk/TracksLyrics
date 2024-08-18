using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Models.Mongo;

namespace TracksLyrics.Services.Helpers;

public static class TrackExtensions
{
    public static List<string>? CombineLyrics(this TrackModel model)
    {
        if (model.Lyrics.Count == 0 || model.Lyrics.Count != model.TranslatedLyrics.Count)
            return null;

        var combinedLyrics = new List<string>();
        
        for (var i = 0; i < model.Lyrics.Count; i++)
        {
            combinedLyrics.Add(model.Lyrics[i]);
            combinedLyrics.Add(model.TranslatedLyrics[i]);
            combinedLyrics.Add("");
        }

        return combinedLyrics;
    }
    
    public static bool Equals(this TrackInfoModel currentTrackInfo, TrackInfoModel otherTrackInfo)
    {
        return currentTrackInfo.Artist == otherTrackInfo.Artist && currentTrackInfo.Name == otherTrackInfo.Name;
    }
}