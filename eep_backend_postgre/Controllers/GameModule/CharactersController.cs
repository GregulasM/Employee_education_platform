using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class CharactersAdminController : ControllerBase
{
    /// <summary>
    /// Метод для создания персонажа.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания персонажа.", 
        Description = "Создает персонажа."
    )]
    [HttpPost("characters")]
    public IActionResult Create_character()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для частичного изменения персонажа.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения персонажа.", 
        Description = "Частично меняет персонажа."
    )]
    [HttpPatch("characters/{name_character}")]
    public IActionResult Edit_character()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения персонажа.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения персонажа.", 
        Description = "Полностью меняет персонажа."
    )]
    [HttpPut("characters/{name_character}")]
    public IActionResult Replace_character()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления персонажа.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления персонажа.", 
        Description = "Удаляет персонажа."
    )]
    [HttpDelete("characters/{name_character}")]
    public IActionResult Delete_character()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class CharactersController : ControllerBase
{
    
    /// <summary>
    /// Метод для выгрузки всех достижений из определенного списка достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех достижений из определенного списка достижений.", 
        Description = "Выгружает все достижения из списка."
    )]
    [HttpGet("characters")]
    public IActionResult Read_All_character()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для выгрузки только одного персонажа.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одного персонажа.", 
        Description = "Выгружает только одно персонажа."
    )]
    [HttpGet("characters/{name_character}")]
    public IActionResult Read_character()
    {
        throw new NotImplementedException();
    }
}