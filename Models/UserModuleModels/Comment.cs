using SurrealDb.Net.Models;

namespace eep_backend.Models;

public class Comment
{
    public RecordIdOfString id { get; set; }
    public RecordIdOfString? news { get; set; }
    public RecordIdOfString? user { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public string? text { get; set; }
    public bool? is_active { get; set; }
}