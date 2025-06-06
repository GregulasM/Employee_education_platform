

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.CourseModuleModels;

public class Link
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public int? ParentId { get; set; }
    public Link? Parent { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Tags { get; set; }

    public bool? IsActive { get; set; }
}