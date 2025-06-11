

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.UserModuleModels;

namespace eep_backend.Models.GameModuleModels;

public class Achievement
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public double? Points { get; set; }

    public int? ListId { get; set; }
    public AchievementList? List { get; set; }
    public List<UserAchievement>? UserAchievements { get; set; }
    

    public bool? IsActive { get; set; }
}