using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class AchievementsAdminController : ControllerBase
{
    /// <summary>
    /// Метод для создания достижения в определенном списке достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания достижения в определенном списке достижений.", 
        Description = "Создает достижение в списке."
    )]
    [HttpPost("achievementlists/{name_achievementlist}/achievements")]
    public IActionResult Create_achievement()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для частичного изменения достижения в определенном списке достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения достижения в определенном списке достижений.", 
        Description = "Частично меняет достижение в списке."
    )]
    [HttpPatch("achievementlists/{name_achievementlist}/achievements/{name_achievement}")]
    public IActionResult Edit_achievement()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения достижения в определенном списке достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения достижения в определенном списке достижений.", 
        Description = "Полностью меняет достижение в списке."
    )]
    [HttpPut("achievementlists/{name_achievementlist}/achievements/{name_achievement}")]
    public IActionResult Replace_achievement()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления достижения в определенном списке достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления достижения в определенном списке достижений.", 
        Description = "Удаляет достижение из списка."
    )]
    [HttpDelete("achievementlists/{name_achievementlist}/achievements/{name_achievement}")]
    public IActionResult Delete_achievement()
    {
        throw new NotImplementedException();
    }
    
}


[Route("api")]
public class AchievementsController : ControllerBase
{
    /// <summary>
    /// Метод для выгрузки всех достижений из определенного списка достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех достижений из определенного списка достижений.", 
        Description = "Выгружает все достижения из списка."
    )]
    [HttpGet("achievementlists/{name_achievementlist}/achievements")]
    public IActionResult Read_All_achievement()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для выгрузки только одного достижения по названию из определенного списка достижений.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одного достижения по названию из определенного списка достижений.", 
        Description = "Выгружает только одно достижение по названию из списка."
    )]
    [HttpGet("achievementlists/{name_achievementlist}/achievements/{name_achievement}")]
    public IActionResult Read_achievement()
    {
        throw new NotImplementedException();
    }
}