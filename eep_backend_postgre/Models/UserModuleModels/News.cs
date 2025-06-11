

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.UserModuleModels;

public class News
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Excerpt { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Content { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    public string? Type { get; set; }
    public DateTime? Date { get; set; }

    [Column(TypeName = "jsonb")]
    public string? Tags { get; set; }

    public bool? IsPinned { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool? IsActive { get; set; }
}