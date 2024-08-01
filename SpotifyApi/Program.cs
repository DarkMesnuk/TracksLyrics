using SpotifyApi;
using SpotifyApi.Background;
using SpotifyApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

do
{
    await builder.Services.GetAccessToken();
} while (Config.AccessToken == null);

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

builder.Services.Configure<Config>(configuration.GetSection("Client"));

builder.Services.AddSingleton<IWorker, Worker>();
builder.Services.AddHostedService<WorkerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();