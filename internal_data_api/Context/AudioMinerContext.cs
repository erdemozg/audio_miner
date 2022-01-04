using internal_data_api.Context.Entites;
using Microsoft.EntityFrameworkCore;

namespace internal_data_api.Context;

public class AudioMinerContext : DbContext
{
    public string DbPath { get; }

    public DbSet<User> Users { get; set; }

    public DbSet<Playlist> Playlists { get; set; }
    
    public DbSet<MediaItem> MediaItems { get; set; }
    
    public DbSet<Log> Logs { get; set; }

    public AudioMinerContext()
    {
        DbPath = System.IO.Path.Join(System.Environment.GetEnvironmentVariable("DB_FOLDER"), "audiominer.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}
