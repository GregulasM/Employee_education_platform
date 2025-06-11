using eep_backend;
using eep_backend.Models.CourseModuleModels;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class TestResultsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public TestResultsAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создать результат прохождения теста.
    /// </summary>
    [SwaggerOperation(Summary = "Создать результат теста", Description = "Создает новый результат теста для пользователя")]
    [HttpPost("test_results")]
    public async Task<IActionResult> Create_test_result([FromBody] TestResultDto dto, CancellationToken cancellationToken)
    {
        if (dto.UserId == null || dto.TestId == null)
            return BadRequest("UserId и TestId обязательны");

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId, cancellationToken);
        var test = await _dbContext.Tests.FirstOrDefaultAsync(t => t.Id == dto.TestId, cancellationToken);
        if (user == null) return BadRequest($"Пользователь с id '{dto.UserId}' не найден.");
        if (test == null) return BadRequest($"Тест с id '{dto.TestId}' не найден.");

        var testResult = new TestResult
        {
            UserId = dto.UserId,
            TestId = dto.TestId,
            Score = dto.Score,
            MaxScore = dto.MaxScore,
            Answers = dto.Answers,
            Date = DateTime.UtcNow,
            Status = dto.Status,
            IsActive = true
        };

        _dbContext.TestResults.Add(testResult);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Read_test_results), new { id = testResult.Id }, testResult);
    }
    
    /// <summary>
    /// Получить один результат теста по id.
    /// </summary>
    [SwaggerOperation(Summary = "Получить результат теста по id", Description = "Выгружает конкретную попытку теста пользователя")]
    [HttpGet("test_results/{id}")]
    public async Task<IActionResult> Read_test_results([FromRoute] int id, CancellationToken cancellationToken)
    {
        var r = await _dbContext.TestResults
            .Include(r => r.User)
            .Include(r => r.Test)
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive == true, cancellationToken);
        if (r == null) return NotFound($"Результат с id '{id}' не найден.");

        var dto = new {
            r.Id,
            r.PublicId,
            UserId = r.UserId,
            User = r.User == null ? null : new { r.User.Id, r.User.Login, r.User.FirstName, r.User.LastName },
            TestId = r.TestId,
            Test = r.Test == null ? null : new { r.Test.Id, r.Test.Title },
            r.Score,
            r.MaxScore,
            r.Answers,
            r.Date,
            r.Status
        };
        return Ok(dto);
    }

    /// <summary>
    /// Частичное обновление результата теста.
    /// </summary>
    [SwaggerOperation(Summary = "Частичное обновление результата теста", Description = "Частично меняет результат теста")]
    [HttpPatch("test_results/{id}")]
    public async Task<IActionResult> Edit_test_result([FromRoute] int id, [FromBody] TestResultDto dto, CancellationToken cancellationToken)
    {
        var result = await _dbContext.TestResults.FirstOrDefaultAsync(r => r.Id == id && r.IsActive == true, cancellationToken);
        if (result == null)
            return NotFound($"Результат с id '{id}' не найден.");

        if (dto.UserId.HasValue) result.UserId = dto.UserId;
        if (dto.TestId.HasValue) result.TestId = dto.TestId;
        if (dto.Score.HasValue) result.Score = dto.Score;
        if (dto.MaxScore.HasValue) result.MaxScore = dto.MaxScore;
        if (dto.Answers != null) result.Answers = dto.Answers;
        if (dto.Status != null) result.Status = dto.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Полное обновление результата теста.
    /// </summary>
    [SwaggerOperation(Summary = "Полное обновление результата теста", Description = "Полностью заменяет результат теста")]
    [HttpPut("test_results/{id}")]
    public async Task<IActionResult> Replace_test_result([FromRoute] int id, [FromBody] TestResultDto dto, CancellationToken cancellationToken)
    {
        var result = await _dbContext.TestResults.FirstOrDefaultAsync(r => r.Id == id && r.IsActive == true, cancellationToken);
        if (result == null)
            return NotFound($"Результат с id '{id}' не найден.");

        result.UserId = dto.UserId;
        result.TestId = dto.TestId;
        result.Score = dto.Score;
        result.MaxScore = dto.MaxScore;
        result.Answers = dto.Answers;
        result.Date = DateTime.UtcNow;
        result.Status = dto.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Удалить результат теста пользователя (soft-delete).
    /// </summary>
    [SwaggerOperation(Summary = "Удалить результат теста", Description = "Удаляет результат теста (soft-delete)")]
    [HttpDelete("test_results/{id}")]
    public async Task<IActionResult> Delete_test_result([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _dbContext.TestResults.FirstOrDefaultAsync(r => r.Id == id && r.IsActive == true, cancellationToken);
        if (result == null)
            return NotFound($"Результат с id '{id}' не найден.");

        result.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Результат теста успешно помечен как неактивный.");
    }
}


[ApiController]
[Route("api")]
public class TestResultsController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public TestResultsController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все результаты тестов (активные).
    /// </summary>
    [SwaggerOperation(Summary = "Получить все результаты тестов", Description = "Выгружает все активные результаты тестов")]
    [HttpGet("admin_panel/test_results")]
    public async Task<IActionResult> Read_All_test_results(CancellationToken cancellationToken)
    {
        var results = await _dbContext.TestResults
            .Where(r => r.IsActive == true)
            .Include(r => r.User)
            .Include(r => r.Test)
            .ToListAsync(cancellationToken);

        // Не передаем поле IsActive!
        var dto = results.Select(r => new {
            r.Id,
            r.PublicId,
            UserId = r.UserId,
            User = r.User == null ? null : new { r.User.Id, r.User.Login, r.User.FirstName, r.User.LastName },
            TestId = r.TestId,
            Test = r.Test == null ? null : new { r.Test.Id, r.Test.Title },
            r.Score,
            r.MaxScore,
            r.Answers,
            r.Date,
            r.Status
        });

        return Ok(dto);
    }

    
}