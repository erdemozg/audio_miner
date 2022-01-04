namespace internal_data_api.Context.Entites;

public class Playlist
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string YtPlaylistId { get; set; }

    public string YtPlaylistName { get; set; }

    public string? DropboxToken { get; set; }

    public string? SyncError { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    
    public User User { get; set; }
}
