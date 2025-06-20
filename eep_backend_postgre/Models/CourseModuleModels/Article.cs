

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.GameModuleModels;
using eep_backend.Models.UserModuleModels;

namespace eep_backend.Models.CourseModuleModels;

public class Article
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Title { get; set; }

    public int? ModuleId { get; set; }
    public Module? Module { get; set; }

    public string? Image { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Content { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Tags { get; set; }
    
    public double? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public bool? IsActive { get; set; }
}