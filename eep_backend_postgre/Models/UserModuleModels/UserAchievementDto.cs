using eep_backend.Models.GameModuleModels;

namespace eep_backend.Models.UserModuleModels;

public class UserAchievementDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AchievementId { get; set; }
    public AchievementDto? Achievement { get; set; }
    public UserDto? User { get; set; }
    
    public bool? IsActive { get; set; }
}