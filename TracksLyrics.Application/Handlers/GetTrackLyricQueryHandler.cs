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

public class GetTrackLyricQueryHandler(
    ILogger<GetTrackLyricQueryHandler> logger, 
    IMapper mapper, 
    IMemoryCache memoryCache,
    ITrackLyricsService trackLyricsService,
    IParsersService parsersService
    ) : BaseHandler<GetTrackLyricQueryHandler, GetTrackLyricQueryRequest, GetCurrentTrackLyricQueryResponse>(logger)
{
    public override async Task<GetCurrentTrackLyricQueryResponse> Handle(GetTrackLyricQueryRequest request, CancellationToken cancellationToken)
    {
        var result = new GetCurrentTrackLyricQueryResponse(mapper);
        TrackInfoModel trackInfo;

        if (request.Artist != null && request.SongName != null)
            trackInfo = new TrackInfoModel { Artist = request.Artist, Name = request.SongName };
        else if (!memoryCache.TryGetValue(TrackInfoConsts.CacheName, out trackInfo) || trackInfo.IsNullOrDefault())
            throw new NotFoundException(nameof(TrackInfoModel));

        var trackLyric = await trackLyricsService.ParseAndSaveTrackLyricAsync(trackInfo, parsersService);
    
        return result.SetData(trackLyric);
    }
}

public class GetTrackLyricQueryRequest : BaseHandlerRequest<GetCurrentTrackLyricQueryResponse>
{
    public string? Artist { get; set; }
    public string? SongName { get; set; }
};

public class GetCurrentTrackLyricQueryResponse(
    IMapper mapper
) : BaseQueryResponse<GetCurrentTrackLyricQueryResponse, TrackLyricDto, TrackLyricModel>(mapper);

internal class GetTrackLyricRequestValidator : AbstractValidator<GetTrackLyricQueryRequest>;