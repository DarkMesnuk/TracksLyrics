using Common.Base.Requests.Handlers;
using Common.Base.Responses;
using MediatR;
using TracksLyrics.Application.Dtos;
using TracksLyrics.Application.Mappings;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;

namespace TracksLyrics.Application.Handlers;

public class GetLyricsCommandHandler(
    ITrackLyricsService trackLyricsService,
    IParsersService parsersService
    ) : IRequestHandler<GetLyricsCommandRequest, GetLyricsCommandResponse>
{
    public async Task<GetLyricsCommandResponse> Handle(GetLyricsCommandRequest request, CancellationToken cancellationToken)
    {
        var result = new GetLyricsCommandResponse();
        
        var trackLyric = await trackLyricsService.ParseAndSaveTrackLyricAsync(request.TrackInfo, parsersService);
        
        return result.SetData(trackLyric.ToDto());
    }
}

public class GetLyricsCommandRequest : BaseHandlerRequest<GetLyricsCommandResponse>
{
    public required TrackInfoModel TrackInfo { get; init; }
}

public class GetLyricsCommandResponse : BaseQueryResponse<GetLyricsCommandResponse, TrackLyricDto>;