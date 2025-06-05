using SurrealDb.Net.Models;

namespace eep_backend.Models.CourseModuleModels;

public class Test
{
    public RecordIdOfString id { get; set; }
    public string title { get; set; }
    public string? description { get; set; }
    public RecordIdOfString? course { get; set; }
    public RecordIdOfString? module { get; set; }
    public List<object>? questions { get; set; }
    public double? time_limit { get; set; }
    public double? max_score { get; set; }
    public double? passing_score { get; set; }
    public double? attempts { get; set; }
    public RecordIdOfString? author { get; set; }
    public DateTime? available_from { get; set; }
    public DateTime? available_to { get; set; }
    public bool? randomize { get; set; }
    public List<string>? tags { get; set; }
    public bool? is_active { get; set; }
}