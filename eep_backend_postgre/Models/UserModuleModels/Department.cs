

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.UserModuleModels;

public class Department
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }
    public string? Description { get; set; }

    public List<User>? Users { get; set; }

    public bool? IsActive { get; set; }
}