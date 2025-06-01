using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;


[ApiController]
[Route("api/admin_panel")]
public class TestsAdminController : ControllerBase
{
    /// <summary>
    /// Метод для создания теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания теста.", 
        Description = "Создает тест."
    )]
    [HttpPost("courses/{name_course}/tests")]
    public IActionResult Create_test()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для частичного изменения теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения теста.", 
        Description = "Частично меняет тест."
    )]
    [HttpPatch("courses/{name_course}/tests/{name_test}")]
    public IActionResult Edit_test()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для полного изменения теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения теста.", 
        Description = "Полностью меняет тест."
    )]
    [HttpPut("courses/{name_course}/tests/{name_test}")]
    public IActionResult Replace_test()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления теста.", 
        Description = "Удаляет тест."
    )]
    [HttpDelete("courses/{name_course}/tests/{name_test}")]
    public IActionResult Delete_test()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class TestsController : ControllerBase
{
    
    /// <summary>
    /// Метод для выгрузки всех тестов.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех тестов.", 
        Description = "Выгружает все достижения из списка."
    )]
    [HttpGet("courses/{name_course}/tests")]
    public IActionResult Read_All_test()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для выгрузки только одного теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одного теста", 
        Description = "Выгружает только один тест."
    )]
    [HttpGet("courses/{name_course}/tests/{name_test}")]
    public IActionResult Read_test()
    {
        throw new NotImplementedException();
    }
}