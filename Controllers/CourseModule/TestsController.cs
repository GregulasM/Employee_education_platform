using eep_backend;
using eep_backend.Models.CourseModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class TestsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public TestsAdminController(SiteDbContext dbContext) { _dbContext = dbContext; }

    /// <summary>
    /// Метод для создания теста.
    /// </summary>
    [SwaggerOperation(Summary = "Метод для создания теста.", Description = "Создает тест.")]
    [HttpPost("courses/{name_course}/tests")]
    public async Task<IActionResult> Create_test([FromRoute] string name_course, [FromBody] TestCreateDto dto, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null)
            return NotFound($"Курс '{name_course}' не найден.");

        Module? module = null;
        if (dto.ModuleId.HasValue)
        {
            module = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == dto.ModuleId.Value && m.IsActive == true, cancellationToken);
            if (module == null)
                return NotFound($"Модуль с Id {dto.ModuleId} не найден.");
        }

        var test = new Test
        {
            Title = dto.Title,
            Description = dto.Description,
            CourseId = course.Id,
            ModuleId = module?.Id,
            Questions = dto.Questions,
            IsActive = true
        };

        _dbContext.Tests.Add(test);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var testDto = new TestDto
        {
            Id = test.Id,
            PublicId = test.PublicId.ToString(),
            Title = test.Title,
            Description = test.Description,
            ModuleId = test.ModuleId,
            ModuleTitle = module?.Title,
            CourseId = test.CourseId,
            CourseTitle = course.Title,
            Questions = test.Questions,
            IsActive = test.IsActive
        };

        return CreatedAtAction(nameof(Read_test), new { name_course = course.Title, name_test = test.Title }, testDto);
    }

    /// <summary>
    /// Метод для выгрузки только одного теста.
    /// </summary>
    [SwaggerOperation(Summary = "Метод для выгрузки только одного теста.", Description = "Выгружает только один тест.")]
    [HttpGet("courses/{name_course}/tests/{name_test}")]
    public async Task<IActionResult> Read_test([FromRoute] string name_course, [FromRoute] string name_test, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound($"Курс '{name_course}' не найден.");
        var test = await _dbContext.Tests
            .Include(t => t.Module)
            .FirstOrDefaultAsync(t => t.Title == name_test && t.CourseId == course.Id && t.IsActive == true, cancellationToken);
        if (test == null) return NotFound($"Тест '{name_test}' не найден.");

        var testDto = new TestDto
        {
            Id = test.Id,
            PublicId = test.PublicId.ToString(),
            Title = test.Title,
            Description = test.Description,
            ModuleId = test.ModuleId,
            ModuleTitle = test.Module?.Title,
            CourseId = test.CourseId,
            CourseTitle = course.Title,
            Questions = test.Questions,
            IsActive = test.IsActive
        };
        return Ok(testDto);
    }

    /// <summary>
    /// Метод для частичного изменения теста.
    /// </summary>
    [SwaggerOperation(Summary = "Метод для частичного изменения теста.", Description = "Частично меняет тест.")]
    [HttpPatch("courses/{name_course}/tests/{name_test}")]
    public async Task<IActionResult> Edit_test(
        [FromRoute] string name_course,
        [FromRoute] string name_test,
        [FromBody] TestPatchDto dto,
        CancellationToken cancellationToken)
    {

        var currentCourse = await _dbContext.Courses.FirstOrDefaultAsync(
            c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (currentCourse == null)
            return NotFound($"Курс '{name_course}' не найден.");

        var test = await _dbContext.Tests
            .Include(t => t.Module)
            .FirstOrDefaultAsync(
                t => t.Title == name_test && t.CourseId == currentCourse.Id && t.IsActive == true,
                cancellationToken);

        if (test == null)
            return NotFound($"Тест '{name_test}' не найден.");
        
        if (dto.Title != null) test.Title = dto.Title;
        if (dto.Description != null) test.Description = dto.Description;
        if (dto.Questions != null) test.Questions = dto.Questions;
        if (dto.CourseId.HasValue)
        {
            var newCourse = await _dbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == dto.CourseId && c.IsActive == true, cancellationToken);
            if (newCourse == null)
                return BadRequest($"Курс с Id '{dto.CourseId}' не найден.");
            test.CourseId = newCourse.Id;
        }
        if (dto.ModuleId.HasValue) test.ModuleId = dto.ModuleId;
        if (dto.IsActive.HasValue) test.IsActive = dto.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        var module = test.ModuleId.HasValue
            ? await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == test.ModuleId, cancellationToken)
            : null;
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == test.CourseId, cancellationToken);

        var testDto = new TestDto
        {
            Id = test.Id,
            PublicId = test.PublicId.ToString(),
            Title = test.Title,
            Description = test.Description,
            ModuleId = test.ModuleId,
            ModuleTitle = module?.Title,
            CourseId = test.CourseId,
            CourseTitle = course?.Title,
            Questions = test.Questions,
            IsActive = test.IsActive
        };
        return Ok(testDto);
    }
    
    [HttpPatch("tests/{id}")]
    public async Task<IActionResult> Edit_test_by_id([FromRoute] int id, [FromBody] TestPatchDto dto, CancellationToken cancellationToken)
    {
        var test = await _dbContext.Tests.Include(t => t.Module).FirstOrDefaultAsync(t => t.Id == id && t.IsActive == true, cancellationToken);
        if (test == null)
            return NotFound($"Тест с id '{id}' не найден.");

        // Меняем любые поля
        if (dto.Title != null) test.Title = dto.Title;
        if (dto.Description != null) test.Description = dto.Description;
        if (dto.Questions != null) test.Questions = dto.Questions;
        if (dto.CourseId.HasValue)
        {
            var newCourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == dto.CourseId && c.IsActive == true, cancellationToken);
            if (newCourse == null)
                return BadRequest($"Курс с Id '{dto.CourseId}' не найден.");
            test.CourseId = newCourse.Id;
        }
        if (dto.ModuleId.HasValue) test.ModuleId = dto.ModuleId;
        if (dto.IsActive.HasValue) test.IsActive = dto.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var module = test.ModuleId.HasValue
            ? await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == test.ModuleId, cancellationToken)
            : null;
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == test.CourseId, cancellationToken);

        var testDto = new TestDto
        {
            Id = test.Id,
            PublicId = test.PublicId.ToString(),
            Title = test.Title,
            Description = test.Description,
            ModuleId = test.ModuleId,
            ModuleTitle = module?.Title,
            CourseId = test.CourseId,
            CourseTitle = course?.Title,
            Questions = test.Questions,
            IsActive = test.IsActive
        };
        return Ok(testDto);
    }


    /// <summary>
    /// Метод для полного изменения теста.  
    /// </summary>
    [SwaggerOperation(Summary = "Метод для полного изменения теста.", Description = "Полностью меняет тест.")]
    [HttpPut("courses/{name_course}/tests/{name_test}")]
    public async Task<IActionResult> Replace_test([FromRoute] string name_course, [FromRoute] string name_test, [FromBody] TestCreateDto dto, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound($"Курс '{name_course}' не найден.");

        var test = await _dbContext.Tests.Include(t => t.Module).FirstOrDefaultAsync(t => t.Title == name_test && t.CourseId == course.Id && t.IsActive == true, cancellationToken);
        if (test == null) return NotFound($"Тест '{name_test}' не найден.");

        test.Title = dto.Title;
        test.Description = dto.Description;
        test.CourseId = course.Id;
        test.ModuleId = dto.ModuleId;
        test.Questions = dto.Questions;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var module = test.ModuleId.HasValue
            ? await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == test.ModuleId, cancellationToken)
            : null;

        var testDto = new TestDto
        {
            Id = test.Id,
            PublicId = test.PublicId.ToString(),
            Title = test.Title,
            Description = test.Description,
            ModuleId = test.ModuleId,
            ModuleTitle = module?.Title,
            CourseId = test.CourseId,
            CourseTitle = course.Title,
            Questions = test.Questions,
            IsActive = test.IsActive
        };

        return Ok(testDto);
    }

    /// <summary>
    /// Метод для удаления теста (soft-delete).
    /// </summary>
    [SwaggerOperation(Summary = "Метод для удаления теста.", Description = "Удаляет тест (soft-delete).")]
    [HttpDelete("courses/{name_course}/tests/{name_test}")]
    public async Task<IActionResult> Delete_test([FromRoute] string name_course, [FromRoute] string name_test, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound($"Курс '{name_course}' не найден.");

        var test = await _dbContext.Tests.FirstOrDefaultAsync(t => t.Title == name_test && t.CourseId == course.Id && t.IsActive == true, cancellationToken);
        if (test == null) return NotFound($"Тест '{name_test}' не найден.");

        test.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Тест успешно помечен как неактивный.");
    }

    /// <summary>
    /// Метод для выгрузки всех тестов (admin-panel).
    /// </summary>
    [SwaggerOperation(Summary = "Метод для выгрузки всех тестов (admin-panel).", Description = "Выгружает все тесты.")]
    [HttpGet("tests")]
    public async Task<IActionResult> Read_All_tests(CancellationToken cancellationToken)
    {
        var tests = await _dbContext.Tests
            .Where(t => t.IsActive == true)
            .Include(t => t.Course)
            .Include(t => t.Module)
            .ToListAsync(cancellationToken);

        var dto = tests.Select(t => new TestDto
        {
            Id = t.Id,
            PublicId = t.PublicId.ToString(),
            Title = t.Title,
            Description = t.Description,
            ModuleId = t.ModuleId,
            ModuleTitle = t.Module?.Title,
            CourseId = t.CourseId,
            CourseTitle = t.Course?.Title,
            Questions = t.Questions,
            IsActive = t.IsActive
        }).ToList();

        return Ok(dto);
    }
}

[ApiController]
[Route("api")]
public class TestsController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public TestsController(SiteDbContext dbContext) { _dbContext = dbContext; }

    /// <summary>
    /// Метод для выгрузки всех тестов по курсу.
    /// </summary>
    [SwaggerOperation(Summary = "Метод для выгрузки всех тестов.", Description = "Выгружает все тесты курса.")]
    [HttpGet("courses/{name_course}/tests")]
    public async Task<IActionResult> Read_All_test([FromRoute] string name_course, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (course == null) return NotFound($"Курс '{name_course}' не найден.");
        var tests = await _dbContext.Tests
            .Where(t => t.CourseId == course.Id && t.IsActive == true)
            .Include(t => t.Module)
            .ToListAsync(cancellationToken);

        var dto = tests.Select(t => new TestDto
        {
            Id = t.Id,
            PublicId = t.PublicId.ToString(),
            Title = t.Title,
            Description = t.Description,
            ModuleId = t.ModuleId,
            ModuleTitle = t.Module?.Title,
            CourseId = t.CourseId,
            CourseTitle = course.Title,
            Questions = t.Questions,
            IsActive = t.IsActive
        }).ToList();

        return Ok(dto);
    }
}
