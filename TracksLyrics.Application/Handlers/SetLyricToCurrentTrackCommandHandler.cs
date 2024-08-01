using Common.Base;
using Common.Base.Requests.Handlers;
using Common.Exceptions;
using Common.Extensions;
using Common.Helpers;
using Common.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TracksLyrics.Domain.Consts;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Schemas;

namespace TracksLyrics.Application.Handlers;

public class SetLyricToCurrentTrackCommandHandler(
    ILogger<SetLyricToCurrentTrackCommandHandler> logger,
    IMemoryCache memoryCache,
    ITrackLyricsService trackLyricsService
) : BaseHandler<SetLyricToCurrentTrackCommandHandler, SetLyricToCurrentTrackCommandRequest, SetLyricToCurrentTrackCommandResponse>(logger)
{
    public override async Task<SetLyricToCurrentTrackCommandResponse> Handle(SetLyricToCurrentTrackCommandRequest request, CancellationToken cancellationToken)
    {
        var result = new SetLyricToCurrentTrackCommandResponse();

        if (!memoryCache.TryGetValue(TrackInfoConsts.CacheName, out TrackInfoModel? trackInfo) && trackInfo.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackInfoModel));

        await trackLyricsService.SetLyricToTrackAsync(trackInfo!, request);

        return result;
    }
}

public class SetLyricToCurrentTrackCommandRequest : BaseHandlerRequest<SetLyricToCurrentTrackCommandResponse>, ISetLyricToTrackSchema
{
    public required bool IsTranslate { get; init; }
    public required List<string> Lyrics { get; init; }
}

public class SetLyricToCurrentTrackCommandResponse : ApplicationResponse<SetLyricToCurrentTrackCommandResponse>;