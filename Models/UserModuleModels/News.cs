using SurrealDb.Net.Models;

namespace eep_backend.Models;

public class News
{
    public RecordIdOfString id { get; set; }
    public string title { get; set; }
    public string? slug { get; set; }
    public string? excerpt { get; set; }
    public List<object>? content { get; set; }
    public RecordIdOfString? author { get; set; }
    public string? type { get; set; }
    public DateTime? date { get; set; }
    public List<string>? tags { get; set; }
    public bool? is_pinned { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public bool? is_active { get; set; }
}