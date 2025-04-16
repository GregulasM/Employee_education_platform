namespace Employee_education_platform.Models;

public class CourseProgress_dto
{
    public string progress_id { get; set; } = null!;
    public string user_id { get; set; } = null!;
    public string course_id { get; set; } = null!;
    public float percent_completed { get; set; }
    public DateTime last_update_date { get; set; }
}