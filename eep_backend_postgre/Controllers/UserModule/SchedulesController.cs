using eep_backend;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class SchedulesAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public SchedulesAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Создать расписание.
    /// </summary>
    [SwaggerOperation(Summary = "Создать расписание", Description = "Создает новое расписание")]
    [HttpPost("users/schedules")]
    public async Task<IActionResult> Create_schedule([FromBody] Schedule schedule, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(schedule.DayOfWeek) || string.IsNullOrWhiteSpace(schedule.TimeSlot))
            return BadRequest("День недели и время обязательны");
        schedule.Id = 0;
        schedule.IsActive = true;
        _dbContext.Schedules.Add(schedule);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Read_schedule), new { id_schedule = schedule.Id }, schedule);
    }
    
    /// <summary>
    /// Получить расписание по id.
    /// </summary>
    [SwaggerOperation(Summary = "Получить расписание по id", Description = "Выгружает расписание по id")]
    [HttpGet("users/schedules/{id_schedule}")]
    public async Task<IActionResult> Read_schedule(int id_schedule, CancellationToken cancellationToken)
    {
        var sch = await _dbContext.Schedules
            .Include(s => s.Course)
            .Include(s => s.Module)
            .Include(s => s.Department)
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id_schedule && s.IsActive == true, cancellationToken);
        if (sch == null)
            return NotFound("Расписание не найдено");
        return Ok(sch);
    }

    /// <summary>
    /// Частично обновить расписание.
    /// </summary>
    [SwaggerOperation(Summary = "Частично обновить расписание", Description = "Частично обновляет расписание")]
    [HttpPatch("users/schedules/{id_schedule}")]
    public async Task<IActionResult> Edit_schedule(int id_schedule, [FromBody] Schedule patch, CancellationToken cancellationToken)
    {
        var sch = await _dbContext.Schedules.FindAsync(new object[] { id_schedule }, cancellationToken);
        if (sch == null) return NotFound("Запись не найдена");

        if (!string.IsNullOrWhiteSpace(patch.DayOfWeek)) sch.DayOfWeek = patch.DayOfWeek;
        if (!string.IsNullOrWhiteSpace(patch.TimeSlot)) sch.TimeSlot = patch.TimeSlot;
        if (!string.IsNullOrWhiteSpace(patch.Subject)) sch.Subject = patch.Subject;
        if (!string.IsNullOrWhiteSpace(patch.Teacher)) sch.Teacher = patch.Teacher;
        if (!string.IsNullOrWhiteSpace(patch.Details)) sch.Details = patch.Details;
        if (patch.CourseId.HasValue) sch.CourseId = patch.CourseId;
        if (patch.ModuleId.HasValue) sch.ModuleId = patch.ModuleId;
        if (patch.DepartmentId.HasValue) sch.DepartmentId = patch.DepartmentId;
        if (patch.UserId.HasValue) sch.UserId = patch.UserId;
        if (patch.IsActive.HasValue) sch.IsActive = patch.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(sch);
    }

    /// <summary>
    /// Полностью заменить расписание.
    /// </summary>
    [SwaggerOperation(Summary = "Полностью заменить расписание", Description = "Полностью заменяет расписание")]
    [HttpPut("users/schedules/{id_schedule}")]
    public async Task<IActionResult> Replace_schedule(int id_schedule, [FromBody] Schedule schedule, CancellationToken cancellationToken)
    {
        var sch = await _dbContext.Schedules.FindAsync(new object[] { id_schedule }, cancellationToken);
        if (sch == null) return NotFound("Запись не найдена");

        sch.DayOfWeek = schedule.DayOfWeek;
        sch.TimeSlot = schedule.TimeSlot;
        sch.Subject = schedule.Subject;
        sch.Teacher = schedule.Teacher;
        sch.Details = schedule.Details;
        sch.CourseId = schedule.CourseId;
        sch.ModuleId = schedule.ModuleId;
        sch.DepartmentId = schedule.DepartmentId;
        sch.UserId = schedule.UserId;
        sch.IsActive = schedule.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(sch);
    }

    /// <summary>
    /// Удалить расписание (soft delete).
    /// </summary>
    [SwaggerOperation(Summary = "Удалить расписание", Description = "Мягко удаляет запись расписания")]
    [HttpDelete("users/schedules/{id_schedule}")]
    public async Task<IActionResult> Delete_schedule(int id_schedule, CancellationToken cancellationToken)
    {
        var sch = await _dbContext.Schedules.FindAsync(new object[] { id_schedule }, cancellationToken);
        if (sch == null)
            return NotFound("Запись не найдена");
        if (sch.IsActive == false)
            return BadRequest("Запись уже неактивна.");
        sch.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Запись расписания помечена как неактивная.");
    }
}

[ApiController]
[Route("api")]
public class SchedulesController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public SchedulesController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все расписания (только активные).
    /// </summary>
    [SwaggerOperation(Summary = "Получить все расписания", Description = "Выгружает все расписания")]
    [HttpGet("users/schedules")]
    public async Task<IActionResult> Read_All_schedule(CancellationToken cancellationToken)
    {
        var list = await _dbContext.Schedules
            .Where(s => s.IsActive == true)
            .Include(s => s.Course)
            .Include(s => s.Module)
            .Include(s => s.Department)
            .Include(s => s.User)
            .ToListAsync(cancellationToken);
        return Ok(list);
    }

    
}
