namespace eep_backend.Models.GameModuleModels;

public class AchievementCreateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public double? Points { get; set; }
}