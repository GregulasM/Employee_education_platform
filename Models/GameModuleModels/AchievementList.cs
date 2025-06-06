

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.GameModuleModels;

public class AchievementList
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public double? SortType { get; set; }
    public bool? IsHidden { get; set; }

    public List<Achievement>? Achievements { get; set; }

    public bool? IsActive { get; set; }
}