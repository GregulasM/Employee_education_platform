using SurrealDb.Net.Models;

namespace eep_backend.Models.CourseModuleModels;

public class Article
{
    public RecordIdOfString id { get; set; }
    public string? title { get; set; }
    public RecordIdOfString? module { get; set; }
    public string? image { get; set; }
    public List<object>? content { get; set; }
    public string? excerpt { get; set; }
    public List<string>? images { get; set; }
    public RecordIdOfString? author { get; set; }
    public List<string>? tags { get; set; }
    public double? rating { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public RecordIdOfString? hidden_achievement { get; set; }
    public List<object>? attachments { get; set; }
    public bool? is_active { get; set; }
}