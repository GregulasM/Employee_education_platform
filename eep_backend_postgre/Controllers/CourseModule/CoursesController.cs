using eep_backend;
using eep_backend.Models.CourseModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class CoursesAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public CoursesAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Метод для создания курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания курса.",
        Description = "Создает новый курс."
    )]
    [HttpPost("courses")]
    public async Task<IActionResult> Create_course([FromBody] CourseCreateDto courseCreateDto, CancellationToken cancellationToken)
    {
        if (courseCreateDto == null || string.IsNullOrWhiteSpace(courseCreateDto.Title))
            return BadRequest("Некорректные данные курса.");

        var course = new Course
        {
            Title = courseCreateDto.Title,
            Description = courseCreateDto.Description,
            Image = courseCreateDto.Image,
            Tags = courseCreateDto.Tags ?? "[]",
            Position = courseCreateDto.Position,
            DepartmentId = courseCreateDto.DepartmentId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Read_course), new { name_course = course.Title }, course);
    }
    
    /// <summary>
    /// Метод для получения только одного курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для получения только одного курса.",
        Description = "Получает только один активный курс с деталями."
    )]
    [HttpGet("courses/{name_course}")]
    public async Task<IActionResult> Read_course(string name_course, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses
            .Where(c => c.Title == name_course && (c.IsActive ?? true))
            .Include(c => c.Department)
            .Include(c => c.UsefulLinks.Where(l => l.IsActive ?? true))
            .Include(c => c.Tests.Where(t => t.IsActive ?? true))
            .FirstOrDefaultAsync(cancellationToken);

        if (course == null) return NotFound();

        return Ok(course);
    }

    /// <summary>
    /// Метод для изменения деталей курса по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для изменения деталей курса по уникальному названию.",
        Description = "Вводим название курса и изменяем его части."
    )]
    [HttpPatch("courses/{name_course}")]
    public async Task<IActionResult> Edit_course(string name_course, [FromBody] CoursePatchDto patchDto, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(patchDto.Title)) course.Title = patchDto.Title;
        if (!string.IsNullOrWhiteSpace(patchDto.Description)) course.Description = patchDto.Description;
        if (!string.IsNullOrWhiteSpace(patchDto.Image)) course.Image = patchDto.Image;
        if (!string.IsNullOrWhiteSpace(patchDto.Tags)) course.Tags = patchDto.Tags;
        if (patchDto.Position.HasValue) course.Position = patchDto.Position;
        if (patchDto.DepartmentId.HasValue) course.DepartmentId = patchDto.DepartmentId;
        if (patchDto.IsActive.HasValue) course.IsActive = patchDto.IsActive;

        course.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(course);
    }

    /// <summary>
    /// Метод для полного изменения курса по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения курса по уникальному названию.",
        Description = "Вводим название курса и изменяем все его детали."
    )]
    [HttpPut("courses/{name_course}")]
    public async Task<IActionResult> Replace_course(string name_course, [FromBody] Course fullCourse, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return NotFound();

        course.Title = fullCourse.Title;
        course.Description = fullCourse.Description;
        course.Image = fullCourse.Image;
        course.Tags = fullCourse.Tags;
        course.Position = fullCourse.Position;
        course.DepartmentId = fullCourse.DepartmentId;
        course.UpdatedAt = DateTime.UtcNow;
        course.IsActive = fullCourse.IsActive;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(course);
    }

    /// <summary>
    /// Метод для удаления курса по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления курса по уникальному названию.",
        Description = "Вводим название курса и удаляем его (мягкое удаление)."
    )]
    [HttpDelete("courses/{name_course}")]
    public async Task<IActionResult> Delete_course(string name_course, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return NotFound();

        course.IsActive = false;
        course.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Курс успешно помечен как неактивный.");
    }
}

[ApiController]
[Route("api")]
public class CoursesController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public CoursesController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Метод для получения всех курсов.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для получения всех курсов.",
        Description = "Получает все активные курсы, которые есть в БД."
    )]
    [HttpGet("courses")]
    public async Task<IActionResult> Read_All_course(CancellationToken cancellationToken)
    {
        var courses = await _dbContext.Courses
            .Where(c => c.IsActive ?? true)
            .Include(c => c.Department)
            .Include(c => c.UsefulLinks.Where(l => l.IsActive ?? true))
            .Include(c => c.Tests.Where(t => t.IsActive ?? true))
            .ThenInclude(t => t.Module)
            .ToListAsync(cancellationToken);

        var dto = courses.Select(c => new CourseDto
        {
            Id = c.Id,
            PublicId = c.PublicId.ToString(),
            Title = c.Title,
            Description = c.Description,
            Image = c.Image,
            Tags = c.Tags,
            Position = c.Position,
            DepartmentId = c.DepartmentId,
            DepartmentName = c.Department?.Name,
            CreatedAt = c.CreatedAt += TimeSpan.FromHours(10), 
            UpdatedAt = c.UpdatedAt += TimeSpan.FromHours(10), 
            UsefulLinks = c.UsefulLinks?.Where(l => l.IsActive ?? true).Select(l => new LinkDto
            {
                Id = l.Id,
                Title = l.Title,
                Url = l.Url
            }).ToList() ?? new List<LinkDto>(),
            Tests = c.Tests
                .Where(t => t.IsActive ?? true)
                .Select(t => new TestDto
                {
                    Id = t.Id,
                    PublicId = t.PublicId.ToString(),
                    Title = t.Title,
                    Description = t.Description,
                    ModuleId = t.ModuleId,
                    ModuleTitle = t.Module?.Title,
                    Questions = t.Questions
                })
                .ToList()
        }).ToList();

        return Ok(dto);
    }

    
}