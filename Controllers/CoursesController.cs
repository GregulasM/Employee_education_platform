using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class CoursesController : ControllerBase
{
    /// <summary>
    /// Метод для создания курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания курса.", 
        Description = "Создает новый курс."
        )]
    [HttpPost("courses")]
    public IActionResult Create_course()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для получения всех курсов.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для получения всех курсов.", 
        Description = "Получает все курсы, которые есть в БД."
    )]
    [HttpGet("courses")]
    public IActionResult Read_All_course()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для получения только одного курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для получения только одного курса.", 
        Description = "Получает только один курс."
    )]
    [HttpGet("courses/{name_course}")]
    public IActionResult Read_course()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для изменения деталей курса по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для изменения деталей курса по уникальному названию.", 
        Description = "Вводим название курса и изменяем его части."
    )]
    [HttpPatch("courses/{name_course}")]
    public IActionResult Edit_course()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения курса по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения курса по уникальному названию.", 
        Description = "Вводим название курса и изменяем все его детали."
    )]
    [HttpPut("courses/{name_course}")]
    public IActionResult Replace_course()
    {
        throw new NotImplementedException();
    }
    
    
    /// <summary>
    /// Метод для удаления курса по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления курса по уникальному названию.", 
        Description = "Вводим название курса и удаляем его."
    )]
    [HttpDelete("courses/{name_course}")]
    public IActionResult Delete_course()
    {
        throw new NotImplementedException();
    }
}


