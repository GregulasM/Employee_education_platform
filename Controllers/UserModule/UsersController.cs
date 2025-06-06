using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class UsersAdminController : ControllerBase
{

    public UsersAdminController()
    {
        
    }
    
    /// <summary>
    /// Метод для выгрузки всех пользователей.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки всех пользователей.", 
        Description = "Выгружает всех пользователей."
    )]
    [HttpGet("users")]
    public async Task<IActionResult> Read_All_user(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для создания пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания пользователя.", 
        Description = "Создает пользователя."
    )]
    [HttpPost("users")]
    public async Task<IActionResult> Create_user([FromBody] User user)
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
    [HttpPut("users/{id}")]
    public async Task<IActionResult> Replace_user(string id, [FromBody] User user, CancellationToken cancellationToken)
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
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> Delete_user(string id)
    {
        throw new NotImplementedException();
    }
}

[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{

    public UsersController()
    {
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
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Метод для выгрузки только одного пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для выгрузки только одного пользователя.", 
        Description = "Выгружает только одного пользователя."
    )]
    [HttpGet("users/user_login/{name_user}")]
    public async Task<IActionResult> Read_Name_user(string name_user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    // /// <summary>
    // /// Метод для частичного изменения пользователя.
    // /// </summary>
    // [SwaggerOperation(
    //     Summary = "Метод для частичного изменения пользователя.", 
    //     Description = "Частично меняет пользователя."
    // )]
    // [HttpPatch("users/{id}")]
    // public async Task<IActionResult> Edit_user(string id, [FromBody] JsonPatchDocument<User> patch, CancellationToken cancellationToken)
    // {
    //     var query = await _surrealDbClient.Select<User>((table, id), cancellationToken);
    //     if (query == null)
    //         return NotFound();
    //
    //     patch.ApplyTo(query);
    //     var result = await _surrealDbClient.Upsert(query, cancellationToken);
    //     return Ok(result);
    // }
    
    /// <summary>
    /// Метод для частичного изменения пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для частичного изменения пользователя.", 
        Description = "Частично меняет пользователя."
    )]
    [HttpPatch("users/{id}")]
    public async Task<IActionResult> Edit_Min_user(string id, [FromBody] UserDto userDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}