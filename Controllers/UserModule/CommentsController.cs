using eep_backend;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using eep_backend.Models.UserModuleModels;
using Microsoft.EntityFrameworkCore;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class CommentsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public CommentsAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создать комментарий к новости.
    /// </summary>
    [SwaggerOperation(
        Summary = "Создать комментарий к новости",
        Description = "Создает новый комментарий к новости"
    )]
    [HttpPost("comments")]
    public async Task<IActionResult> Create_comment_news([FromBody] Comment comment, CancellationToken cancellationToken)
    {
        if (comment.NewsId == null || comment.UserId == null || string.IsNullOrWhiteSpace(comment.Text))
            return BadRequest("newsId, userId и text обязательны.");

        comment.Id = 0;
        comment.IsActive = true;
        comment.CreatedAt = DateTime.UtcNow;
        comment.UpdatedAt = DateTime.UtcNow;
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Read_comment_news), new { id_comment = comment.Id }, comment);
    }
    
    /// <summary>
    /// Получить конкретный комментарий по id.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить конкретный комментарий по id",
        Description = "Выгружает комментарий по id"
    )]
    [HttpGet("admin_panel/comments/{id_comment}")]
    public async Task<IActionResult> Read_comment_news(int id_comment, CancellationToken cancellationToken)
    {
        var c = await _dbContext.Comments
            .FirstOrDefaultAsync(x => x.Id == id_comment && x.IsActive == true, cancellationToken);
        if (c == null) return NotFound("Комментарий не найден");
        return Ok(c);
    }

    /// <summary>
    /// Частично обновить комментарий.
    /// </summary>
    [SwaggerOperation(
        Summary = "Частично обновить комментарий",
        Description = "Частично обновляет комментарий"
    )]
    [HttpPatch("comments/{id_comment}")]
    public async Task<IActionResult> Edit_comment_news(int id_comment, [FromBody] Comment patch, CancellationToken cancellationToken)
    {
        var c = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id_comment && x.IsActive == true, cancellationToken);
        if (c == null) return NotFound("Комментарий не найден");

        if (patch.NewsId != null) c.NewsId = patch.NewsId;
        if (patch.UserId != null) c.UserId = patch.UserId;
        if (!string.IsNullOrWhiteSpace(patch.Text)) c.Text = patch.Text;

        c.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(c);
    }

    /// <summary>
    /// Полностью заменить комментарий.
    /// </summary>
    [SwaggerOperation(
        Summary = "Полностью заменить комментарий",
        Description = "Полностью заменяет комментарий"
    )]
    [HttpPut("comments/{id_comment}")]
    public async Task<IActionResult> Replace_comment_news(int id_comment, [FromBody] Comment comment, CancellationToken cancellationToken)
    {
        var c = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id_comment && x.IsActive == true, cancellationToken);
        if (c == null) return NotFound("Комментарий не найден");

        c.NewsId = comment.NewsId;
        c.UserId = comment.UserId;
        c.Text = comment.Text;
        c.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(c);
    }

    /// <summary>
    /// Удалить комментарий.
    /// </summary>
    [SwaggerOperation(
        Summary = "Удалить комментарий",
        Description = "Удаляет комментарий"
    )]
    [HttpDelete("comments/{id_comment}")]
    public async Task<IActionResult> Delete_comment_news(int id_comment, CancellationToken cancellationToken)
    {
        var c = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id_comment && x.IsActive == true, cancellationToken);
        if (c == null) return NotFound("Комментарий не найден");
        c.IsActive = false;
        c.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Комментарий помечен как неактивный.");
    }
}

[ApiController]
[Route("api")]
public class CommentsController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public CommentsController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все комментарии.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить все комментарии",
        Description = "Выгружает все комментарии"
    )]
    [HttpGet("admin_panel/comments")]
    public async Task<IActionResult> Read_All_comment_news(CancellationToken cancellationToken)
    {
        var list = await _dbContext.Comments
            .Where(x => x.IsActive == true)
            .ToListAsync(cancellationToken);
        return Ok(list);
    }

    
}
