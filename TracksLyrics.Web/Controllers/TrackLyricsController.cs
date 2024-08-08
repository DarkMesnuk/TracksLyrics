using AutoMapper;
using Common.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TracksLyrics.Application.Handlers;
using TracksLyrics.Application.Handlers.Parses;
using TracksLyrics.Web.Requests;
using TracksLyrics.Web.Responses;

namespace TracksLyrics.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackLyricsController(
    ILogger<TrackLyricsController> logger, 
    IMapper mapper, 
    IMediator mediator
    ) : BaseApiController(logger, mapper, mediator)
{
    [HttpGet]
    [Route("get-current")]
    [ProducesResponseType(typeof(GetTrackLyricResponse), 200)]
    public async Task<IActionResult> GetCurrentLyrics([FromQuery] GetCurrentTrackLyricRequest request)
    {
        var getRequest = Mapper.Map<GetCurrentTrackLyricQueryRequest>(request);

        var applicationResponse = await Mediator.Send(getRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new GetTrackLyricResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    [HttpGet]
    [Route("get-lyric-by-artists-and-title")]
    [ProducesResponseType(typeof(GetTrackLyricResponse), 200)]
    public async Task<IActionResult> GetLyricByArtistsAndTitle([FromQuery] GetTrackRequest request)
    {
        var getRequest = Mapper.Map<GetLyricsCommandRequest>(request);

        var applicationResponse = await Mediator.Send(getRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new GetTrackLyricResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    [HttpPost]
    [Route("set-lyric-to-current")]
    public async Task<IActionResult> SetLyricToCurrentTrack([FromBody] SetLyricToCurrentTrackRequest request)
    {
        var setRequest = Mapper.Map<SetLyricToCurrentTrackCommandRequest>(request);

        var applicationResponse = await Mediator.Send(setRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    [HttpPost]
    [Route("muztext/parse-lyric-for-current")]
    public async Task<IActionResult> ParseLyricForCurrentTrackByMuztext([FromBody] ParseLyricForCurrentTrackRequest request)
    {
        var setRequest = Mapper.Map<ParseLyricForCurrentTrackByMuztextCommandRequest>(request);

        var applicationResponse = await Mediator.Send(setRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    [HttpPost]
    [Route("genius/set-lyric-to-current")]
    public async Task<IActionResult> ParseLyricForCurrentTrackByGenius([FromBody] ParseLyricForCurrentTrackRequest request)
    {
        var setRequest = Mapper.Map<ParseLyricForCurrentTrackByGeniusCommandRequest>(request);

        var applicationResponse = await Mediator.Send(setRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }

    [HttpPost]
    [Route("scan-and-parse-current-playlist")]
    public async Task<IActionResult> ScanAndParseCurrentPlaylist([FromBody] ScanAndParseCurrentPlaylistRequest request)
    {
        var commandRequest = Mapper.Map<ScanAndParseCurrentPlaylistCommandRequest>(request);

        var applicationResponse = await Mediator.Send(commandRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
}