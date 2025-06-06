

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.UserModuleModels;

namespace eep_backend.Models.CourseModuleModels;

public class Test
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Title { get; set; }
    public string? Description { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public int? ModuleId { get; set; }
    public Module? Module { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Questions { get; set; }

    public double? TimeLimit { get; set; }
    public double? MaxScore { get; set; }
    public double? PassingScore { get; set; }
    public int? Attempts { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }

    public bool? Randomize { get; set; }
    public List<string>? Tags { get; set; }
    public bool? IsActive { get; set; }
}