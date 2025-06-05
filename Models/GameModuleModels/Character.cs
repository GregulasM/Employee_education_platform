using SurrealDb.Net.Models;

namespace eep_backend.Models.GameModuleModels;

public class Character
{
    public RecordIdOfString id { get; set; }
    public string name { get; set; }
    public string? avatar { get; set; }
    public string? description { get; set; }
    public object? base_stats { get; set; }
    public object? cosmetics { get; set; }
    public List<object>? skills { get; set; }
    public bool? @default { get; set; }
    public string? rarity { get; set; }
    public string? unlock_cond { get; set; }
    public bool? is_active { get; set; }
}