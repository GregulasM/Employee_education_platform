using SurrealDb.Net.Models;

namespace eep_backend.Models.CourseModuleModels;

public class TestResult
{
    public RecordIdOfString id { get; set; }
    public RecordIdOfString? user { get; set; }
    public RecordIdOfString? test { get; set; }
    public double? score { get; set; }
    public double? max_score { get; set; }
    public List<object>? answers { get; set; }
    public DateTime? date { get; set; }
    public string? status { get; set; }
    public bool? is_active { get; set; }
}