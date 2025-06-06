using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.CourseModuleModels;
using eep_backend.Models.GameModuleModels;


namespace eep_backend.Models.UserModuleModels;

public class User
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    [MaxLength(100)]
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
    public Setting? Theme { get; set; }

    public int? FontId { get; set; }
    public Setting? Font { get; set; }

    public int? ActiveCourseId { get; set; }
    public Course? ActiveCourse { get; set; }

    public int? SelectedCharacterId { get; set; }
    public Character? SelectedCharacter { get; set; }

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public List<Course>? ChosenCourses { get; set; }
    public List<Achievement>? Achievements { get; set; }
    public List<UserCharacter>? UserCharacters { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool? IsActive { get; set; }

}