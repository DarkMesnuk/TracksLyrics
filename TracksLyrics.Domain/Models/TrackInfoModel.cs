using Common.Base.Interfaces.Models;

namespace TracksLyrics.Domain.Models;

public class TrackInfoModel : IEntityModel
{
    public string Name { get; set; }
    public string Artist { get; set; }
    public TimeSpan ProgressTime { get; set; }
    public TimeSpan DurationTime { get; set; }
    
    public override bool Equals(object? val)
    {
        return val is TrackInfoModel trackInfo && Name == trackInfo.Name && Artist == trackInfo.Artist;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }
}