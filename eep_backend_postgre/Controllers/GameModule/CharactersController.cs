using eep_backend;
using eep_backend.Models.GameModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/admin_panel")]
public class CharactersAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public CharactersAdminController(SiteDbContext dbContext) => _dbContext = dbContext;

    [HttpPost("characters")]
    public async Task<IActionResult> Create_character([FromBody] CharacterCreateDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Имя персонажа обязательно.");
        var character = new Character {
            Name = dto.Name,
            Avatar = dto.Avatar,
            Description = dto.Description,
            BaseStats = dto.BaseStats,
            Cosmetics = dto.Cosmetics,
            Skills = dto.Skills,
            Default = dto.Default,
            Rarity = dto.Rarity,
            UnlockCond = dto.UnlockCond,
            IsActive = dto.IsActive ?? true,
            PublicId = Guid.NewGuid()
        };
        _dbContext.Characters.Add(character);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(character);
    }

    [HttpPatch("characters/{id}")]
    public async Task<IActionResult> Edit_character(int id, [FromBody] CharacterPatchDto dto, CancellationToken cancellationToken)
    {
        var character = await _dbContext.Characters.FindAsync(id, cancellationToken);
        if (character == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Name)) character.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Avatar)) character.Avatar = dto.Avatar;
        if (!string.IsNullOrWhiteSpace(dto.Description)) character.Description = dto.Description;
        if (!string.IsNullOrWhiteSpace(dto.BaseStats)) character.BaseStats = dto.BaseStats;
        if (!string.IsNullOrWhiteSpace(dto.Cosmetics)) character.Cosmetics = dto.Cosmetics;
        if (!string.IsNullOrWhiteSpace(dto.Skills)) character.Skills = dto.Skills;
        if (dto.Default.HasValue) character.Default = dto.Default;
        if (!string.IsNullOrWhiteSpace(dto.Rarity)) character.Rarity = dto.Rarity;
        if (!string.IsNullOrWhiteSpace(dto.UnlockCond)) character.UnlockCond = dto.UnlockCond;
        if (dto.IsActive.HasValue) character.IsActive = dto.IsActive;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(character);
    }

    [HttpPut("characters/{id}")]
    public async Task<IActionResult> Replace_character(int id, [FromBody] Character dto, CancellationToken cancellationToken)
    {
        var character = await _dbContext.Characters.FindAsync(id, cancellationToken);
        if (character == null) return NotFound();

        character.Name = dto.Name;
        character.Avatar = dto.Avatar;
        character.Description = dto.Description;
        character.BaseStats = dto.BaseStats;
        character.Cosmetics = dto.Cosmetics;
        character.Skills = dto.Skills;
        character.Default = dto.Default;
        character.Rarity = dto.Rarity;
        character.UnlockCond = dto.UnlockCond;
        character.IsActive = dto.IsActive;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(character);
    }

    [HttpDelete("characters/{id}")]
    public async Task<IActionResult> Delete_character(int id, CancellationToken cancellationToken)
    {
        var character = await _dbContext.Characters.FindAsync(id, cancellationToken);
        if (character == null) return NotFound();
        character.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Персонаж успешно помечен как неактивный.");
    }
}

[ApiController]
[Route("api")]
public class CharactersController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public CharactersController(SiteDbContext dbContext) => _dbContext = dbContext;

    [HttpGet("characters")]
    public async Task<IActionResult> Read_All_character(CancellationToken cancellationToken)
    {
        var chars = await _dbContext.Characters
            .Where(x => x.IsActive == true)
            .ToListAsync(cancellationToken);
        return Ok(chars);
    }

    [HttpGet("characters/{id}")]
    public async Task<IActionResult> Read_character(int id, CancellationToken cancellationToken)
    {
        var character = await _dbContext.Characters
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true, cancellationToken);
        if (character == null) return NotFound();
        return Ok(character);
    }
}
