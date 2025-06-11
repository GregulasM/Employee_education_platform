

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.GameModuleModels;

public class Character
{
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }

    [Column(TypeName = "jsonb")]
    public string? BaseStats { get; set; } 

    [Column(TypeName = "jsonb")]
    public string? Cosmetics { get; set; } 

    [Column(TypeName = "jsonb")]
    public string? Skills { get; set; }    

    public bool? Default { get; set; }
    public string? Rarity { get; set; }
    public string? UnlockCond { get; set; }
    public bool? IsActive { get; set; }
}