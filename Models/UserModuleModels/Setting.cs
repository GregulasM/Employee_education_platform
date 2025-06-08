using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.UserModuleModels;

public class Setting
{
    public int Id { get; set; }
    
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();
    
    public string? Type { get; set; }
    public string? Name { get; set; }
    
    [Column(TypeName = "jsonb")]
    public string? Value { get; set; } 
    
    public string? Icon { get; set; }
    public bool? IsActive { get; set; }
    
    
}