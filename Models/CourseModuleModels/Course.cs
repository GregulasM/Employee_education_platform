using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.UserModuleModels;

namespace eep_backend.Models.CourseModuleModels;

public class Course
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    public List<string>? Tags { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public double? Position { get; set; }

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public List<Test>? Tests { get; set; }
    public List<Link>? UsefulLinks { get; set; }

    public bool? IsActive { get; set; }
}