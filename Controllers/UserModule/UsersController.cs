using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using SurrealDb.Net;
using SurrealDb.Net.Models;
using Swashbuckle.AspNetCore.Annotations;
using SystemTextJsonPatch;

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
    public async Task<IActionResult> Read_All_user(CancellationToken cancellationToken)
    {
        var result = await _surrealDbClient.Select<User>(table, cancellationToken);
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
    public async Task<IActionResult> Create_user([FromBody] User user)
    {
        var result = await _surrealDbClient.Create(table, user);
        return Ok(result);
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
        // Получить текущего пользователя
        var user_new = await _surrealDbClient.Select<User>((table, id), cancellationToken);

        if (user_new == null)
            return NotFound();

        user.login = user_new.login;
        user.password                         =         user_new.password           ;
        user.phone_number                     =             user_new.phone_number       ;
        user.first_name                       =             user_new.first_name         ;
        user.second_name                      =             user_new.second_name        ;
        user.last_name                        =             user_new.last_name          ;
        user.email                            =         user_new.email              ;
        user.avatar                           =         user_new.avatar             ;
        user.rating                           =         user_new.rating             ;
        user.theme                            =         user_new.theme              ;
        user.font                             =     user_new.font               ;
        user.active_course                    =                 user_new.active_course      ;
        user.chosen_courses                   =                 user_new.chosen_courses     ;
        user.achievements                     =             user_new.achievements       ;
        user.selected_character               =                     user_new.selected_character ;
        user.department                       =             user_new.department         ;
        user.updated_at     =                                 DateTime.UtcNow   ;      
        user.is_active                        =             user_new.is_active          ;
        
        user.Id = user_new.Id;
        user.created_at = user_new.created_at;
        
        var result = await _surrealDbClient.Upsert( user, cancellationToken );
        return Ok(result);
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
        await _surrealDbClient.Delete((table, id));
        return Ok();
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
        var result = await _surrealDbClient.Select<User>((table, id), cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
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
        var result = await _surrealDbClient.Query($@"SELECT * FROM user WHERE login = {name_user}",  cancellationToken);
        var users = result.GetValue<List<User>>(0);

        var user = users.FirstOrDefault();
        if (user == null)
            return NotFound();

        return Ok(user);
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
        var existing = await _surrealDbClient.Select<User>((table, id), cancellationToken);
        if (existing == null)
            return NotFound();
            
        // Создаем объект для частичного обновления
        var updateData = new Dictionary<string, object>
        {
            ["updated_at"] = DateTime.UtcNow
        };
        
        if (userDto.FirstName != null)
            updateData["first_name"] = userDto.FirstName;
        if (userDto.SecondName != null)
            updateData["second_name"] = userDto.SecondName;
        if (userDto.LastName != null)
            updateData["last_name"] = userDto.LastName;
        
        var result = await _surrealDbClient.Merge<User>((table, id), updateData, cancellationToken);
        return Ok(result);
    }
}