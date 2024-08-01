namespace SpotifyApi.Entities;

public class TrackInfoEntity(string name, string artist, TimeSpan progressTime, TimeSpan durationTime)
{
    public string Name { get; set; } = name;

    public string Artist { get; set; } = artist;

    public TimeSpan ProgressTime { get; set; } = progressTime;

    public TimeSpan DurationTime { get; set; } = durationTime;

    public override string ToString()
    {
        return Name + " - " + Artist + "\t" + $"{ProgressTime.Minutes:D2}:{ProgressTime.Seconds:D2} - {DurationTime.Minutes:D2}:{DurationTime.Seconds:D2}";
    }
}