using System.Text.Json;
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
        var currentCourse = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Title == name_course && c.IsActive == true, cancellationToken);
        if (currentCourse == null)
            return NotFound($"Курс '{name_course}' не найден.");

        var test = await _dbContext.Tests
            .Include(t => t.Module)
            .FirstOrDefaultAsync(
                t => t.Title == name_test && t.CourseId == currentCourse.Id && t.IsActive == true,
                cancellationToken);

        if (test == null)
            return NotFound($"Тест '{name_test}' не найден.");

        // Меняем основные поля
        if (dto.Title != null) test.Title = dto.Title;
        if (dto.Description != null) test.Description = dto.Description;
        if (dto.Questions != null) test.Questions = dto.Questions;
        if (dto.IsActive.HasValue) test.IsActive = dto.IsActive;

        // 1. Если меняем курс
        if (dto.CourseId.HasValue && dto.CourseId.Value != test.CourseId)
        {
            var newCourse = await _dbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == dto.CourseId && c.IsActive == true, cancellationToken);
            if (newCourse == null)
                return BadRequest($"Курс с Id '{dto.CourseId}' не найден.");

            // Если у теста есть модуль, но он не принадлежит новому курсу — сбрасываем
            if (test.ModuleId.HasValue)
            {
                var module = await _dbContext.Modules
                    .FirstOrDefaultAsync(m => m.Id == test.ModuleId, cancellationToken);
                if (module == null || module.CourseId != newCourse.Id)
                    test.ModuleId = null;
            }
            test.CourseId = newCourse.Id;
        }

        // 2. Если меняем модуль
        if (dto.ModuleId.HasValue)
        {
            if (dto.ModuleId.Value == 0)
            {
                // Позволяем сбросить модуль на null
                test.ModuleId = null;
            }
            else
            {
                var module = await _dbContext.Modules
                    .FirstOrDefaultAsync(m => m.Id == dto.ModuleId, cancellationToken);
                if (module == null)
                    return BadRequest($"Модуль с Id '{dto.ModuleId}' не найден.");
                if (module.CourseId != test.CourseId)
                    return BadRequest($"Модуль с Id '{dto.ModuleId}' не принадлежит курсу с Id '{test.CourseId}'.");
                test.ModuleId = dto.ModuleId;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Вернем DTO
        var moduleNow = test.ModuleId.HasValue
            ? await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == test.ModuleId, cancellationToken)
            : null;
        var courseNow = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == test.CourseId, cancellationToken);

        var testDto = new TestDto
        {
            Id = test.Id,
            PublicId = test.PublicId.ToString(),
            Title = test.Title,
            Description = test.Description,
            ModuleId = test.ModuleId,
            ModuleTitle = moduleNow?.Title,
            CourseId = test.CourseId,
            CourseTitle = courseNow?.Title,
            Questions = test.Questions,
            IsActive = test.IsActive
        };
        return Ok(testDto);
    }
    
   [HttpPatch("tests/{id}")]
public async Task<IActionResult> Edit_test_by_id(
    [FromRoute] int id,
    [FromBody] JsonElement patchDto,
    CancellationToken cancellationToken)
{
    var test = await _dbContext.Tests.Include(t => t.Module)
        .FirstOrDefaultAsync(t => t.Id == id && t.IsActive == true, cancellationToken);
    if (test == null)
        return NotFound($"Тест с id '{id}' не найден.");

    // --- 1. Сначала обработай смену курса (если есть)
    bool courseChanged = false;
    int? newCourseId = null;
    if (patchDto.TryGetProperty("courseId", out var courseIdProp) && courseIdProp.ValueKind == JsonValueKind.Number)
    {
        newCourseId = courseIdProp.GetInt32();
        if (newCourseId != test.CourseId)
        {
            var newCourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == newCourseId && c.IsActive == true, cancellationToken);
            if (newCourse == null)
                return BadRequest($"Курс с Id '{newCourseId}' не найден.");
            // ---- Сбрасываем модуль ПЕРЕД установкой courseId, если модуль не принадлежит курсу
            if (test.ModuleId.HasValue)
            {
                var module = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == test.ModuleId, cancellationToken);
                if (module == null || module.CourseId != newCourseId)
                    test.ModuleId = null;
            }
            test.CourseId = newCourseId.Value;
            courseChanged = true;
        }
    }

    // --- 2. Основные поля
    if (patchDto.TryGetProperty("title", out var titleProp) && titleProp.ValueKind != JsonValueKind.Null)
        test.Title = titleProp.GetString();
    if (patchDto.TryGetProperty("description", out var descProp) && descProp.ValueKind != JsonValueKind.Null)
        test.Description = descProp.GetString();
    if (patchDto.TryGetProperty("questions", out var questionsProp) && questionsProp.ValueKind != JsonValueKind.Null)
        test.Questions = questionsProp.GetString();
    if (patchDto.TryGetProperty("isActive", out var activeProp) && activeProp.ValueKind != JsonValueKind.Null)
        test.IsActive = activeProp.GetBoolean();

    // --- 3. Теперь обработай moduleId
    if (patchDto.TryGetProperty("moduleId", out var moduleIdProp))
    {
        if (moduleIdProp.ValueKind == JsonValueKind.Null)
        {
            test.ModuleId = null;
        }
        else if (moduleIdProp.ValueKind == JsonValueKind.Number)
        {
            var moduleId = moduleIdProp.GetInt32();
            if (moduleId == 0)
            {
                test.ModuleId = null;
            }
            else
            {
                var module = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == moduleId, cancellationToken);
                // Вот тут! Если модуль не принадлежит текущему тесту — просто сбрасываем, но НЕ выкидываем ошибку!
                if (module == null || module.CourseId != test.CourseId)
                {
                    test.ModuleId = null;
                }
                else
                {
                    test.ModuleId = moduleId;
                }
            }
        }
    }

    await _dbContext.SaveChangesAsync(cancellationToken);

    var moduleNow = test.ModuleId.HasValue
        ? await _dbContext.Modules.FirstOrDefaultAsync(m => m.Id == test.ModuleId, cancellationToken)
        : null;
    var courseNow = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == test.CourseId, cancellationToken);

    var testDto = new TestDto
    {
        Id = test.Id,
        PublicId = test.PublicId.ToString(),
        Title = test.Title,
        Description = test.Description,
        ModuleId = test.ModuleId,
        ModuleTitle = moduleNow?.Title,
        CourseId = test.CourseId,
        CourseTitle = courseNow?.Title,
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
