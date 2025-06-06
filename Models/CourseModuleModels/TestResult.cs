

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.UserModuleModels;

namespace eep_backend.Models.CourseModuleModels;

public class TestResult
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public int? UserId { get; set; }
    public User? User { get; set; }

    public int? TestId { get; set; }
    public Test? Test { get; set; }

    public double? Score { get; set; }
    public double? MaxScore { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Answers { get; set; }

    public DateTime? Date { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }
}