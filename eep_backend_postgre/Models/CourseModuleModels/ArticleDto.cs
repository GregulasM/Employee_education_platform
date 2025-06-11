namespace eep_backend.Models.CourseModuleModels;

public class ArticleDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Image { get; set; }
    public string? Tags { get; set; }
    public string? Content { get; set; }
    public double? Rating { get; set; }
    public int? ModuleId { get; set; }
    public string? ModuleTitle { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool? IsActive { get; set; }
}