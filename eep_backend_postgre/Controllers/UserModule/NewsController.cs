using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class NewsAdminController : ControllerBase
{
    /// <summary>
    /// Создать новость.
    /// </summary>
    [SwaggerOperation(
        Summary = "Создать новость",
        Description = "Создает новую новость"
    )]
    [HttpPost("users/news")]
    public IActionResult Create_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Частично обновить новость.
    /// </summary>
    [SwaggerOperation(
        Summary = "Частично обновить новость",
        Description = "Частично обновляет новость"
    )]
    [HttpPatch("users/news/{slug_news}")]
    public IActionResult Edit_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Полностью заменить новость.
    /// </summary>
    [SwaggerOperation(
        Summary = "Полностью заменить новость",
        Description = "Полностью заменяет новость"
    )]
    [HttpPut("users/news/{slug_news}")]
    public IActionResult Replace_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Удалить новость.
    /// </summary>
    [SwaggerOperation(
        Summary = "Удалить новость",
        Description = "Удаляет новость"
    )]
    [HttpDelete("users/news/{slug_news}")]
    public IActionResult Delete_news()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class NewsController : ControllerBase
{
    /// <summary>
    /// Получить все новости.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить все новости",
        Description = "Выгружает все новости"
    )]
    [HttpGet("users/news")]
    public IActionResult Read_All_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить конкретную новость по slug.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить новость по slug",
        Description = "Выгружает новость по slug"
    )]
    [HttpGet("users/news/{slug_news}")]
    public IActionResult Read_news()
    {
        throw new NotImplementedException();
    }
}
