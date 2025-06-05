using SurrealDb.Net.Models;

namespace eep_backend.Models.CourseModuleModels;

public class Link
{
    public RecordIdOfString id { get; set; }
    public string? title { get; set; }
    public string? url { get; set; }
    public string? description { get; set; }
    public RecordIdOfString? course { get; set; }
    public RecordIdOfString? parent { get; set; }
    public List<string>? tags { get; set; }
    public bool? is_active { get; set; }
}