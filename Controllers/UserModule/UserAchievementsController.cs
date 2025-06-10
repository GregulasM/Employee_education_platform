using eep_backend;
using eep_backend.Models.GameModuleModels;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class UserAchievementsController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public UserAchievementsController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создать (выдать) достижение пользователю
    /// </summary>
    [HttpPost("users/{userId}/achievements/{achievementId}")]
    public async Task<IActionResult> GiveAchievementToUser(int userId, int achievementId, CancellationToken cancellationToken)
    {
        var alreadyHas = await _dbContext.UserAchievements
            .AnyAsync(x => x.UserId == userId && x.AchievementId == achievementId, cancellationToken);

        if (alreadyHas)
            return BadRequest("Пользователь уже имеет это достижение.");

        var user = await _dbContext.Users.FindAsync(userId, cancellationToken);
        if (user == null) return NotFound("Пользователь не найден");

        var ach = await _dbContext.Achievements.FindAsync(achievementId, cancellationToken);
        if (ach == null) return NotFound("Достижение не найдено");

        var userAch = new UserAchievement { UserId = userId, AchievementId = achievementId };
        _dbContext.UserAchievements.Add(userAch);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var userAchievementDto = new UserAchievementDto
        {
            Id = userAch.Id,
            UserId = userAch.UserId,
            AchievementId = userAch.AchievementId,
            Achievement = new AchievementDto
            {
                Id = ach.Id,
                PublicId = ach.PublicId,
                Name = ach.Name,
                Description = ach.Description,
                Icon = ach.Icon,
                Points = ach.Points,
                ListId = ach.ListId
            }
        };

        return Ok(userAchievementDto);
    }

    /// <summary>
    /// Получить все связки пользователь-достижение
    /// </summary>
    [HttpGet("user-achievements")]
    public async Task<IActionResult> GetAllUserAchievements(CancellationToken cancellationToken)
    {
        var userAchievements = await _dbContext.UserAchievements
            .Where(userAch => userAch.IsActive != false)
            .Include(userAch => userAch.Achievement)
            .Include(userAch => userAch.User)
            .Select(userAch => new UserAchievementDto
            {
                Id = userAch.Id,
                UserId = userAch.UserId,
                AchievementId = userAch.AchievementId,
                Achievement = new AchievementDto
                {
                    Id = userAch.Achievement.Id,
                    PublicId = userAch.Achievement.PublicId,
                    Name = userAch.Achievement.Name,
                    Description = userAch.Achievement.Description,
                    Icon = userAch.Achievement.Icon,
                    Points = userAch.Achievement.Points,
                    ListId = userAch.Achievement.ListId
                },
                User = new UserDto
                {
                    Id = userAch.User.Id,
                    Login = userAch.User.Login,
                    Email = userAch.User.Email
                    // Добавьте остальные поля UserDto
                }
            })
            .ToListAsync(cancellationToken);

        return Ok(userAchievements);
    }

    /// <summary>
    /// Получить достижения конкретного пользователя
    /// </summary>
    [HttpGet("users/{userId}/achievements")]
    public async Task<IActionResult> GetUserAchievements(int userId, CancellationToken cancellationToken)
    {
        var achievements = await _dbContext.UserAchievements
            .Where(userAch => userAch.UserId == userId)
            .Select(userAch => new AchievementDto
            {
                Id = userAch.Achievement.Id,
                PublicId = userAch.Achievement.PublicId,
                Name = userAch.Achievement.Name,
                Description = userAch.Achievement.Description,
                Icon = userAch.Achievement.Icon,
                Points = userAch.Achievement.Points,
                ListId = userAch.Achievement.ListId
            })
            .ToListAsync(cancellationToken);

        return Ok(achievements);
    }

    /// <summary>
    /// Получить конкретную связку пользователь-достижение
    /// </summary>
    [HttpGet("user-achievements/{id}")]
    public async Task<IActionResult> GetUserAchievement(int id, CancellationToken cancellationToken)
    {
        var userAchievement = await _dbContext.UserAchievements
            .Include(userAch => userAch.Achievement)
            .Include(userAch => userAch.User)
            .Where(userAch => userAch.Id == id)
            .Select(userAch => new UserAchievementDto
            {
                Id = userAch.Id,
                UserId = userAch.UserId,
                AchievementId = userAch.AchievementId,
                Achievement = new AchievementDto
                {
                    Id = userAch.Achievement.Id,
                    PublicId = userAch.Achievement.PublicId,
                    Name = userAch.Achievement.Name,
                    Description = userAch.Achievement.Description,
                    Icon = userAch.Achievement.Icon,
                    Points = userAch.Achievement.Points,
                    ListId = userAch.Achievement.ListId
                },
                User = new UserDto
                {
                    Id = userAch.User.Id,
                    Login = userAch.User.Login,
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userAchievement == null)
            return NotFound("Связка пользователь-достижение не найдена");

        return Ok(userAchievement);
    }

    /// <summary>
    /// Удалить достижение у пользователя
    /// </summary>
    [HttpDelete("users/{userId}/achievements/{achievementId}")]
    public async Task<IActionResult> RemoveAchievementFromUser(int userId, int achievementId, CancellationToken cancellationToken)
    {
        var userAchievement = await _dbContext.UserAchievements
            .FirstOrDefaultAsync(userAch => userAch.UserId == userId && userAch.AchievementId == achievementId, cancellationToken);

        if (userAchievement == null)
            return NotFound("У пользователя нет этого достижения");

        userAchievement.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Достижение помечено как неактивное у пользователя");
    }

    /// <summary>
    /// Удалить связку пользователь-достижение по ID
    /// </summary>
    [HttpDelete("user_achievements/{id}")]
    public async Task<IActionResult> DeleteUserAchievement(int id, CancellationToken cancellationToken)
    {
        var userAchievement = await _dbContext.UserAchievements.FindAsync(id, cancellationToken);

        if (userAchievement == null)
            return NotFound("Связка пользователь-достижение не найдена");

        userAchievement.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Связка пользователь-достижение помечена как неактивная");
    }
}