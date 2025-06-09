namespace eep_backend.Models.GameModuleModels;

public class CharacterDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public string? BaseStats { get; set; }
    public string? Cosmetics { get; set; }
    public string? Skills { get; set; }
    public bool? Default { get; set; }
    public string? Rarity { get; set; }
    public string? UnlockCond { get; set; }
    public bool? IsActive { get; set; }
}