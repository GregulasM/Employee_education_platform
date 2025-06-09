using eep_backend;
using eep_backend.Models.CourseModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;


[ApiController]
[Route("api/admin_panel")]
public class ArticlesAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public ArticlesAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Метод для создания статьи внутри модуля.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания статьи внутри модуля.", 
        Description = "Создает статью внутри модуля."
    )]
    [HttpPost("modules/{moduleId}/articles")]
    public async Task<IActionResult> Create_article(int moduleId, [FromBody] Article article, CancellationToken cancellationToken)
    {
        var module = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == moduleId && (m.IsActive ?? true), cancellationToken);
        if (module == null) return BadRequest("Модуль не найден");

        article.ModuleId = module.Id;
        article.CreatedAt = DateTime.UtcNow;
        article.IsActive = true;

        _dbContext.Articles.Add(article);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Read_article), new { moduleId, articleTitle = article.Title }, article);
    }
    
    /// <summary>
    /// Метод для просмотра только одной статьи внутри модуля.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для просмотра только одной статьи внутри модуля.", 
        Description = "Просмотр только одной статьи, привязанной к модулю."
    )]
    [HttpGet("modules/{moduleId}/articles/{articleTitle}")]
    public async Task<IActionResult> Read_article(int moduleId, string articleTitle, CancellationToken cancellationToken)
    {
        var article = await _dbContext.Articles
            .Where(a => a.ModuleId == moduleId && a.Title == articleTitle && (a.IsActive ?? true))
            .Include(a => a.Module)
            .FirstOrDefaultAsync(cancellationToken);
        if (article == null) return NotFound();

        var dto = new ArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Image = article.Image,
            Tags = article.Tags,
            Content = article.Content,
            Rating = article.Rating,
            ModuleId = article.ModuleId,
            ModuleTitle = article.Module?.Title,
            CreatedAt = article.CreatedAt += TimeSpan.FromHours(10),
            UpdatedAt = article.UpdatedAt += TimeSpan.FromHours(10),
            IsActive = article.IsActive
        };
        return Ok(dto);
    }
    
    
    
    /// <summary>
    /// Метод для изменения деталей статьи по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для изменения деталей статьи по уникальному названию.", 
        Description = "Вводим название статьи и изменяем его части."
    )]
    [HttpPatch("modules/{moduleId}/articles/{articleTitle}")]
    public async Task<IActionResult> Edit_article(int moduleId, string articleTitle, [FromBody] Article patch, CancellationToken cancellationToken)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.ModuleId == moduleId && a.Title == articleTitle && (a.IsActive ?? true), cancellationToken);
        if (article == null) return NotFound("Статья не найдена");

        if (!string.IsNullOrWhiteSpace(patch.Title)) article.Title = patch.Title;
        if (!string.IsNullOrWhiteSpace(patch.Image)) article.Image = patch.Image;
        if (!string.IsNullOrWhiteSpace(patch.Tags)) article.Tags = patch.Tags;
        if (!string.IsNullOrWhiteSpace(patch.Content)) article.Content = patch.Content;
        if (patch.Rating.HasValue) article.Rating = patch.Rating;
        if (patch.ModuleId.HasValue && patch.ModuleId.Value != article.ModuleId)
        {
            var newModule = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == patch.ModuleId.Value && (m.IsActive ?? true), cancellationToken);
            if (newModule == null) return BadRequest("Новый модуль не найден");
            article.ModuleId = newModule.Id;
        }
        article.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(article);
    }
    
    /// <summary>
    /// Метод для полного изменения статьи по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения статьи по уникальному названию.", 
        Description = "Вводим название статьи и полностью меняем его."
    )]
    [HttpPut("modules/{moduleId}/articles/{articleTitle}")]
    public async Task<IActionResult> Replace_article(int moduleId, string articleTitle, [FromBody] Article newArticle, CancellationToken cancellationToken)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.ModuleId == moduleId && a.Title == articleTitle && (a.IsActive ?? true), cancellationToken);
        if (article == null) return NotFound("Статья не найдена");

        article.Title = newArticle.Title;
        article.Image = newArticle.Image;
        article.Tags = newArticle.Tags;
        article.Content = newArticle.Content;
        article.Rating = newArticle.Rating;
        article.ModuleId = newArticle.ModuleId;
        article.UpdatedAt = DateTime.UtcNow;
        article.IsActive = newArticle.IsActive;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(article);
    }
    
    /// <summary>
    /// Метод для удаления статьи по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления статьи по уникальному названию.", 
        Description = "Вводим название статьи и удаляем ее."
    )]
    [HttpDelete("modules/{moduleId}/articles/{articleTitle}")]
    public async Task<IActionResult> Delete_article(int moduleId, string articleTitle, CancellationToken cancellationToken)
    {
        var article = await _dbContext.Articles
            .FirstOrDefaultAsync(a => a.ModuleId == moduleId && a.Title == articleTitle && (a.IsActive ?? true), cancellationToken);
        if (article == null) return NotFound("Статья не найдена");
        if (article.IsActive == false) return BadRequest("Статья уже неактивна.");

        article.IsActive = false;
        article.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Статья успешно помечена как неактивная.");
    }
}

[Route("api")]
public class ArticlesController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public ArticlesController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    /// <summary>
    /// Метод для просмотра всех статей внутри модуля.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для просмотра всех статей внутри модуля.", 
        Description = "Просмотр всех статей модуля, привязанного к курсу."
    )]
    [HttpGet("modules/{moduleId}/articles")]
    public async Task<IActionResult> Read_All_article(int moduleId, CancellationToken cancellationToken)
    {
        var articles = await _dbContext.Articles
            .Where(a => a.ModuleId == moduleId && (a.IsActive ?? true))
            .Include(a => a.Module)
            .ToListAsync(cancellationToken);

        var dtoList = articles.Select(article => new ArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Image = article.Image,
            Tags = article.Tags,
            Content = article.Content,
            Rating = article.Rating,
            ModuleId = article.ModuleId,
            ModuleTitle = article.Module?.Title,
            CreatedAt = article.CreatedAt += TimeSpan.FromHours(10),
            UpdatedAt = article.UpdatedAt += TimeSpan.FromHours(10),
            IsActive = article.IsActive
        }).ToList();

        return Ok(dtoList);
    }
    
    
}