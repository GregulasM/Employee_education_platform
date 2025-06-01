using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class AchievementListsAdminController : ControllerBase
{
    /// <summary>
    /// Метод для создания листа достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания листа достижений.", 
        Description = "Создает лист достижений."
    )]
    [HttpPost("achievementlist")]
    public IActionResult Create_list()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для частичного изменения листа достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения листа достижений.", 
        Description = "Частично меняет лист достижений."
    )]
    [HttpPatch("achievementlist")]
    public IActionResult Edit_list()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения листа достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения листа достижений.", 
        Description = "Полностью меняет лист достижений."
    )]
    [HttpPut("achievementlist")]
    public IActionResult Replace_list()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления листа достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления листа достижений.", 
        Description = "Удаляет лист достижений."
    )]
    [HttpDelete("achievementlist/{name_achievementlist}")]
    public IActionResult Delete_list()
    {
        throw new NotImplementedException();
    }
    
}


[Route("api")]
public class AchievementListsController : ControllerBase
{
    /// <summary>
    /// Метод для выгрузки всех листов достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех листов достижений.", 
        Description = "Выгружает все листы достижений."
    )]
    [HttpGet("achievementlist")]
    public IActionResult Read_All_list()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для выгрузки только одного листа достижений по названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одного листа достижений по названию.", 
        Description = "Выгружает только один лист достижений по названию."
    )]
    [HttpGet("achievementlist/{name_achievementlist}")]
    public IActionResult Read_list()
    {
        throw new NotImplementedException();
    }
}