namespace Employee_education_platform.Models;

public class User_dto
{
    public string user_id { get; set; } = null!;
    public string username { get; set; } = null!;
    public string email { get; set; } = null!;
    public string password_hash { get; set; } = null!;
    public object profile_settings { get; set; } = null!;
    public object theme_preferences { get; set; } = null!;
    public string character_id { get; set; } = null!;
}