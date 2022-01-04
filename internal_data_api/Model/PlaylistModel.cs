using System.Text.Json.Serialization;
using internal_data_api.Context;

namespace internal_data_api.Model;

public class PlaylistModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("yt_playlist_id")]
    public string YtPlaylistId { get; set; }

    [JsonPropertyName("yt_playlist_name")]
    public string YtPlaylistName { get; set; }

    [JsonPropertyName("dropbox_token")]
    public string? DropboxToken { get; set; }

    [JsonPropertyName("sync_error")]
    public string? SyncError { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class PlaylistQuery
{
    public static List<PlaylistModel> GetPlaylists()
    {
        using (var db = new AudioMinerContext())
        {
            var ret = db.Playlists.Select(p => new PlaylistModel { 
                CreatedAt = p.CreatedAt,
                DropboxToken = p.DropboxToken,
                Id = p.Id,
                SyncError = p.SyncError,
                UpdatedAt = p.UpdatedAt,
                UserId = p.UserId,
                YtPlaylistId = p.YtPlaylistId,
                YtPlaylistName = p.YtPlaylistName
            }).ToList();

            return ret;
        }
    }

    public static PlaylistModel GetPlaylist(int id)
    {
        using (var db = new AudioMinerContext())
        {
            var ret = db.Playlists.Where(p => p.Id == id).Select(p => new PlaylistModel { 
                CreatedAt = p.CreatedAt,
                DropboxToken = p.DropboxToken,
                Id = p.Id,
                UpdatedAt = p.UpdatedAt,
                SyncError = p.SyncError,
                UserId = p.UserId,
                YtPlaylistId = p.YtPlaylistId,
                YtPlaylistName = p.YtPlaylistName
            }).FirstOrDefault();

            return ret;
        }
    }

    public static List<PlaylistModel> GetPlaylistsByUserId(int userId)
    {
        using (var db = new AudioMinerContext())
        {
            var ret = db.Playlists.Where(p => p.UserId == userId).Select(p => new PlaylistModel { 
                CreatedAt = p.CreatedAt,
                DropboxToken = p.DropboxToken,
                Id = p.Id,
                UpdatedAt = p.UpdatedAt,
                SyncError = p.SyncError,
                UserId = p.UserId,
                YtPlaylistId = p.YtPlaylistId,
                YtPlaylistName = p.YtPlaylistName
            }).ToList();

            return ret;
        }
    }

    public static PlaylistModel GetByYtPlaylistId(string ytPlaylistId)
    {
        using (var db = new AudioMinerContext())
        {
            var ret = db.Playlists.Where(p => p.YtPlaylistId == ytPlaylistId).Select(p => new PlaylistModel { 
                CreatedAt = p.CreatedAt,
                DropboxToken = p.DropboxToken,
                Id = p.Id,
                UpdatedAt = p.UpdatedAt,
                SyncError = p.SyncError,
                UserId = p.UserId,
                YtPlaylistId = p.YtPlaylistId,
                YtPlaylistName = p.YtPlaylistName
            }).FirstOrDefault();

            return ret;
        }
    }

    public static void AddPlaylist(PlaylistModel model)
    {
        using (var db = new AudioMinerContext())
        {
            db.Playlists.Add(new Context.Entites.Playlist {
                CreatedAt = DateTime.UtcNow,
                DropboxToken = model.DropboxToken,
                UserId = model.UserId,
                YtPlaylistId = model.YtPlaylistId,
                YtPlaylistName = model.YtPlaylistName
            });

            db.SaveChanges();
        }
    }

    public static void UpdatePlaylist(PlaylistModel model)
    {
        using (var db = new AudioMinerContext())
        {
            var entity = db.Playlists.Where(p => p.Id == model.Id).FirstOrDefault();

            if (entity != null)
            {
                entity.DropboxToken = model.DropboxToken;
                entity.SyncError = model.SyncError;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.YtPlaylistId = model.YtPlaylistId;
                entity.YtPlaylistName = model.YtPlaylistName;

                db.SaveChanges();
            }

            
        }
    }

    public static void DeletePlaylist(int id)
    {
        using (var db = new AudioMinerContext())
        {
            var entity = db.Playlists.Where(p => p.Id == id).FirstOrDefault();

            if (entity != null)
            {
                db.Playlists.Remove(entity);
                db.SaveChanges();
            }
        }
    }

}
