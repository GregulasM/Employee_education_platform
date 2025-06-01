using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;


[ApiController]
[Route("api/admin_panel")]
public class SettingsAdminController : ControllerBase
{
    /// <summary>
    /// Метод для создания настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания настройки у пользователя.", 
        Description = "Создает настройку у пользователя."
    )]
    [HttpPost("users/{name_user}/settings")]
    public IActionResult Create_setting()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для частичного изменения настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения настройки у пользователя.", 
        Description = "Частично меняет настройку у пользователя."
    )]
    [HttpPatch("users/{name_user}/settings/{name_setting}")]
    public IActionResult Edit_setting()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения настройки у пользователя.", 
        Description = "Полностью меняет настройку у пользователя."
    )]
    [HttpPut("users/{name_user}/settings/{name_setting}")]
    public IActionResult Replace_setting()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления настройки у пользователя.", 
        Description = "Удаляет настройку у пользователя."
    )]
    [HttpDelete("users/{name_user}/settings/{name_setting}")]
    public IActionResult Delete_setting()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class SettingsController : ControllerBase
{

    /// <summary>
    /// Метод для выгрузки всех настроек у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех настроек у пользователя.",
        Description = "Выгружает все настройки пользователя."
    )]
    [HttpGet("users/{name_user}/settings")]
    public IActionResult Read_All_setting()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Метод для выгрузки только одной настройки у пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одной настройки у пользователя.",
        Description = "Выгружает только одну настройку у пользователя."
    )]
    [HttpGet("users/{name_user}/settings/{name_setting}")]
    public IActionResult Read_setting()
    {
        throw new NotImplementedException();
    }
}