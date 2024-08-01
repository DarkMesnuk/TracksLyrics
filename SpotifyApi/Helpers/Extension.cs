using SpotifyApi.Service;

namespace SpotifyApi.Helpers;

public static class Extension
{
    public static async Task GetAccessToken(this IServiceCollection service)
    {
        await SpotifyAuth.AuthorizeAsync();
    }
}