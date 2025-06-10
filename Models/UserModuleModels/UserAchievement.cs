using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using eep_backend.Models.GameModuleModels;

namespace eep_backend.Models.UserModuleModels;

public class UserAchievement
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int AchievementId { get; set; }
    public Achievement Achievement { get; set; } = null!;
    
    public bool? IsActive { get; set; }
}