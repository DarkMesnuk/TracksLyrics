using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using SpotifyApi.Entities;

namespace SpotifyApi.Service;

public class SendService : IDisposable
{
    private readonly UdpClient _udpSocket = new();
    private readonly IPEndPoint _remotePoint = new(IPAddress.Parse("127.0.0.1"), 5555);

    public async void Main(TrackInfoEntity track)
    {
        var text = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(track));
        
        Console.WriteLine(track.ToString());
        
        await _udpSocket.SendAsync(text, _remotePoint);
    }

    public void Dispose()
    {
        _udpSocket.Dispose();
    }
}