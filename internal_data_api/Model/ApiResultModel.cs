using System.Text.Json.Serialization;

namespace internal_data_api.Model;

public class ApiResultModel
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class ApiResultModel<T> : ApiResultModel
{    
    [JsonPropertyName("data")]
    public T Data { get; set; }
}

