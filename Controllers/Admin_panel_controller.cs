using Employee_education_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class Admin_panel_controller : ControllerBase
{
    /// <summary>
    /// Метод для создания самого курса. Принимает данные курса через DTO и сохраняет его в БД.
    /// </summary>
    [HttpPost("courses")]
    public IActionResult Create_course([FromBody] Course_dto course_dto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Метод для создания модуля внутри курса!
    /// </summary>
    [HttpPost("courses/{course_id}/modules")]
    public IActionResult Create_module(string course_id, [FromBody] Module_dto module_dto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Метод для создания статьи для конкретного модуля!
    /// </summary>
    [HttpPost("modules/{module_id}/articles")]
    public IActionResult Create_article(string module_id, [FromBody] Article_dto article_dto)
    {
        throw new NotImplementedException();
    }
}


