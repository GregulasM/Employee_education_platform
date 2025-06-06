

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.GameModuleModels;
using eep_backend.Models.UserModuleModels;

namespace eep_backend.Models.CourseModuleModels;

public class Module
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Title { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public string? Description { get; set; }
    public string? Image { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    public double? Order { get; set; }

    public int? HiddenAchievementId { get; set; }
    public Achievement? HiddenAchievement { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Tags { get; set; }

    public bool? IsActive { get; set; }
}