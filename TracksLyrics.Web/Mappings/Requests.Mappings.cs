using AutoMapper;

namespace TracksLyrics.Web.Mappings;

public partial class RequestsMappings : Profile
{
    public RequestsMappings()
    {
        CreateMapTracks();
    }
}