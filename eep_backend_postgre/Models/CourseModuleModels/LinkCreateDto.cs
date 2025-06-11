namespace eep_backend.Models.CourseModuleModels;

public class LinkCreateDto
{
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; } // stringified JSON array
    public int? ParentId { get; set; }
    public int? CourseId { get; set; }
}