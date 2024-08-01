using Common.Base;
using Common.Base.Requests.Handlers;
using Common.Exceptions;
using Common.Extensions;
using Common.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TracksLyrics.Domain.Consts;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;

namespace TracksLyrics.Application.Handlers;

public class ScanAndParseCurrentPlaylistCommandHandler(
    ILogger<ScanAndParseCurrentPlaylistCommandHandler> logger,
    IMemoryCache memoryCache,
    ITrackLyricsService trackLyricsService,
    IParsersService parsersService,
    ISpotifyControlService spotifyControlService
) : BaseHandler<ScanAndParseCurrentPlaylistCommandHandler, ScanAndParseCurrentPlaylistCommandRequest, ScanAndParseCurrentPlaylistCommandResponse>(logger)
{
    private TrackInfoModel firstTrack;
    
    public override async Task<ScanAndParseCurrentPlaylistCommandResponse> Handle(ScanAndParseCurrentPlaylistCommandRequest request, CancellationToken cancellationToken)
    {
        var result = new ScanAndParseCurrentPlaylistCommandResponse();

        if (!memoryCache.TryGetValue(TrackInfoConsts.CacheName, out TrackInfoModel? trackInfo) && trackInfo.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackInfoModel));

        firstTrack = new TrackInfoModel { Name = trackInfo!.Name, Artist = trackInfo.Artist };

        var currentTrack = new TrackInfoModel { Name = trackInfo!.Name, Artist = trackInfo.Artist };
        
        do
        {
            await trackLyricsService.ParseAndSaveTrackLyricAsync(currentTrack!, parsersService);

            bool isSendNextCommand;
            
            do
            {
                isSendNextCommand = await spotifyControlService.NextTrackAsync();
            } while (!isSendNextCommand);
            
            do
            {
                if (!memoryCache.TryGetValue(TrackInfoConsts.CacheName, out trackInfo))
                    continue;
            } while (currentTrack.Equals(trackInfo));
            
            currentTrack = trackInfo;
            await Task.Delay(2000, cancellationToken);
        } while (!firstTrack.Equals(currentTrack));

        return result;
    }
}

public class ScanAndParseCurrentPlaylistCommandRequest : BaseHandlerRequest<ScanAndParseCurrentPlaylistCommandResponse>;

public class ScanAndParseCurrentPlaylistCommandResponse : ApplicationResponse<ScanAndParseCurrentPlaylistCommandResponse>;