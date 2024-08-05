using TracksLyrics.Application.Handlers;
using TracksLyrics.Application.Handlers.Parses;
using TracksLyrics.Web.Requests;

namespace TracksLyrics.Web.Mappings;

public partial class RequestsMappings
{
    private void CreateMapTracks()
    {
        CreateMap<GetTrackLyricRequest, GetTrackLyricQueryRequest>();
        CreateMap<SetLyricToCurrentTrackRequest, SetLyricToCurrentTrackCommandRequest>();
        CreateMap<ScanAndParseCurrentPlaylistRequest, ScanAndParseCurrentPlaylistCommandRequest>();
        
        CreateMap<ParseLyricForCurrentTrackRequest, ParseLyricForCurrentTrackByMuztextCommandRequest>();
        CreateMap<ParseLyricForCurrentTrackRequest, ParseLyricForCurrentTrackByGeniusCommandRequest>();
    }
}