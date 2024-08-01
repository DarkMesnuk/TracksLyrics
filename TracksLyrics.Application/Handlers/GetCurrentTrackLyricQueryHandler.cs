using AutoMapper;
using Common.Base;
using Common.Base.Requests.Handlers;
using Common.Base.Responses;
using Common.Exceptions;
using Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TracksLyrics.Application.Dtos;
using TracksLyrics.Domain.Consts;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;

namespace TracksLyrics.Application.Handlers;

public class GetCurrentTrackLyricQueryHandler(
    ILogger<GetCurrentTrackLyricQueryHandler> logger, 
    IMapper mapper, 
    IMemoryCache memoryCache,
    ITrackLyricsService trackLyricsService,
    IParsersService parsersService
    ) : BaseHandler<GetCurrentTrackLyricQueryHandler, GetCurrentTrackLyricQueryRequest, GetCurrentTrackLyricQueryResponse>(logger)
{
    public override async Task<GetCurrentTrackLyricQueryResponse> Handle(GetCurrentTrackLyricQueryRequest request, CancellationToken cancellationToken)
    {
        var result = new GetCurrentTrackLyricQueryResponse(mapper);

        if (!memoryCache.TryGetValue(TrackInfoConsts.CacheName, out TrackInfoModel? trackInfo) && trackInfo.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackInfoModel));

        var trackLyric = await trackLyricsService.ParseAndSaveTrackLyricAsync(trackInfo!, parsersService);
        
        return result.SetData(trackLyric);
    }
}

public class GetCurrentTrackLyricQueryRequest : BaseHandlerRequest<GetCurrentTrackLyricQueryResponse>;

public class GetCurrentTrackLyricQueryResponse(
    IMapper mapper
) : BaseQueryResponse<GetCurrentTrackLyricQueryResponse, TrackLyricDto, TrackLyricModel>(mapper);

internal class GetCurrentTrackLyricRequestValidator : AbstractValidator<GetCurrentTrackLyricQueryRequest>;