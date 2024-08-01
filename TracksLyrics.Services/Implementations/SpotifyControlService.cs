using Microsoft.Extensions.Options;
using Requestor;
using Requestor.Domain;
using TracksLyrics.Domain.Configurations;
using TracksLyrics.Domain.Interfaces.Services;

namespace TracksLyrics.Services.Implementations;

public class SpotifyControlService(
    ISender sender,
    IOptions<SpotifyConfigurations> spotifyConfigurationsOptions
    ) : ISpotifyControlService
{
    private readonly SpotifyConfigurations _spotifyConfigurations = spotifyConfigurationsOptions.Value;
    
    public async Task<bool> NextTrackAsync()
    {
        var request = new ApiRequest
        {
            Type = HttpMethod.Post,
            Url = _spotifyConfigurations.ApiUrl + "/api/spotify/nextTrack",
        };

        await sender.SendAsync(request);

        return true;
    }
}