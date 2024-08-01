using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TracksLyrics.Domain.Consts;
using TracksLyrics.Domain.Interfaces.Services;
using TracksLyrics.Domain.Models;

namespace TracksLyrics.Application.BackGround;

public class TransmitterWorker(
    IServiceProvider serviceProvider
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = new Timer(async _ => await DoWorkAsync(), null, TimeSpan.Zero, TimeSpan.Zero);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Task DoWorkAsync()
    {
        using var scope = serviceProvider.CreateScope();   
        
        var transmitterMusicService = scope.ServiceProvider.GetRequiredService<ITransmitterMusicService>();
        var memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
        
        transmitterMusicService.GetCurrentPlayingTrack(SetInMemoryCache(memoryCache));
        
        return Task.CompletedTask;
    }

    private Action<TrackInfoModel?> SetInMemoryCache(IMemoryCache memoryCache)
    {
        return trackInfoModel =>
        {
            memoryCache.Set(TrackInfoConsts.CacheName, trackInfoModel, new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(3)));
        };
    }
    
    public override Task StopAsync(CancellationToken stoppingToken)
    {
        return base.StopAsync(stoppingToken);
    }
}