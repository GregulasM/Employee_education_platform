namespace eep_backend.Models.CourseModuleModels;

public class ModulePatchDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Tags { get; set; }
    public double? Order { get; set; }
    public int? HiddenAchievementId { get; set; }
    
    public int? CourseId { get; set; }
    public bool? IsActive { get; set; }
}