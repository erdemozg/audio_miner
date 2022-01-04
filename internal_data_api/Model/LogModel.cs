using System.Text.Json.Serialization;
using internal_data_api.Context;

namespace internal_data_api.Model;

public class LogModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("log_type")]
    public string? LogType { get; set; }
    
    [JsonPropertyName("submitter")]
    public string? Submitter { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }
}

public class LogQuery
{
    public static void CreateLog(LogModel model)
    {
        using (var db = new AudioMinerContext())
        {
            db.Logs.Add(new Context.Entites.Log {
                CreatedAt = DateTime.UtcNow,
                LogType = model.LogType,
                Message = model.Message,
                Submitter = model.Submitter
            });

            db.SaveChanges();
        }
    }
}
