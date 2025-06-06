using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class CommentsAdminController : ControllerBase
{
    /// <summary>
    /// Создать комментарий к новости.
    /// </summary>
    [SwaggerOperation(
        Summary = "Создать комментарий к новости",
        Description = "Создает новый комментарий к новости"
    )]
    [HttpPost("users/news/{slug_news}/comments")]
    public IActionResult Create_comment_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Частично обновить комментарий.
    /// </summary>
    [SwaggerOperation(
        Summary = "Частично обновить комментарий",
        Description = "Частично обновляет комментарий"
    )]
    [HttpPatch("users/news/{slug_news}/comments/{id_comment}")]
    public IActionResult Edit_comment_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Полностью заменить комментарий.
    /// </summary>
    [SwaggerOperation(
        Summary = "Полностью заменить комментарий",
        Description = "Полностью заменяет комментарий"
    )]
    [HttpPut("users/news/{slug_news}/comments/{id_comment}")]
    public IActionResult Replace_comment_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Удалить комментарий.
    /// </summary>
    [SwaggerOperation(
        Summary = "Удалить комментарий",
        Description = "Удаляет комментарий"
    )]
    [HttpDelete("users/news/{slug_news}/comments/{id_comment}")]
    public IActionResult Delete_comment_news()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class CommentsController : ControllerBase
{
    /// <summary>
    /// Получить все комментарии новости.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить все комментарии новости",
        Description = "Выгружает все комментарии новости"
    )]
    [HttpGet("users/news/{slug_news}/comments")]
    public IActionResult Read_All_comment_news()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить конкретный комментарий по id.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить конкретный комментарий по id",
        Description = "Выгружает комментарий по id"
    )]
    [HttpGet("users/news/{slug_news}/comments/{id_comment}")]
    public IActionResult Read_comment_news()
    {
        throw new NotImplementedException();
    }
}
