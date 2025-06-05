using SurrealDb.Net.Models;

namespace eep_backend.Models;

public class Department
{
    public RecordIdOfString id { get; set; }
    public string name { get; set; }
    public string? description { get; set; }
    public RecordIdOfString? manager { get; set; }
    public bool? is_active { get; set; }
}