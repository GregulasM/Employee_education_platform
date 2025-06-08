namespace eep_backend.Models.CourseModuleModels;

public class TestCreateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? CourseId { get; set; }
    public int? ModuleId { get; set; }
    public string? Questions { get; set; }
}