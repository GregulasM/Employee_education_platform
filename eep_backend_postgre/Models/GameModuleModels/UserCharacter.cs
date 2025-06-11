using System.ComponentModel.DataAnnotations.Schema;

namespace eep_backend.Models.GameModuleModels;
using eep_backend.Models.UserModuleModels;

public class UserCharacter
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public int CharacterId { get; set; }
    public Character? Character { get; set; }
    
    public int Level { get; set; }
    public double Experience { get; set; }
    
    [Column(TypeName = "jsonb")]
    public string? CustomStats { get; set; }

}