using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;


[ApiController]
[Route("api/admin_panel")]
public class UsefulLinksAdminController : ControllerBase
{
    /// <summary>
    /// Метод для создания полезной ссылки в курсе.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания полезной ссылки в курсе.", 
        Description = "Создает полезную ссылку в курсе."
    )]
    [HttpPost("courses/{name_course}/usefullinks")]
    public IActionResult Create_usefullink()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для частичного изменения полезной ссылки в курсе.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения полезной ссылки в курсе.", 
        Description = "Частично меняет полезную ссылку в курсе."
    )]
    [HttpPatch("courses/{name_course}/usefullinks/{name_usefullink}")]
    public IActionResult Edit_usefullink()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения полезной ссылки в курсе.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения полезной ссылки в курсе.", 
        Description = "Полностью меняет полезную ссылку в курсе."
    )]
    [HttpPut("courses/{name_course}/usefullinks/{name_usefullink}")]
    public IActionResult Replace_usefullink()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления полезной ссылки из курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления полезной ссылки из курса.", 
        Description = "Удаляет полезную ссылку из курса."
    )]
    [HttpDelete("courses/{name_course}/usefullinks/{name_usefullink}")]
    public IActionResult Delete_usefullink()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class UsefulLinksController : ControllerBase
{
    
    /// <summary>
    /// Метод для выгрузки всех полезных ссылок из курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех полезных ссылок из курса.", 
        Description = "Выгружает все полезные ссылки из курса."
    )]
    [HttpGet("courses/{name_course}/usefullinks")]
    public IActionResult Read_All_usefullink()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для выгрузки только одной полезной ссылки из курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одной полезной ссылки из курса", 
        Description = "Выгружает только одну полезную ссылку из курса."
    )]
    [HttpGet("courses/{name_course}/usefullinks/{name_usefullink}")]
    public IActionResult Read_usefullink()
    {
        throw new NotImplementedException();
    }
}