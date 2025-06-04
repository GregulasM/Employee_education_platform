using SurrealDb.Net.Models;

namespace eep_backend.Models;

public class User
{
    public RecordIdOfString id { get; set; }
    public string? login { get; set; }
    public string? password { get; set; }
    public string? phone_number { get; set; }
    public string? first_name { get; set; }
    public string? second_name { get; set; }
    public string? last_name { get; set; }
    public string? email { get; set; }
    public string? avatar { get; set; }
    public double? rating { get; set; }
    public string? theme { get; set; } 
    public string? font { get; set; }
    public string? active_course { get; set; } 
    public List<string>? chosen_courses { get; set; }
    public List<string>? achievements { get; set; }
    public string? selected_character { get; set; }
    public string? department { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public bool? is_active { get; set; }
}