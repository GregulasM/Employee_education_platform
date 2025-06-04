using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class SchedulesAdminController : ControllerBase
{
    /// <summary>
    /// Создать расписание.
    /// </summary>
    [SwaggerOperation(
        Summary = "Создать расписание",
        Description = "Создает новое расписание"
    )]
    [HttpPost("users/schedules")]
    public IActionResult Create_schedule()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Частично обновить расписание.
    /// </summary>
    [SwaggerOperation(
        Summary = "Частично обновить расписание",
        Description = "Частично обновляет расписание"
    )]
    [HttpPatch("users/schedules/{id_schedule}")]
    public IActionResult Edit_schedule()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Полностью заменить расписание.
    /// </summary>
    [SwaggerOperation(
        Summary = "Полностью заменить расписание",
        Description = "Полностью заменяет расписание"
    )]
    [HttpPut("users/schedules/{id_schedule}")]
    public IActionResult Replace_schedule()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Удалить расписание.
    /// </summary>
    [SwaggerOperation(
        Summary = "Удалить расписание",
        Description = "Удаляет расписание"
    )]
    [HttpDelete("users/schedules/{id_schedule}")]
    public IActionResult Delete_schedule()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class SchedulesController : ControllerBase
{
    /// <summary>
    /// Получить все расписания.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить все расписания",
        Description = "Выгружает все расписания"
    )]
    [HttpGet("users/schedules")]
    public IActionResult Read_All_schedule()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить конкретное расписание по id.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить расписание по id",
        Description = "Выгружает расписание по id"
    )]
    [HttpGet("users/schedules/{id_schedule}")]
    public IActionResult Read_schedule()
    {
        throw new NotImplementedException();
    }
}
