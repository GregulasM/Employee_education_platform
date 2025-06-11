using eep_backend;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class NewsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public NewsAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создать новость.
    /// </summary>
    [SwaggerOperation(Summary = "Создать новость", Description = "Создает новую новость")]
    [HttpPost("users/news")]
    public async Task<IActionResult> Create_news([FromBody] News news, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(news.Title) || string.IsNullOrWhiteSpace(news.Slug))
            return BadRequest("Заголовок и slug обязательны.");
        news.Id = 0;
        news.IsActive = true;
        news.CreatedAt = DateTime.UtcNow;
        news.UpdatedAt = DateTime.UtcNow;
        _dbContext.News.Add(news);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Read_news), new { slug_news = news.Slug }, news);
    }
    
    /// <summary>
    /// Получить новость по slug.
    /// </summary>
    [SwaggerOperation(Summary = "Получить новость по slug", Description = "Выгружает новость по slug")]
    [HttpGet("users/news/{slug_news}")]
    public async Task<IActionResult> Read_news(string slug_news, CancellationToken cancellationToken)
    {
        var n = await _dbContext.News
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Slug == slug_news && x.IsActive == true, cancellationToken);
        if (n == null) return NotFound("Новость не найдена");
        return Ok(n);
    }

    /// <summary>
    /// Частично обновить новость.
    /// </summary>
    [SwaggerOperation(Summary = "Частично обновить новость", Description = "Частично обновляет новость")]
    [HttpPatch("users/news/{slug_news}")]
    public async Task<IActionResult> Edit_news(string slug_news, [FromBody] News patch, CancellationToken cancellationToken)
    {
        var n = await _dbContext.News.FirstOrDefaultAsync(x => x.Slug == slug_news, cancellationToken);
        if (n == null) return NotFound("Новость не найдена");

        if (!string.IsNullOrWhiteSpace(patch.Title)) n.Title = patch.Title;
        if (!string.IsNullOrWhiteSpace(patch.Excerpt)) n.Excerpt = patch.Excerpt;
        if (!string.IsNullOrWhiteSpace(patch.Content)) n.Content = patch.Content;
        if (patch.AuthorId.HasValue) n.AuthorId = patch.AuthorId;
        if (!string.IsNullOrWhiteSpace(patch.Type)) n.Type = patch.Type;
        if (patch.Date.HasValue) n.Date = patch.Date;
        if (!string.IsNullOrWhiteSpace(patch.Tags)) n.Tags = patch.Tags;
        if (patch.IsPinned.HasValue) n.IsPinned = patch.IsPinned;
        if (patch.IsActive.HasValue) n.IsActive = patch.IsActive;
        if (!string.IsNullOrWhiteSpace(patch.Slug) && patch.Slug != n.Slug)
            n.Slug = patch.Slug;
        n.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(n);
    }

    /// <summary>
    /// Полностью заменить новость.
    /// </summary>
    [SwaggerOperation(Summary = "Полностью заменить новость", Description = "Полностью заменяет новость")]
    [HttpPut("users/news/{slug_news}")]
    public async Task<IActionResult> Replace_news(string slug_news, [FromBody] News news, CancellationToken cancellationToken)
    {
        var n = await _dbContext.News.FirstOrDefaultAsync(x => x.Slug == slug_news, cancellationToken);
        if (n == null) return NotFound("Новость не найдена");

        n.Title = news.Title;
        n.Slug = news.Slug;
        n.Excerpt = news.Excerpt;
        n.Content = news.Content;
        n.AuthorId = news.AuthorId;
        n.Type = news.Type;
        n.Date = news.Date;
        n.Tags = news.Tags;
        n.IsPinned = news.IsPinned;
        n.IsActive = news.IsActive;
        n.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(n);
    }

    /// <summary>
    /// Удалить новость (мягко).
    /// </summary>
    [SwaggerOperation(Summary = "Удалить новость", Description = "Мягко удаляет новость")]
    [HttpDelete("users/news/{slug_news}")]
    public async Task<IActionResult> Delete_news(string slug_news, CancellationToken cancellationToken)
    {
        var n = await _dbContext.News.FirstOrDefaultAsync(x => x.Slug == slug_news, cancellationToken);
        if (n == null) return NotFound("Новость не найдена");
        if (n.IsActive == false) return BadRequest("Новость уже неактивна.");
        n.IsActive = false;
        n.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Новость помечена как неактивная.");
    }
}

[ApiController]
[Route("api")]
public class NewsController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public NewsController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все новости (только активные).
    /// </summary>
    [SwaggerOperation(Summary = "Получить все новости", Description = "Выгружает все новости")]
    [HttpGet("users/news")]
    public async Task<IActionResult> Read_All_news(CancellationToken cancellationToken)
    {
        var list = await _dbContext.News
            .Where(x => x.IsActive == true)
            .Include(x => x.Author)
            .ToListAsync(cancellationToken);
        return Ok(list);
    }

    
}
