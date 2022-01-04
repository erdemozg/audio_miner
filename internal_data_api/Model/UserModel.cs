using System.Text.Json.Serialization;
using internal_data_api.Context;

namespace internal_data_api.Model;

public class UserModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class UserQuery
{
    public static List<UserModel> GetUsers()
    {
        using (var db = new AudioMinerContext())
        {
            var ret = db.Users.Select(user => new UserModel { 
                CreatedAt = user.CreatedAt,
                Password = user.Password,
                UpdatedAt = user.UpdatedAt,
                Id = user.Id,
                Username = user.Username
            }).ToList();

            return ret;
        }
    }

    public static UserModel GetUser(int id)
    {
        using (var db = new AudioMinerContext())
        {
            var ret = db.Users.Where(user => user.Id == id).Select(user => new UserModel { 
                CreatedAt = user.CreatedAt,
                Password = user.Password,
                UpdatedAt = user.UpdatedAt,
                Id = user.Id,
                Username = user.Username
            }).FirstOrDefault();

            return ret;
        }
    }

    public static UserModel GetUserByUsername(string username)
    {
        using (var db = new AudioMinerContext())
        {
            var ret = db.Users.AsQueryable().Where(user => user.Username == username).Select(user => new UserModel { 
                CreatedAt = user.CreatedAt,
                Password = user.Password,
                UpdatedAt = user.UpdatedAt,
                Id = user.Id,
                Username = user.Username
            }).FirstOrDefault();

            return ret;
        }
    }

    public static void CreateUser(UserModel model)
    {
        using (var db = new AudioMinerContext())
        {
            db.Users.Add(new Context.Entites.User {
                CreatedAt = DateTime.UtcNow,
                Password = model.Password,
                Username = model.Username
            });

            db.SaveChanges();
        }
    }
}
