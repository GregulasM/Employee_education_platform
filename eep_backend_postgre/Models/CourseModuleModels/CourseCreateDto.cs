namespace eep_backend.Models.CourseModuleModels;

public class CourseCreateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Tags { get; set; }
    public double? Position { get; set; }
    public int? DepartmentId { get; set; }
}