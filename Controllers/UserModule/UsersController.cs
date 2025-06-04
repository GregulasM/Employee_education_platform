using eep_backend.Models;
using Microsoft.AspNetCore.Mvc;
using SurrealDb.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class UsersAdminController : ControllerBase
{
    private string table = "user";
    private readonly ISurrealDbClient _surrealDbClient;

    public UsersAdminController(ISurrealDbClient surrealDbClient)
    {
        _surrealDbClient = surrealDbClient;
    }
    
    /// <summary>
    /// Метод для выгрузки всех пользователей.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех пользователей.", 
        Description = "Выгружает всех пользователей."
    )]
    [HttpGet("users")]
    public async Task<IActionResult> Read_All_user()
    {
        var result = await _surrealDbClient.Select<User>(table);
        return Ok(result);
    }
    
    /// <summary>
    /// Метод для создания пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания пользователя.", 
        Description = "Создает пользователя."
    )]
    [HttpPost("users")]
    public IActionResult Create_user()
    {
        throw new NotImplementedException();
    }
    
    
    
    /// <summary>
    /// Метод для полного изменения пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для полного изменения пользователя.", 
        Description = "Полностью меняет пользователя."
    )]
    [HttpPut("users/{name_user}")]
    public IActionResult Replace_user()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для удаления пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления пользователя.", 
        Description = "Удаляет пользователя."
    )]
    [HttpDelete("users/{name_user}")]
    public IActionResult Delete_user()
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    private string table = "user";
    private readonly ISurrealDbClient _surrealDbClient;

    public UsersController(ISurrealDbClient surrealDbClient)
    {
        _surrealDbClient = surrealDbClient;
    }
    /// <summary>
    /// Метод для выгрузки только одного пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одного пользователя.", 
        Description = "Выгружает только одного пользователя."
    )]
    [HttpGet("users/{id}")]
    public async Task<IActionResult> Read_user(string id, CancellationToken cancellationToken)
    {
        var ogo = await _surrealDbClient.Select<User>((table, id), cancellationToken);

        if (ogo is null)
            return NotFound();

        return Ok(ogo);
    }
    
    /// <summary>
    /// Метод для частичного изменения пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения пользователя.", 
        Description = "Частично меняет пользователя."
    )]
    [HttpPatch("users/{name_user}")]
    public IActionResult Edit_user()
    {
        throw new NotImplementedException();
    }
}