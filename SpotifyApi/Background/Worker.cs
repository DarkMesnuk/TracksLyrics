using SpotifyApi.Entities;
using SpotifyApi.Service;
using SpotifyAPI.Web;

namespace SpotifyApi.Background;

public class Worker : IWorker
{
    private readonly SendService _sendService = new();

    public async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var config = SpotifyClientConfig.CreateDefault().WithAuthenticator(new TokenAuthenticator(Config.AccessToken!, "Bearer"));
            var spotify = new SpotifyClient(config);

            await MonitorPlayingTrackAsync(spotify);
        }
    }

    private async Task MonitorPlayingTrackAsync(SpotifyClient spotify)
    {
        await SendCurrentPlayingTrackAsync(spotify);
        await Task.Delay(500);
    }

    private async Task SendCurrentPlayingTrackAsync(SpotifyClient spotify)
    {
        Console.Clear();
            
        try
        {
            var playback = await spotify.Player.GetCurrentPlayback();
            
            if (playback.Item is FullTrack track)
            {
                var trackInfo = new TrackInfoEntity(
                    track.Name,
                    UnionArtists(track.Artists.Select(x => x.Name).ToList()),
                    TimeSpan.FromMilliseconds(playback.ProgressMs),
                    TimeSpan.FromMilliseconds(track.DurationMs)
                );

                _sendService.Main(trackInfo);
            }
            else
                Console.WriteLine("No track is currently playing.");
        }
        catch (Exception ex)
        {
            if (ex.Message == "The access token expired")
                await SpotifyAuth.AuthorizeAsync();
                
            Console.WriteLine($"Error getting current playing track: {ex.Message}");
        }
    }

    private string UnionArtists(List<string> artists)
    {
        var result = artists[0];
        artists.RemoveAt(0);

        foreach (var artist in artists)
            result += ' ' + artist;

        return result;
    }
}