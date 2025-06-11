

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.UserModuleModels;

public class Comment
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public int? NewsId { get; set; }
    public News? News { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string? Text { get; set; }
    public bool? IsActive { get; set; }
}