using eep_backend;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class SettingsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public SettingsAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Метод для создания настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания настройки у пользователя.",
        Description = "Создает настройку у пользователя."
    )]
    [HttpPost("settings")]
    public async Task<IActionResult> Create_setting([FromBody] Setting setting, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(setting.Name) || string.IsNullOrWhiteSpace(setting.Type))
            return BadRequest("Имя и тип настройки обязательны.");

        setting.Id = 0;
        _dbContext.Settings.Add(setting);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Read_setting_by_id), new { id = setting.Id }, setting);
    }

    /// <summary>
    /// Метод для полного изменения настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения настройки у пользователя.",
        Description = "Полностью меняет настройку у пользователя."
    )]
    [HttpPut("settings/{id}")]
    public async Task<IActionResult> Replace_setting(int id, [FromBody] Setting setting, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Settings.FindAsync(new object[] { id }, cancellationToken);
        if (existing == null)
            return NotFound("Настройка не найдена");

        existing.Type = setting.Type;
        existing.Name = setting.Name;
        existing.Value = setting.Value;
        existing.Icon = setting.Icon;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(existing);
    }

    /// <summary>
    /// Метод для частичного изменения настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения настройки у пользователя.",
        Description = "Частично меняет настройку у пользователя."
    )]
    [HttpPatch("settings/{id}")]
    public async Task<IActionResult> Edit_setting(int id, [FromBody] Setting patch, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Settings.FindAsync(new object[] { id }, cancellationToken);
        if (existing == null)
            return NotFound("Настройка не найдена");

        if (!string.IsNullOrWhiteSpace(patch.Type)) existing.Type = patch.Type;
        if (!string.IsNullOrWhiteSpace(patch.Name)) existing.Name = patch.Name;
        if (!string.IsNullOrWhiteSpace(patch.Value)) existing.Value = patch.Value;
        if (!string.IsNullOrWhiteSpace(patch.Icon)) existing.Icon = patch.Icon;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(existing);
    }

    /// <summary>
    /// Метод для удаления настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления настройки у пользователя.",
        Description = "Удаляет настройку у пользователя."
    )]
    [HttpDelete("settings/{id}")]
    public async Task<IActionResult> Delete_setting(int id, CancellationToken cancellationToken)
    {
        var setting = await _dbContext.Settings.FindAsync(new object[] { id }, cancellationToken);
        if (setting == null)
            return NotFound("Настройка не найдена");

        if (setting.IsActive == false)
            return BadRequest("Настройка уже неактивна.");

        setting.IsActive = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Настройка успешно помечена как неактивная.");
    }

    /// <summary>
    /// Метод для выгрузки только одной настройки по id.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одной настройки у пользователя.",
        Description = "Выгружает только одну настройку по её id."
    )]
    [HttpGet("settings/{id}")]
    public async Task<IActionResult> Read_setting_by_id(int id, CancellationToken cancellationToken)
    {
        var setting = await _dbContext.Settings.FindAsync(new object[] { id }, cancellationToken);
        if (setting == null)
            return NotFound("Настройка не найдена");
        return Ok(setting);
    }

    /// <summary>
    /// Метод для выгрузки всех настроек.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех настроек.",
        Description = "Выгружает все доступные настройки."
    )]
    [HttpGet("settings")]
    public async Task<IActionResult> Read_All_settings([FromQuery] string? type, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Settings.AsQueryable();
        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(s => s.Type == type);

        var list = await query.ToListAsync(cancellationToken);
        return Ok(list);
    }
}
