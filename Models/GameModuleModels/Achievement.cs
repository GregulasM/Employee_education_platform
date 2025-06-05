using SurrealDb.Net.Models;

namespace eep_backend.Models.GameModuleModels;

public class Achievement
{
    public RecordIdOfString id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public string? icon { get; set; }
    public double? points { get; set; }
    public RecordIdOfString? list { get; set; }
    public bool? is_active { get; set; }
}