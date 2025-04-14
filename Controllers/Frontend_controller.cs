using Microsoft.AspNetCore.Mvc;

namespace Employee_education_platform.Controllers;

/// <summary>
/// Контроллер для фронтенда, тут будем чисто выгружать данные.
/// </summary>
[ApiController]
[Route("api/frontend")]
public class Frontend_controller : ControllerBase
{
    /// <summary>
    /// Метод для получения списка курсов!
    /// </summary>
    [HttpGet("courses")]
    public IActionResult Get_courses()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Метод для получения информации по конкретному курсу, мб даже включу модули и статьи.
    /// </summary>
    [HttpGet("courses/{course_id}")]
    public IActionResult Get_course_details(string course_id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Метод для получения профиля пользователя. Сюда же пойдет и прогресс прохождения курсов, данные персонажа и достижения.
    /// </summary>
    [HttpGet("users/{user_id}/profile")]
    public IActionResult Get_user_profile(string user_id)
    {
        throw new NotImplementedException();
    }
}