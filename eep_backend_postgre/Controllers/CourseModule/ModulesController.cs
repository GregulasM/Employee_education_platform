using eep_backend;
using eep_backend.Models.CourseModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class ModulesAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public ModulesAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Метод для создания модуля внутри курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания модуля внутри курса.", 
        Description = "Создает модуль, привязанный к курсу."
    )]
    [HttpPost("courses/{name_course}/modules")]
    public async Task<IActionResult> Create_module(string name_course, [FromBody] ModuleCreateDto dto, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return BadRequest("Курс не найден");
        var module = new Module
        {
            Title = dto.Title,
            Description = dto.Description,
            Image = dto.Image,
            Tags = dto.Tags ?? "[]",
            Order = dto.Order,
            CreatedAt = DateTime.UtcNow,
            CourseId = course.Id,
            IsActive = true,
        };
        _dbContext.Modules.Add(module);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Выгружаем DTO
        var moduleDto = new ModuleDto
        {
            Id = module.Id,
            Title = module.Title,
            Description = module.Description,
            Image = module.Image,
            Tags = module.Tags,
            Order = module.Order,
            CourseId = module.CourseId,
            CourseTitle = course.Title,
            CreatedAt = module.CreatedAt ,
            UpdatedAt = module.UpdatedAt ,
            IsActive = module.IsActive
        };

        return CreatedAtAction(nameof(Read_module), new { name_course, name_module = module.Title }, moduleDto);
    }
    
    /// <summary>
    /// Метод для чтения только одного модуля внутри курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для чтения только одного модуля внутри курса.", 
        Description = "Просматривает только один модуль, привязанный к курсу."
    )]
    [HttpGet("courses/{name_course}/modules/{name_module}")]
    public async Task<IActionResult> Read_module(string name_course, string name_module, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return NotFound();
        var module = await _dbContext.Modules
            .Where(m => m.Title == name_module && m.CourseId == course.Id && (m.IsActive ?? true))
            .FirstOrDefaultAsync(cancellationToken);
        if (module == null) return NotFound();

        var moduleDto = new ModuleDto
        {
            Id = module.Id,
            Title = module.Title,
            Description = module.Description,
            Image = module.Image,
            Tags = module.Tags,
            Order = module.Order,
            CourseId = module.CourseId,
            CourseTitle = course.Title,
            CreatedAt = module.CreatedAt += TimeSpan.FromHours(10),
            UpdatedAt = module.UpdatedAt += TimeSpan.FromHours(10),
            IsActive = module.IsActive
        };

        return Ok(moduleDto);
    }
    

    /// <summary>
    /// Метод для изменения деталей модуля по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для изменения деталей модуля по уникальному названию.", 
        Description = "Вводим название модуля и изменяем его части."
    )]
    [HttpPatch("courses/{name_course}/modules/{name_module}")]
    public async Task<IActionResult> Edit_module(string name_course, string name_module, [FromBody] ModulePatchDto patchDto, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return NotFound("Курс не найден");
        var module = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Title == name_module && m.CourseId == course.Id && (m.IsActive ?? true), cancellationToken);
        if (module == null) return NotFound("Модуль не найден");

        // Новое! Меняем курс у модуля, если пришёл другой CourseId
        if (patchDto.CourseId.HasValue && patchDto.CourseId.Value != module.CourseId)
        {
            // Проверяем существует ли такой курс
            var newCourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == patchDto.CourseId.Value && (c.IsActive ?? true), cancellationToken);
            if (newCourse == null) return BadRequest("Новый курс не найден");
            module.CourseId = patchDto.CourseId.Value;
        }

        if (!string.IsNullOrWhiteSpace(patchDto.Title)) module.Title = patchDto.Title;
        if (!string.IsNullOrWhiteSpace(patchDto.Description)) module.Description = patchDto.Description;
        if (!string.IsNullOrWhiteSpace(patchDto.Image)) module.Image = patchDto.Image;
        if (!string.IsNullOrWhiteSpace(patchDto.Tags)) module.Tags = patchDto.Tags;
        if (patchDto.Order.HasValue) module.Order = patchDto.Order;
        if (patchDto.IsActive.HasValue) module.IsActive = patchDto.IsActive;

        module.UpdatedAt = DateTime.UtcNow ;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var moduleDto = new ModuleDto
        {
            Id = module.Id,
            Title = module.Title,
            Description = module.Description,
            Image = module.Image,
            Tags = module.Tags,
            Order = module.Order,
            CourseId = module.CourseId,
            CourseTitle = course.Title,
            IsActive = module.IsActive,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };

        return Ok(moduleDto);
    }

    /// <summary>
    /// Метод для полного изменения модуля по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения модуля по уникальному названию.", 
        Description = "Вводим название модуля и полностью меняем его."
    )]
    [HttpPut("courses/{name_course}/modules/{name_module}")]
    public async Task<IActionResult> Replace_module(string name_course, string name_module, [FromBody] Module fullModule, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return NotFound("Курс не найден");
        var module = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Title == name_module && m.CourseId == course.Id && (m.IsActive ?? true), cancellationToken);
        if (module == null) return NotFound("Модуль не найден");

        module.Title = fullModule.Title;
        module.Description = fullModule.Description;
        module.Image = fullModule.Image;
        module.Tags = fullModule.Tags;
        module.Order = fullModule.Order;
        module.IsActive = fullModule.IsActive;
        module.UpdatedAt = DateTime.UtcNow ;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var moduleDto = new ModuleDto
        {
            Id = module.Id,
            Title = module.Title,
            Description = module.Description,
            Image = module.Image,
            Tags = module.Tags,
            Order = module.Order,
            CourseId = module.CourseId,
            CourseTitle = course.Title,
            IsActive = module.IsActive,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };

        return Ok(moduleDto);
    }

    /// <summary>
    /// Метод для удаления модуля по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления модуля по уникальному названию.", 
        Description = "Вводим название модуля и удаляем его."
    )]
    [HttpDelete("courses/{name_course}/modules/{name_module}")]
    public async Task<IActionResult> Delete_module(string name_course, string name_module, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == name_course && (c.IsActive ?? true), cancellationToken);
        if (course == null) return NotFound("Курс не найден");
        var module = await _dbContext.Modules.FirstOrDefaultAsync(m => m.Title == name_module && m.CourseId == course.Id && (m.IsActive ?? true), cancellationToken);
        if (module == null) return NotFound("Модуль не найден");
        module.IsActive = false;
        module.UpdatedAt = DateTime.UtcNow ;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Модуль успешно помечен как неактивный.");
    }
}

[ApiController]
[Route("api")]
public class ModulesController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    public ModulesController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Метод для чтения всех модулей внутри курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для чтения всех модулей внутри курса.", 
        Description = "Просматривает все модули, привязанный к курсу."
    )]
    [HttpGet("modules")]
    public async Task<IActionResult> Read_All_module(CancellationToken cancellationToken)
    {
        var modules = await _dbContext.Modules
            .Where(m => m.IsActive ?? true)
            .Include(m => m.Course)
            //.Include(m => m.Tests.Where(t => t.IsActive ?? true)) // НЕ включай Tests если они не нужны для выдачи, иначе цикл снова появится!
            .ToListAsync(cancellationToken);

        var dto = modules.Select(m => new ModuleDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            Image = m.Image,
            Tags = m.Tags,
            Order = m.Order,
            CourseId = m.CourseId,
            CourseTitle = m.Course?.Title,
            CreatedAt = m.CreatedAt += TimeSpan.FromHours(10),
            UpdatedAt = m.UpdatedAt += TimeSpan.FromHours(10),
            IsActive = m.IsActive
        }).ToList();

        return Ok(dto);
    }

    
}