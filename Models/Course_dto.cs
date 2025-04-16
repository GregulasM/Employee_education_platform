namespace Employee_education_platform.Models;

public class Course_dto
{
    public string course_id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Duration { get; set; }
    public string Icon { get; set; } = null!;
}