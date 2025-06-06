using SurrealDb.Net.Models;

namespace eep_backend.Models.UserModuleModels;

public class User
{
    public string? login { get; set; }
    public string? password { get; set; }
    public string? phone_number { get; set; }
    public string? first_name { get; set; }
    public string? second_name { get; set; }
    public string? last_name { get; set; }
    public string? email { get; set; }
    public string? avatar { get; set; }
    public double? rating { get; set; }
    public RecordIdOfString? theme { get; set; } 
    public RecordIdOfString? font { get; set; }
    public RecordIdOfString? active_course { get; set; } 
    public List<RecordIdOfString>? chosen_courses { get; set; }
    public List<RecordIdOfString>? achievements { get; set; }
    public RecordIdOfString? selected_character { get; set; }
    public RecordIdOfString? department { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public bool? is_active { get; set; }

}