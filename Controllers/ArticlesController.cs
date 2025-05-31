using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;


[ApiController]
[Route("api/admin_panel")]
public class ArticlesAdminController
{
    
    /// <summary>
    /// Метод для создания статьи внутри модуля.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания статьи внутри модуля.", 
        Description = "Создает статью внутри модуля."
    )]
    [HttpPost("courses/{name_course}/modules/{name_module}/articles")]
    public IActionResult Create_article()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для изменения деталей статьи по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для изменения деталей статьи по уникальному названию.", 
        Description = "Вводим название статьи и изменяем его части."
    )]
    [HttpPatch("courses/{name_course}/modules/{name_module}/articles/{name_article}")]
    public IActionResult Edit_article()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения статьи по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения статьи по уникальному названию.", 
        Description = "Вводим название статьи и полностью меняем его."
    )]
    [HttpPut("courses/{name_course}/modules/{name_module}/articles/{name_article}")]
    public IActionResult Replace_article()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления статьи по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления статьи по уникальному названию.", 
        Description = "Вводим название статьи и удаляем ее."
    )]
    [HttpDelete("courses/{name_course}/modules/{name_module}/articles/{name_article}")]
    public IActionResult Delete_article()
    {
        throw new NotImplementedException();
    }
}

[Route("api")]
public class ArticlesController
{
    
    /// <summary>
    /// Метод для просмотра всех статей внутри модуля.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для просмотра всех статей внутри модуля.", 
        Description = "Просмотр всех статей модуля, привязанного к курсу"
    )]
    [HttpGet("courses/{name_course}/modules/{name_module}/articles")]
    public IActionResult Read_All_article()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для просмотра только одной статьи внутри модуля.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для просмотра только одной статьи внутри модуля.", 
        Description = "Просмотр только одной статьи, привязанной к модулю"
    )]
    [HttpGet("courses/{name_course}/modules/{name_module}/articles/{name_article}")]
    public IActionResult Read_article()
    {
        throw new NotImplementedException();
    }
}