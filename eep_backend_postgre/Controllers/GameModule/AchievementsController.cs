using eep_backend;
using eep_backend.Models.GameModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/admin_panel")]
public class AchievementsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public AchievementsAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все листы достижений (для выпадающего списка).
    /// </summary>
    [HttpGet("achievementlists")]
    public async Task<IActionResult> GetAchievementLists(CancellationToken cancellationToken)
    {
        var lists = await _dbContext.AchievementLists
            .Where(l => l.IsActive == true)
            .Select(l => new AchievementListDto
            {
                Id = l.Id,
                PublicId = l.PublicId,
                Name = l.Name,
                Description = l.Description
            }).ToListAsync(cancellationToken);
        return Ok(lists);
    }

    /// <summary>
    /// Создать достижение в определенном списке достижений.
    /// </summary>
    [HttpPost("achievementlists/{listId}/achievements")]
    public async Task<IActionResult> CreateAchievement(int listId, [FromBody] AchievementCreateDto dto, CancellationToken cancellationToken)
    {
        var achievementList = await _dbContext.AchievementLists
            .FirstOrDefaultAsync(l => l.Id == listId && l.IsActive == true, cancellationToken);
        if (achievementList == null)
            return NotFound("Лист достижений не найден.");

        var achievement = new Achievement
        {
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
            Points = dto.Points,
            ListId = listId,
            IsActive = true
        };
        _dbContext.Achievements.Add(achievement);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new AchievementDto
        {
            Id = achievement.Id,
            PublicId = achievement.PublicId,
            Name = achievement.Name,
            Description = achievement.Description,
            Icon = achievement.Icon,
            Points = achievement.Points,
            ListId = achievement.ListId
        });
    }

    /// <summary>
    /// Частичное изменение достижения.
    /// </summary>
    [HttpPatch("achievementlists/{listId}/achievements/{achievementId}")]
    public async Task<IActionResult> EditAchievement(int listId, int achievementId, [FromBody] AchievementCreateDto dto, CancellationToken cancellationToken)
    {
        var achievement = await _dbContext.Achievements
            .FirstOrDefaultAsync(a => a.Id == achievementId && a.ListId == listId && a.IsActive == true, cancellationToken);
        if (achievement == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Name))
            achievement.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Description))
            achievement.Description = dto.Description;
        if (!string.IsNullOrWhiteSpace(dto.Icon))
            achievement.Icon = dto.Icon;
        if (dto.Points.HasValue)
            achievement.Points = dto.Points;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new AchievementDto
        {
            Id = achievement.Id,
            PublicId = achievement.PublicId,
            Name = achievement.Name,
            Description = achievement.Description,
            Icon = achievement.Icon,
            Points = achievement.Points,
            ListId = achievement.ListId
        });
    }

    /// <summary>
    /// Полное изменение достижения.
    /// </summary>
    [HttpPut("achievementlists/{listId}/achievements/{achievementId}")]
    public async Task<IActionResult> ReplaceAchievement(int listId, int achievementId, [FromBody] AchievementCreateDto dto, CancellationToken cancellationToken)
    {
        var achievement = await _dbContext.Achievements
            .FirstOrDefaultAsync(a => a.Id == achievementId && a.ListId == listId && a.IsActive == true, cancellationToken);
        if (achievement == null)
            return NotFound();

        achievement.Name = dto.Name;
        achievement.Description = dto.Description;
        achievement.Icon = dto.Icon;
        achievement.Points = dto.Points;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new AchievementDto
        {
            Id = achievement.Id,
            PublicId = achievement.PublicId,
            Name = achievement.Name,
            Description = achievement.Description,
            Icon = achievement.Icon,
            Points = achievement.Points,
            ListId = achievement.ListId
        });
    }

    /// <summary>
    /// Мягкое удаление достижения.
    /// </summary>
    [HttpDelete("achievementlists/{listId}/achievements/{achievementId}")]
    public async Task<IActionResult> DeleteAchievement(int listId, int achievementId, CancellationToken cancellationToken)
    {
        var achievement = await _dbContext.Achievements
            .FirstOrDefaultAsync(a => a.Id == achievementId && a.ListId == listId && a.IsActive == true, cancellationToken);
        if (achievement == null)
            return NotFound();

        achievement.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Достижение помечено как неактивное.");
    }
}

// --- API для чтения достижений ---

[ApiController]
[Route("api")]
public class AchievementsController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public AchievementsController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все достижения в листе.
    /// </summary>
    [HttpGet("achievementlists/{listId}/achievements")]
    public async Task<IActionResult> GetAchievements(int listId, CancellationToken cancellationToken)
    {
        var achievements = await _dbContext.Achievements
            .Where(a => a.ListId == listId && a.IsActive == true)
            .Select(a => new AchievementDto
            {
                Id = a.Id,
                PublicId = a.PublicId,
                Name = a.Name,
                Description = a.Description,
                Icon = a.Icon,
                Points = a.Points,
                ListId = a.ListId
            })
            .ToListAsync(cancellationToken);

        return Ok(achievements);
    }

    /// <summary>
    /// Получить достижение по id.
    /// </summary>
    [HttpGet("achievementlists/{listId}/achievements/{achievementId}")]
    public async Task<IActionResult> GetAchievement(int listId, int achievementId, CancellationToken cancellationToken)
    {
        var achievement = await _dbContext.Achievements
            .Where(a => a.Id == achievementId && a.ListId == listId && a.IsActive == true)
            .Select(a => new AchievementDto
            {
                Id = a.Id,
                PublicId = a.PublicId,
                Name = a.Name,
                Description = a.Description,
                Icon = a.Icon,
                Points = a.Points,
                ListId = a.ListId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (achievement == null)
            return NotFound();

        return Ok(achievement);
    }
}