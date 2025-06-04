using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;


[ApiController]
[Route("api/admin_panel")]
public class TestResultsAdminController : ControllerBase
{
    /// <summary>
    /// Создать результат прохождения теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Создать результат теста",
        Description = "Создает новый результат теста для пользователя"
    )]
    [HttpPost("courses/{name_course}/tests/{name_test}/test_results")]
    public IActionResult Create_test_result()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Частичное обновление результата теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Частичное обновление результата теста",
        Description = "Частично меняет результат теста"
    )]
    [HttpPatch("courses/{name_course}/tests/{name_test}/test_results/{name_user}")]
    public IActionResult Edit_test_result()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Полное обновление результата теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Полное обновление результата теста",
        Description = "Полностью заменяет результат теста"
    )]
    [HttpPut("courses/{name_course}/tests/{name_test}/test_results/{name_user}")]
    public IActionResult Replace_test_result()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Удалить результат теста пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Удалить результат теста",
        Description = "Удаляет результат теста для пользователя"
    )]
    [HttpDelete("courses/{name_course}/tests/{name_test}/test_results/{name_user}")]
    public IActionResult Delete_test_result()
    {
        throw new NotImplementedException();
    }
}


[ApiController]
[Route("api")]
public class TestResultsController : ControllerBase
{
    /// <summary>
    /// Получить все результаты теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить все результаты теста",
        Description = "Выгружает все результаты теста"
    )]
    [HttpGet("courses/{name_course}/tests/{name_test}/test_results")]
    public IActionResult Read_All_test_results()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить все результаты по тесту конкретного пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить результаты теста пользователя",
        Description = "Выгружает все попытки пользователя по данному тесту"
    )]
    [HttpGet("courses/{name_course}/tests/{name_test}/test_results/{name_user}")]
    public IActionResult Read_User_TestResults()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить один результат по id результата теста.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить результат теста по попытке",
        Description = "Выгружает конкретную попытку теста пользователя"
    )]
    [HttpGet("courses/{name_course}/tests/{name_test}/test_results/{name_user}/{id_test_result}")]
    public IActionResult Read_One_TestResult()
    {
        throw new NotImplementedException();
    }
}
