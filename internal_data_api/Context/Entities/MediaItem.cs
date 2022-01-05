namespace internal_data_api.Context.Entites;

public class MediaItem
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? PlaylistId { get; set; }

    public string ItemGuid { get; set; }

    public string? YtPlaylistId { get; set; }

    public string? YtPlaylistItemId { get; set; }

    public string YtVideoId { get; set; }

    public string YtVideoTitle { get; set; }

    public int? Duration { get; set; }

    public string? Status { get; set; }

    public int? Progress { get; set; }

    public string? ThumbnailPath { get; set; }

    public string? Mp3Path { get; set; }

    public bool IsSyncedToDropbox { get; set; }

    public bool IsDeleted { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    
    public User User { get; set; }
}
