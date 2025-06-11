namespace eep_backend.Models.CourseModuleModels;

public class TestDto
{
    public int Id { get; set; }
    public string? PublicId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? ModuleId { get; set; }
    public string? ModuleTitle { get; set; }
    public int CourseId { get; set; }
    public string? CourseTitle { get; set; }
    public string? Questions { get; set; }
    public bool? IsActive { get; set; }
}