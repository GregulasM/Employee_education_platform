namespace eep_backend.Models.GameModuleModels;

public class AchievementDto
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public double? Points { get; set; }
    public int? ListId { get; set; }
}