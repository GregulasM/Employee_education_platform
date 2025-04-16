namespace Employee_education_platform.Models;

public class Module_dto
{
    public string module_id { get; set; } = Guid.NewGuid().ToString();
    public string course_id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int Article_count { get; set; }
    public object? Hidden_achievement { get; set; }
}