using Common.Base;
using Common.Base.Requests.Handlers;
using Common.Exceptions;
using Common.Extensions;
using Common.Helpers;
using Common.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TracksLyrics.Domain.Consts;
using TracksLyrics.Domain.Interfaces.Parsers;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;
using TracksLyrics.Domain.Schemas;

namespace TracksLyrics.Application.Handlers.Parses;

public class ParseLyricForCurrentTrackByMuztextCommandHandler(
    ILogger<ParseLyricForCurrentTrackByMuztextCommandHandler> logger,
    IMemoryCache memoryCache,
    ITrackLyricService trackLyricService,
    IMuztextParserService muztextParserService
) : BaseHandler<ParseLyricForCurrentTrackByMuztextCommandHandler, ParseLyricForCurrentTrackByMuztextCommandRequest, ParseLyricForCurrentTrackByMuztextCommandResponse>(logger)
{
    public override async Task<ParseLyricForCurrentTrackByMuztextCommandResponse> Handle(ParseLyricForCurrentTrackByMuztextCommandRequest request, CancellationToken cancellationToken)
    {
        var result = new ParseLyricForCurrentTrackByMuztextCommandResponse();

        if (!memoryCache.TryGetValue(TrackInfoConsts.CacheName, out TrackInfoModel? trackInfo) && trackInfo.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackInfoModel));

        var trackLyric = await muztextParserService.ParsAsync(request.Url, trackInfo!);
        
        if (!trackLyric.IsFinded)
            throw new NotFoundException(nameof(TrackLyricModel));

        await trackLyricService.UpdateLyricsAsync(trackLyric);

        return result;
    }
}

public class ParseLyricForCurrentTrackByMuztextCommandRequest : BaseHandlerRequest<ParseLyricForCurrentTrackByMuztextCommandResponse>
{
    public required string Url { get; init; }
}

public class ParseLyricForCurrentTrackByMuztextCommandResponse : ApplicationResponse<ParseLyricForCurrentTrackByMuztextCommandResponse>;