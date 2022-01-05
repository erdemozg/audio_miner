using System.Text.Json.Serialization;
using internal_data_api.Context;
using Newtonsoft.Json;

namespace internal_data_api.Model;

public class MediaItemModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("playlist_id")]
    public int? PlaylistId { get; set; }

    [JsonPropertyName("item_guid")]
    public string ItemGuid { get; set; }

    [JsonPropertyName("yt_playlist_id")]
    public string? YtPlaylistId { get; set; }

    [JsonPropertyName("yt_playlist_item_id")]
    public string? YtPlaylistItemId { get; set; }

    [JsonPropertyName("yt_video_id")]
    public string YtVideoId { get; set; }

    [JsonPropertyName("yt_video_title")]
    public string YtVideoTitle { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("progress")]
    public int? Progress { get; set; }

    [JsonPropertyName("thumbnail_path")]
    public string? ThumbnailPath { get; set; }

    [JsonPropertyName("mp3_path")]
    public string? Mp3Path { get; set; }

    [JsonPropertyName("is_synced_to_dropbox")]
    public bool IsSyncedToDropbox { get; set; }

    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class MediaItemQuery
{
    public static List<MediaItemModel> GetMediaItems(MediaItemQueryFilters filters)
    {
        using (var db = new AudioMinerContext())
        {
            var query = db.MediaItems.Join(
                db.Users, 
                mediaItem => mediaItem.UserId, 
                user => user.Id,
                (mediaItem, user) => new {
                    Id = mediaItem.Id,
                    UserId = mediaItem.UserId,
                    Username = user != null ? user.Username : "",
                    PlaylistId = mediaItem.PlaylistId,
                    YtPlaylistId = mediaItem.YtPlaylistId,
                    YtPlaylistItemId = mediaItem.YtPlaylistItemId,
                    YtVideoId = mediaItem.YtVideoId,
                    YtVideoTitle = mediaItem.YtVideoTitle,
                    Duration = mediaItem.Duration,
                    Status = mediaItem.Status,
                    Progress = mediaItem.Progress,
                    ThumbnailPath = mediaItem.ThumbnailPath,
                    Mp3Path = mediaItem.Mp3Path,
                    ItemGuid = mediaItem.ItemGuid,
                    IsSyncedToDropbox = mediaItem.IsSyncedToDropbox,
                    IsDeleted = mediaItem.IsDeleted,
                    ErrorMessage = mediaItem.ErrorMessage,
                    CreatedAt = mediaItem.CreatedAt,
                    UpdatedAt = mediaItem.UpdatedAt
                }
            );

            if (filters.PlaylistId != null)
            {
                query = query.Where(p => p.PlaylistId == filters.PlaylistId);
            }

            if (!string.IsNullOrWhiteSpace(filters.YtTitle))
            {
                query = query.Where(p => p.YtVideoTitle.ToLower().Contains(filters.YtTitle.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filters.User))
            {
                query = query.Where(p => p.Username.Contains(filters.User));
            }

            if (filters.ExcludeDeletedItems)
            {
                query = query.Where(p => p.IsDeleted != true);
            }

            var ret = query
                .Select(p => new MediaItemModel {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.Username,
                    PlaylistId = p.PlaylistId,
                    YtPlaylistId = p.YtPlaylistId,
                    YtPlaylistItemId = p.YtPlaylistItemId,
                    YtVideoId = p.YtVideoId,
                    YtVideoTitle = p.YtVideoTitle,
                    Duration = p.Duration,
                    Status = p.Status,
                    Progress = p.Progress,
                    ThumbnailPath = p.ThumbnailPath,
                    Mp3Path = p.Mp3Path,
                    ItemGuid = p.ItemGuid,
                    IsSyncedToDropbox = p.IsSyncedToDropbox,
                    IsDeleted = p.IsDeleted,
                    ErrorMessage = p.ErrorMessage,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .OrderByDescending(p => p.CreatedAt)
                .Skip(filters.Skip ?? 0)
                .Take(filters.Take ?? Int32.MaxValue)
                .ToList();

            return ret;
        }
    }

    public static MediaItemModel GetMediaItemByItemGuid(string itemGuid)
    {
        using (var db = new AudioMinerContext())
        {
            var query = db.MediaItems.Join(
                db.Users, 
                mediaItem => mediaItem.UserId, 
                user => user.Id,
                (mediaItem, user) => new {
                    Id = mediaItem.Id,
                    UserId = mediaItem.UserId,
                    Username = mediaItem.User != null ? mediaItem.User.Username : "",
                    PlaylistId = mediaItem.PlaylistId,
                    YtPlaylistId = mediaItem.YtPlaylistId,
                    YtPlaylistItemId = mediaItem.YtPlaylistItemId,
                    YtVideoId = mediaItem.YtVideoId,
                    YtVideoTitle = mediaItem.YtVideoTitle,
                    Duration = mediaItem.Duration,
                    Status = mediaItem.Status,
                    Progress = mediaItem.Progress,
                    ThumbnailPath = mediaItem.ThumbnailPath,
                    Mp3Path = mediaItem.Mp3Path,
                    ItemGuid = mediaItem.ItemGuid,
                    IsSyncedToDropbox = mediaItem.IsSyncedToDropbox,
                    IsDeleted = mediaItem.IsDeleted,
                    ErrorMessage = mediaItem.ErrorMessage,
                    CreatedAt = mediaItem.CreatedAt,
                    UpdatedAt = mediaItem.UpdatedAt
                }
            );

            var ret = query
                .Where(p => p.ItemGuid == itemGuid)
                .Select(p => new MediaItemModel {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.Username,
                    PlaylistId = p.PlaylistId,
                    YtPlaylistId = p.YtPlaylistId,
                    YtPlaylistItemId = p.YtPlaylistItemId,
                    YtVideoId = p.YtVideoId,
                    YtVideoTitle = p.YtVideoTitle,
                    Duration = p.Duration,
                    Status = p.Status,
                    Progress = p.Progress,
                    ThumbnailPath = p.ThumbnailPath,
                    Mp3Path = p.Mp3Path,
                    ItemGuid = p.ItemGuid,
                    IsSyncedToDropbox = p.IsSyncedToDropbox,
                    IsDeleted = p.IsDeleted,
                    ErrorMessage = p.ErrorMessage,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).FirstOrDefault();

            return ret;
        }
    }

    public static void CreateMediaItem(MediaItemModel model)
    {
        using (var db = new AudioMinerContext())
        {
            db.MediaItems.Add(new Context.Entites.MediaItem {
                CreatedAt = DateTime.UtcNow,
                Duration = model.Duration,
                ErrorMessage = model.ErrorMessage,
                IsSyncedToDropbox = model.IsSyncedToDropbox,
                ItemGuid = model.ItemGuid,
                Mp3Path = model.Mp3Path,
                PlaylistId = model.PlaylistId,
                Progress = model.Progress,
                Status = model.Status,
                ThumbnailPath = model.ThumbnailPath,
                UserId = model.UserId,
                YtPlaylistId = model.YtPlaylistId,
                YtPlaylistItemId = model.YtPlaylistItemId,
                YtVideoId = model.YtVideoId,
                YtVideoTitle = model.YtVideoTitle
            });

            db.SaveChanges();
        }
    }

    public static void UpdateMediaItem(MediaItemModel model)
    {
        using (var db = new AudioMinerContext())
        {
            var entity = db.MediaItems.Where(p => p.Id == model.Id).FirstOrDefault();

            if (entity != null)
            {
                entity.ErrorMessage = model.ErrorMessage;
                entity.IsDeleted = model.IsDeleted;
                entity.IsSyncedToDropbox = model.IsSyncedToDropbox;
                entity.Mp3Path = model.Mp3Path;
                entity.Progress = model.Progress;
                entity.Status = model.Status;
                entity.ThumbnailPath = model.ThumbnailPath;
                entity.UpdatedAt = DateTime.UtcNow;

                db.SaveChanges();
            }
            
        }
    }

    public static void DeleteMediaItem(int id)
    {
        using (var db = new AudioMinerContext())
        {
            var entity = db.MediaItems.Where(p => p.Id == id).FirstOrDefault();

            if (entity != null)
            {
                entity.IsDeleted = true;

                db.SaveChanges();
            }          
        }
    }

}
