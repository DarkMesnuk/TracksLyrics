using Common.Exceptions.Configuration;
using Requestor;
using TracksLyrics.Application;
using TracksLyrics.Domain.Configurations;
using TracksLyrics.Repository.DataBase;
using TracksLyrics.Repository.DataBase.Mongo;
using TracksLyrics.Search;
using TracksLyrics.Services;
using TracksLyrics.Web.Configuration;
using TracksLyrics.Web.Mappings;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false);

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    config.AddJsonFile("appsettings.Development.json", optional: false);

var configuration = config.Build();

// builder.Services.AddJsonOptions(opt =>
// {
//     opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//     opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//     opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
// });

builder.Services
    .Configure<SpotifyConfigurations>(configuration.GetSection(SpotifyConfigurations.ConfigSectionName));

builder.Services.AddControllers();
builder.Services.AddOptions();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddAutoMapper(typeof(RequestsMappings).Assembly);
        
builder.Services.AddApplication();
builder.Services.AddServices();
//builder.Services.AddPostgreDatabase(configuration.GetPostgreSqlConnectionString(), configuration.GetDbContextPoolSize());
builder.Services.AddMongoDatabase(configuration.GetMongoConnectionConfiguration());
//builder.Services.AddFileRepository();
builder.Services.AddParsers();
builder.Services.AddRequestor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();

app.UseRouting();
        
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseSwagger()
    .UseSwaggerUI();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});
#pragma warning restore ASP0014

//app.MigrateDbContext<TracksLyricsContext>()
    //.Run();