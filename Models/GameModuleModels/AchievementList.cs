using SurrealDb.Net.Models;

namespace eep_backend.Models.GameModuleModels;

public class AchievementList
{
    public RecordIdOfString id { get; set; }
    public string name { get; set; }
    public string? description { get; set; }
    public string? icon { get; set; }
    public double? sort_type { get; set; }
    public bool? is_hidden { get; set; }
    public List<RecordIdOfString>? achievements { get; set; }
    public bool? is_active { get; set; }
}