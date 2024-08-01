namespace SpotifyApi.Background;

public interface IWorker
{
    public Task DoWork(CancellationToken cancellationToken);
}