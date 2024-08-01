using System.Diagnostics;
using System.Runtime.InteropServices;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace SpotifyApi.Service;

public static class SpotifyAuth
{
    private const string RedirectUri = @"https://localhost:5000/";
    private const string Scope = "playlist-modify-public playlist-read-private playlist-modify-private user-read-playback-state app-remote-control user-modify-playback-state";
    private const string AuthUrl = @"https://accounts.spotify.com/authorize";
    private const string TokenUrl = @"https://accounts.spotify.com/api/token";

    public static async Task AuthorizeAsync()
    {
        Console.WriteLine("Opening browser for authorization...");
        OpenBrowser(GetAuthorizationUrl());

        var accessToken = await WaitForAccessTokenAsync();
        Console.WriteLine($"Access Token: {accessToken}");
    }

    private static string GetAuthorizationUrl()
    {
        return AuthUrl.SetQueryParams(new
        {
            client_id = Config.ClientId,
            response_type = "code",
            redirect_uri = RedirectUri,
            scope = Scope
        });
    }

    private static async Task<string?> GetAccessTokenAsync(string code)
    {
        var response = await TokenUrl.PostUrlEncodedAsync(new
        {
            grant_type = "authorization_code",
            code,
            redirect_uri = RedirectUri,
            client_id = Config.ClientId,
            client_secret = Config.ClientSecret
        }).ReceiveJson<TokenResponse>();

        return response.AccessToken;
    }

    private class TokenResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
    }

    private static void OpenBrowser(string url)
    {
        try
        {
            Process.Start(url);
        }
        catch
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", url);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Process.Start("open", url);
        }
    }

    private static async Task<string> WaitForAccessTokenAsync()
    {
        var accessTokenTcs = new TaskCompletionSource<string>();
        using var host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.Configure(app => app.Run(async context =>
                {
                    var code = context.Request.Query["code"].ToString();
                    if (!string.IsNullOrEmpty(code))
                    {
                        var accessToken = await GetAccessTokenAsync(code);
                        Config.AccessToken = accessToken;

                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync("<html><body><script>setTimeout(function() { window.close(); }, 5000);</script></body></html>");
                    }
                    else
                    {
                        accessTokenTcs.SetResult(string.Empty);
                        await context.Response.WriteAsync("Authorization failed. Please try again.");
                    }
                }))
                .UseUrls(RedirectUri);
            })
            .Build();

        host.Start();

        return await accessTokenTcs.Task;
    }
}