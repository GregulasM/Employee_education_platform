using SurrealDb.Net.Models;

namespace eep_backend.Models.UserModuleModels;

public class Setting
{
    public RecordIdOfString id { get; set; } 
    public string? type { get; set; }           
    public string? name { get; set; } 
    public Dictionary<string, object>? value { get; set; }
    public string? icon { get; set; }
    public bool? is_active { get; set; } 
}