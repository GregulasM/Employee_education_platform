using eep_backend;
using eep_backend.Models.GameModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class AchievementListsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public AchievementListsAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Метод для создания листа достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания листа достижений.",
        Description = "Создает лист достижений."
    )]
    [HttpPost("achievementlists")]
    public async Task<IActionResult> Create_list([FromBody] AchievementList dto, CancellationToken cancellationToken)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Название листа обязательно.");

        var list = new AchievementList
        {
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
            SortType = dto.SortType,
            IsHidden = dto.IsHidden,
            IsActive = true
        };

        _dbContext.AchievementLists.Add(list);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return Ok(new { list.Id, list.Name });
    }
    
    // /// <summary>
    // /// Метод для выгрузки всех листов достижений.
    // /// </summary>
    // [SwaggerOperation(
    //     Summary = "Метод для выгрузки всех листов достижений.",
    //     Description = "Выгружает все листы достижений."
    // )]
    // [HttpGet("achievementlists")]
    // public async Task<IActionResult> Read_All_list(CancellationToken cancellationToken)
    // {
    //     var lists = await _dbContext.AchievementLists
    //         .Include(l => l.Achievements)
    //         .Where(l => l.IsActive == true)
    //         .Select(l => new {
    //             l.Id,
    //             l.PublicId,
    //             l.Name,
    //             l.Description,
    //             l.Icon,
    //             l.SortType,
    //             l.IsHidden,
    //             l.IsActive,
    //             Achievements = l.Achievements != null ? l.Achievements.Where(a => a.IsActive == true).Select(a => new { a.Id, a.Name }).ToList() : null
    //         })
    //         .ToListAsync(cancellationToken);
    //
    //     return Ok(lists);
    // }

    /// <summary>
    /// Метод для частичного изменения листа достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения листа достижений.",
        Description = "Частично меняет лист достижений."
    )]
    [HttpPatch("achievementlists/{id}")]
    public async Task<IActionResult> Edit_list(int id, [FromBody] AchievementList patch, CancellationToken cancellationToken)
    {
        var list = await _dbContext.AchievementLists.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true, cancellationToken);
        if (list == null)
            return NotFound("Лист не найден");

        if (!string.IsNullOrWhiteSpace(patch.Name)) list.Name = patch.Name;
        if (!string.IsNullOrWhiteSpace(patch.Description)) list.Description = patch.Description;
        if (!string.IsNullOrWhiteSpace(patch.Icon)) list.Icon = patch.Icon;
        if (patch.SortType.HasValue) list.SortType = patch.SortType;
        if (patch.IsHidden.HasValue) list.IsHidden = patch.IsHidden;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(list);
    }

    /// <summary>
    /// Метод для полного изменения листа достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения листа достижений.",
        Description = "Полностью меняет лист достижений."
    )]
    [HttpPut("achievementlists/{id}")]
    public async Task<IActionResult> Replace_list(int id, [FromBody] AchievementList dto, CancellationToken cancellationToken)
    {
        var list = await _dbContext.AchievementLists.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true, cancellationToken);
        if (list == null)
            return NotFound("Лист не найден");

        list.Name = dto.Name;
        list.Description = dto.Description;
        list.Icon = dto.Icon;
        list.SortType = dto.SortType;
        list.IsHidden = dto.IsHidden;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(list);
    }

    /// <summary>
    /// Метод для удаления листа достижений (soft delete).
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления листа достижений.",
        Description = "Удаляет лист достижений."
    )]
    [HttpDelete("achievementlists/{id}")]
    public async Task<IActionResult> Delete_list(int id, CancellationToken cancellationToken)
    {
        var list = await _dbContext.AchievementLists.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true, cancellationToken);
        if (list == null)
            return NotFound();

        list.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Лист достижений успешно помечен как неактивный.");
    }
}


[ApiController]
[Route("api")]
public class AchievementListsController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public AchievementListsController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    

    /// <summary>
    /// Метод для выгрузки только одного листа достижений по id.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одного листа достижений по id.",
        Description = "Выгружает только один лист достижений по id."
    )]
    [HttpGet("achievementlists/{id}")]
    public async Task<IActionResult> Read_list(int id, CancellationToken cancellationToken)
    {
        var list = await _dbContext.AchievementLists
            .Include(l => l.Achievements)
            .Where(l => l.Id == id && l.IsActive == true)
            .Select(l => new {
                l.Id,
                l.PublicId,
                l.Name,
                l.Description,
                l.Icon,
                l.SortType,
                l.IsHidden,
                l.IsActive,
                Achievements = l.Achievements != null ? l.Achievements.Where(a => a.IsActive == true).Select(a => new { a.Id, a.Name }).ToList() : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (list == null)
            return NotFound();

        return Ok(list);
    }
}