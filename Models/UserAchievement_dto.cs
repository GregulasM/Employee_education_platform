namespace Employee_education_platform.Models;

public class UserAchievement_dto
{
    public string user_achievement_id { get; set; } = null!;
    public string user_id { get; set; } = null!;
    public string achievement_id { get; set; } = null!;
    public DateTime date_earned { get; set; }
    public float progress { get; set; }
}