namespace eep_backend.Models.CourseModuleModels;

public class CourseDto
{
    public int Id { get; set; }
    public string? PublicId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Tags { get; set; }
    public double? Position { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public List<TestDto> Tests { get; set; } = new();
}