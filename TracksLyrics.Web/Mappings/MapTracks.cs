using TracksLyrics.Application.Handlers;
using TracksLyrics.Application.Handlers.Parses;
using TracksLyrics.Domain.Models;
using TracksLyrics.Web.Requests;

namespace TracksLyrics.Web.Mappings;

public partial class RequestsMappings
{
    private void CreateMapTracks()
    {
        CreateMap<GetCurrentTrackLyricRequest, GetCurrentTrackLyricQueryRequest>();
        CreateMap<GetTrackRequest, GetLyricsCommandRequest>()
            .ForMember(x => x.TrackInfo, y => y.MapFrom(z => new TrackInfoModel { Name = z.Title, Artist = z.Artist }));
        CreateMap<SetLyricToCurrentTrackRequest, SetLyricToCurrentTrackCommandRequest>();
        CreateMap<ScanAndParseCurrentPlaylistRequest, ScanAndParseCurrentPlaylistCommandRequest>();
        
        CreateMap<ParseLyricForCurrentTrackRequest, ParseLyricForCurrentTrackByMuztextCommandRequest>();
        CreateMap<ParseLyricForCurrentTrackRequest, ParseLyricForCurrentTrackByGeniusCommandRequest>();
    }
}