namespace eep_backend.Models.CourseModuleModels;

public class ModuleDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Tags { get; set; }
    public double? Order { get; set; }
    public int? CourseId { get; set; }
    public string? CourseTitle { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } 
    public bool? IsActive { get; set; }
}