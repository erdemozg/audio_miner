namespace internal_data_api.Context.Entites;

public class Log
{
    public int Id { get; set; }

    public string? LogType { get; set; }
    
    public string? Submitter { get; set; }
    
    public string Message { get; set; }

    public DateTime? CreatedAt { get; set; }

}
