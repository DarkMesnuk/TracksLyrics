using Microsoft.EntityFrameworkCore;
using TracksLyrics.Repository.DataBase.Entities;

namespace TracksLyrics.Repository.DataBase;

public class TracksLyricsContext(
    DbContextOptions<TracksLyricsContext> options
    ) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    
    public DbSet<TrackLyricEntity> TrackLyrics { get; set; }
}