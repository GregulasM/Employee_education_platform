using SurrealDb.Net.Models;

namespace eep_backend.Models.CourseModuleModels;

public class Module
{
    public RecordIdOfString id { get; set; }
    public string? title { get; set; }
    public RecordIdOfString? course { get; set; }
    public string? description { get; set; }
    public string? image { get; set; }
    public RecordIdOfString? author { get; set; }
    public double? order { get; set; }
    public RecordIdOfString? hidden_achievement { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public List<string>? tags { get; set; }
    public bool? is_active { get; set; }
}