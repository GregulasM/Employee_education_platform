using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class ModulesAdminController : ControllerBase
{
    /// <summary>
    /// Метод для создания модуля внутри курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания модуля внутри курса.", 
        Description = "Создает модуль, привязанный к курсу"
    )]
    [HttpPost("courses/{name_course}/modules")]
    public IActionResult Create_module()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для изменения деталей модуля по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для изменения деталей модуля по уникальному названию.", 
        Description = "Вводим название модуля и изменяем его части."
    )]
    [HttpPatch("courses/{name_course}/modules/{name_module}")]
    public IActionResult Edit_module()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения модуля по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения модуля по уникальному названию.", 
        Description = "Вводим название модуля и полностью меняем его."
    )]
    [HttpPut("courses/{name_course}/modules/{name_module}")]
    public IActionResult Replace_module()
    {
        throw new NotImplementedException();
    }
    
    
    /// <summary>
    /// Метод для удаления модуля по уникальному названию.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления модуля по уникальному названию.", 
        Description = "Вводим название модуля и удаляем его."
    )]
    [HttpDelete("courses/{name_course}/modules/{name_module}")]
    public IActionResult Delete_module()
    {
        throw new NotImplementedException();
    }
}

[Route("api")]
public class ModulesController : ControllerBase
{
    /// <summary>
    /// Метод для чтения всех модулей внутри курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для чтения всех модулей внутри курса.", 
        Description = "Просматривает все модули, привязанный к курсу"
    )]
    [HttpGet("courses/{name_course}/modules")]
    public IActionResult Read_All_module()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Метод для чтения только одного модуля внутри курса.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для чтения только одного модуля внутри курса.", 
        Description = "Просматривает только один модуль, привязанный к курсу"
    )]
    [HttpGet("courses/{name_course}/modules/{name_module}")]
    public IActionResult Read_module()
    {
        throw new NotImplementedException();
    }
}