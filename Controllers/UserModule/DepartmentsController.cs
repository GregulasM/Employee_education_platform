using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class DepartmentsAdminController : ControllerBase
{
    /// <summary>
    /// Создать отдел.
    /// </summary>
    [SwaggerOperation(
        Summary = "Создать отдел",
        Description = "Создает новый отдел"
    )]
    [HttpPost("users/departments")]
    public IActionResult Create_department()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Частично обновить отдел.
    /// </summary>
    [SwaggerOperation(
        Summary = "Частично обновить отдел",
        Description = "Частично обновляет отдел"
    )]
    [HttpPatch("users/departments/{name_department}")]
    public IActionResult Edit_department()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Полностью заменить отдел.
    /// </summary>
    [SwaggerOperation(
        Summary = "Полностью заменить отдел",
        Description = "Полностью заменяет отдел"
    )]
    [HttpPut("users/departments/{name_department}")]
    public IActionResult Replace_department()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Удалить отдел.
    /// </summary>
    [SwaggerOperation(
        Summary = "Удалить отдел",
        Description = "Удаляет отдел"
    )]
    [HttpDelete("users/departments/{name_department}")]
    public IActionResult Delete_department()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class DepartmentsController : ControllerBase
{
    /// <summary>
    /// Получить все отделы.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить все отделы",
        Description = "Выгружает все отделы"
    )]
    [HttpGet("users/departments")]
    public IActionResult Read_All_department()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить конкретный отдел по названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить конкретный отдел",
        Description = "Выгружает отдел по названию"
    )]
    [HttpGet("users/departments/{name_department}")]
    public IActionResult Read_department()
    {
        throw new NotImplementedException();
    }
}
