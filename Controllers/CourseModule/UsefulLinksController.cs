using eep_backend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using eep_backend.Models.CourseModuleModels;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class UsefulLinksAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public UsefulLinksAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создать полезную ссылку в курсе.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания полезной ссылки в курсе.", 
        Description = "Создает полезную ссылку в курсе."
    )]
    [HttpPost("courses/{name_course}/usefullinks")]
    public async Task<IActionResult> Create_usefullink(string name_course, [FromBody] LinkCreateDto dto, CancellationToken cancellationToken)
    {
        if (dto == null) return BadRequest("Ссылка не передана");
        if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Название обязательно");
        // Найти курс
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound("Курс не найден");

        var link = new Link
        {
            Title = dto.Title,
            Url = dto.Url,
            Description = dto.Description,
            Tags = dto.Tags ?? "[]",
            CourseId = course.Id,
            ParentId = dto.ParentId,
            IsActive = true
        };

        _dbContext.Links.Add(link);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Read_usefullink), new { name_course = course.Title, name_usefullink = link.Title }, dto);
    }
    
    /// <summary>
    /// Получить одну ссылку из курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одной полезной ссылки из курса.",
        Description = "Выгружает только одну полезную ссылку из курса."
    )]
    [HttpGet("courses/{name_course}/usefullinks/{name_usefullink}")]
    public async Task<IActionResult> Read_usefullink(string name_course, string name_usefullink, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound("Курс не найден");
        var link = await _dbContext.Links.FirstOrDefaultAsync(l => l.Title == name_usefullink && l.CourseId == course.Id && l.IsActive == true, cancellationToken);
        if (link == null) return NotFound("Ссылка не найдена");
        return Ok(link);
    }

    /// <summary>
    /// Частичное изменение полезной ссылки.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения полезной ссылки в курсе.", 
        Description = "Частично меняет полезную ссылку в курсе."
    )]
    [HttpPatch("useful_links/{id}")]
    public async Task<IActionResult> PatchUsefulLink(int id, [FromBody] LinkPatchDto dto, CancellationToken cancellationToken)
    {
        var link = await _dbContext.Links.FirstOrDefaultAsync(l => l.Id == id && l.IsActive == true, cancellationToken);
        if (link == null) return NotFound("Ссылка не найдена");
        if (!string.IsNullOrWhiteSpace(dto.Title)) link.Title = dto.Title;
        if (!string.IsNullOrWhiteSpace(dto.Url)) link.Url = dto.Url;
        if (!string.IsNullOrWhiteSpace(dto.Description)) link.Description = dto.Description;
        if (dto.Tags != null) link.Tags = dto.Tags;
        if (dto.ParentId.HasValue) link.ParentId = dto.ParentId;
        if (dto.CourseId.HasValue) link.CourseId = dto.CourseId;
        if (dto.IsActive.HasValue) link.IsActive = dto.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(link);
    }

    /// <summary>
    /// Полное изменение полезной ссылки.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения полезной ссылки в курсе.", 
        Description = "Полностью меняет полезную ссылку в курсе."
    )]
    [HttpPut("courses/{name_course}/usefullinks/{name_usefullink}")]
    public async Task<IActionResult> Replace_usefullink(string name_course, string name_usefullink, [FromBody] LinkCreateDto dto, CancellationToken cancellationToken)
    {
        // Найти курс
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound("Курс не найден");
        // Найти ссылку
        var link = await _dbContext.Links.FirstOrDefaultAsync(l => l.Title == name_usefullink && l.CourseId == course.Id && l.IsActive == true, cancellationToken);
        if (link == null) return NotFound("Ссылка не найдена");

        link.Title = dto.Title ?? link.Title;
        link.Url = dto.Url ?? link.Url;
        link.Description = dto.Description ?? link.Description;
        link.Tags = dto.Tags ?? link.Tags;
        link.ParentId = dto.ParentId;
        link.CourseId = dto.CourseId ?? link.CourseId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(link);
    }

    /// <summary>
    /// Soft-delete (IsActive = false)
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления полезной ссылки из курса.", 
        Description = "Удаляет полезную ссылку из курса."
    )]
    [HttpDelete("useful_links/{id}")]
    public async Task<IActionResult> DeleteUsefulLink(int id, CancellationToken cancellationToken)
    {
        var link = await _dbContext.Links.FirstOrDefaultAsync(l => l.Id == id && l.IsActive == true, cancellationToken);
        if (link == null) return NotFound("Ссылка не найдена");
        if (link.IsActive == false) return BadRequest("Ссылка уже удалена");

        link.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Ссылка помечена как неактивная.");
    }
}

[ApiController]
[Route("api")]
public class UsefulLinksController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public UsefulLinksController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все полезные ссылки (по всем курсам)
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех полезных ссылок из курса.",
        Description = "Выгружает все полезные ссылки."
    )]
    [HttpGet("useful_links")]
    public async Task<IActionResult> Read_All_usefullink(CancellationToken cancellationToken)
    {
        var result = await _dbContext.Links
            .Where(l => l.IsActive == true)
            .OrderByDescending(l => l.Id)
            .ToListAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Получить все полезные ссылки курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех полезных ссылок из курса.",
        Description = "Выгружает все полезные ссылки из курса."
    )]
    [HttpGet("courses/{name_course}/usefullinks")]
    public async Task<IActionResult> Read_All_usefullink_in_course(string name_course, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound("Курс не найден");
        var links = await _dbContext.Links
            .Where(l => l.CourseId == course.Id && l.IsActive == true)
            .OrderByDescending(l => l.Id)
            .ToListAsync(cancellationToken);
        return Ok(links);
    }

    
}