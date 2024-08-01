using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;

namespace TracksLyrics.Services.Implementations;

public class TransmitterMusicService : ITransmitterMusicService
{
    private readonly UdpClient _udpSocket = new(5555);
    private IPEndPoint _remoteIp = new(IPAddress.Any, 0);

    public void GetCurrentPlayingTrack(Action<TrackInfoModel?> trackAction)
    {
        do
        {
            var track = JsonConvert.DeserializeObject<TrackInfoModel>(Encoding.UTF8.GetString(_udpSocket.Receive(ref _remoteIp)));

            trackAction(track);
        } while (true);
        // ReSharper disable once FunctionNeverReturns
    }

    public void Dispose() 
        => _udpSocket.Dispose();
}