using SurrealDb.Net.Models;

namespace eep_backend.Models.CourseModuleModels;

public class Course
{
    public RecordIdOfString id { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }
    public string? image { get; set; }
    public RecordIdOfString? author { get; set; }
    public List<string>? tags { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public double? position { get; set; }
    public RecordIdOfString? department { get; set; }
    public List<RecordIdOfString>? tests { get; set; }
    public List<RecordIdOfString>? useful_links { get; set; }
    public bool? is_active { get; set; }
}