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

public class ParseLyricForCurrentTrackByGeniusCommandHandler(
    ILogger<ParseLyricForCurrentTrackByGeniusCommandHandler> logger,
    IMemoryCache memoryCache,
    ITrackLyricService trackLyricService,
    IGeniusParserService geniusParserService
) : BaseHandler<ParseLyricForCurrentTrackByGeniusCommandHandler, ParseLyricForCurrentTrackByGeniusCommandRequest, ParseLyricForCurrentTrackByGeniusCommandResponse>(logger)
{
    public override async Task<ParseLyricForCurrentTrackByGeniusCommandResponse> Handle(ParseLyricForCurrentTrackByGeniusCommandRequest request, CancellationToken cancellationToken)
    {
        var result = new ParseLyricForCurrentTrackByGeniusCommandResponse();

        if (!memoryCache.TryGetValue(TrackInfoConsts.CacheName, out TrackInfoModel? trackInfo) && trackInfo.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackInfoModel));

        var trackLyric = await geniusParserService.ParsAsync(request.Url, trackInfo!);
        
        if (!trackLyric.IsFinded)
            throw new NotFoundException(nameof(TrackLyricModel));

        var isSuccess = await trackLyricService.SetLyricToTrackAsync(trackInfo!, new SetLyricToTrackSchema
        {
            IsTranslate = false,
            Lyrics = trackLyric.Lyrics
        });
        
        return isSuccess ? result : result.SetData(StatusCodes.SomethingWentWrong);
    }
}

public class ParseLyricForCurrentTrackByGeniusCommandRequest : BaseHandlerRequest<ParseLyricForCurrentTrackByGeniusCommandResponse>
{
    public required string Url { get; init; }
}

public class ParseLyricForCurrentTrackByGeniusCommandResponse : ApplicationResponse<ParseLyricForCurrentTrackByGeniusCommandResponse>;