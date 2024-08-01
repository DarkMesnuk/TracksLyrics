namespace SpotifyApi.Background;

public class WorkerService(
    IWorker worker
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await worker.DoWork(stoppingToken);
}