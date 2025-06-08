namespace eep_backend.Models.GameModuleModels;

public class AchievementListDto
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}