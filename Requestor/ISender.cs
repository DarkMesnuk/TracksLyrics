using Requestor.Domain;

namespace Requestor;

public interface ISender : IDisposable
{
    Task<string> SendAsync(ApiRequest apiRequest);
}