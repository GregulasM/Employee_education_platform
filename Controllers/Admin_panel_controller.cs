using Employee_education_platform.Models;
using Employee_education_platform.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employee_education_platform.Controllers;
[ApiController]
[Route("api/admin_panel")]
public class Admin_panel_controller : ControllerBase
{
    private readonly SurrealDbService _db;

    public Admin_panel_controller(SurrealDbService db)
    {
        _db = db;
    }

    [HttpPost("courses")]
    public async Task<IActionResult> Create_course([FromBody] Course_dto course_dto)
    {
        var created = await _db.CreateRecord("Course", course_dto);
        return Ok(created);
    }

    [HttpPost("courses/{course_id}/modules")]
    public async Task<IActionResult> Create_module(string course_id, [FromBody] Module_dto module_dto)
    {
        module_dto.course_id = course_id;
        var created = await _db.CreateRecord("Module", module_dto);
        return Ok(created);
    }

    [HttpPost("modules/{module_id}/articles")]
    public async Task<IActionResult> Create_article(string module_id, [FromBody] Article_dto article_dto)
    {
        article_dto.module_id = module_id;
        var created = await _db.CreateRecord("Article", article_dto);
        return Ok(created);
    }
}