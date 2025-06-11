using eep_backend.Models.CourseModuleModels;
using eep_backend.Models.GameModuleModels;

namespace eep_backend.Models.UserModuleModels;

public class UserDto
{
    public int Id { get; set; }
    public string? Login { get; set; }
    
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public double? Rating { get; set; }
    public int? ThemeId { get; set; }
    public SettingDto? Theme { get; set; } 
    public int? FontId { get; set; }
    public SettingDto? Font { get; set; } 
    public int? ActiveCourseId { get; set; }
    public int? SelectedCharacterId { get; set; }
    public int? DepartmentId { get; set; }
    public int? RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool? IsActive { get; set; }
    
    public CourseDto? ActiveCourse { get; set; }
    public DepartmentDto? Department { get; set; }
    public UserRoleDto? Role { get; set; }
    public CharacterDto? SelectedCharacter { get; set; }
    public List<CourseDto>? ChosenCourses { get; set; }
    public List<AchievementDto>? Achievements { get; set; }
}