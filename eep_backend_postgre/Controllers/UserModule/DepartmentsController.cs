using eep_backend;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class DepartmentsAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public DepartmentsAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создать отдел.
    /// </summary>
    [SwaggerOperation(Summary = "Создать отдел", Description = "Создает новый отдел")]
    [HttpPost("departments")]
    public async Task<IActionResult> Create_department([FromBody] Department department, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(department.Name))
            return BadRequest("Название обязательно");

        department.Id = 0;
        department.IsActive = true;
        _dbContext.Departments.Add(department);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Read_department), new { id = department.Id }, department);
    }
    
    /// <summary>
    /// Получить все отделы (только активные).
    /// </summary>
    [SwaggerOperation(Summary = "Получить все отделы", Description = "Выгружает все отделы")]
    [HttpGet("departments")]
    public async Task<IActionResult> Read_All_department(CancellationToken cancellationToken)
    {
        var departments = await _dbContext.Departments
            .Where(d => d.IsActive == true)
            .ToListAsync(cancellationToken);
        return Ok(departments);
    }

    /// <summary>
    /// Получить отдел по ID.
    /// </summary>
    [SwaggerOperation(Summary = "Получить отдел по ID", Description = "Выгружает отдел по ID")]
    [HttpGet("departments/{id}")]
    public async Task<IActionResult> Read_department(int id, CancellationToken cancellationToken)
    {
        var dep = await _dbContext.Departments.FindAsync(new object[] { id }, cancellationToken);
        if (dep == null || dep.IsActive == false)
            return NotFound("Отдел не найден");
        return Ok(dep);
    }

    /// <summary>
    /// Частично обновить отдел.
    /// </summary>
    [SwaggerOperation(Summary = "Частично обновить отдел", Description = "Частично обновляет отдел")]
    [HttpPatch("departments/{id}")]
    public async Task<IActionResult> Edit_department(int id, [FromBody] Department patch, CancellationToken cancellationToken)
    {
        var dep = await _dbContext.Departments.FindAsync(new object[] { id }, cancellationToken);
        if (dep == null) return NotFound("Отдел не найден");

        if (!string.IsNullOrWhiteSpace(patch.Name)) dep.Name = patch.Name;
        if (!string.IsNullOrWhiteSpace(patch.Description)) dep.Description = patch.Description;
        if (patch.IsActive.HasValue) dep.IsActive = patch.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(dep);
    }

    /// <summary>
    /// Полностью заменить отдел.
    /// </summary>
    [SwaggerOperation(Summary = "Полностью заменить отдел", Description = "Полностью заменяет отдел")]
    [HttpPut("departments/{id}")]
    public async Task<IActionResult> Replace_department(int id, [FromBody] Department department, CancellationToken cancellationToken)
    {
        var dep = await _dbContext.Departments.FindAsync(new object[] { id }, cancellationToken);
        if (dep == null) return NotFound("Отдел не найден");

        dep.Name = department.Name;
        dep.Description = department.Description;
        dep.IsActive = department.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(dep);
    }

    /// <summary>
    /// Удалить отдел (мягко).
    /// </summary>
    [SwaggerOperation(Summary = "Удалить отдел", Description = "Удаляет отдел (мягко)")]
    [HttpDelete("departments/{id}")]
    public async Task<IActionResult> Delete_department(int id, CancellationToken cancellationToken)
    {
        var dep = await _dbContext.Departments.FindAsync(new object[] { id }, cancellationToken);
        if (dep == null)
            return NotFound("Отдел не найден");

        if (dep.IsActive == false)
            return BadRequest("Отдел уже неактивен.");

        dep.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Отдел успешно помечен как неактивный.");
    }
}

// [ApiController]
// [Route("api")]
// public class DepartmentsController : ControllerBase
// {
//     private readonly SiteDbContext _dbContext;
//
//     public DepartmentsController(SiteDbContext dbContext)
//     {
//         _dbContext = dbContext;
//     }
//
//    
// }
