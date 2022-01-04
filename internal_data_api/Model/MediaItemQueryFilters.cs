
namespace internal_data_api.Model;

public class MediaItemQueryFilters
{
    public int? PlaylistId { get; set; }

    public int? Skip { get; set; }

    public int? Take { get; set; }

    public string? YtTitle { get; set; }

    public string? User { get; set; }

    public bool ExcludeDeletedItems { get; set; }
}
