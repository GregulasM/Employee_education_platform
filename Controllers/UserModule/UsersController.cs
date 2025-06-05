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
        
        user_new.login = user.login;
        user_new.password = user.password;
        user_new.phone_number = user.phone_number;
        user_new.first_name = user.first_name;
        user_new.second_name = user.second_name;
        user_new.last_name = user.last_name;
        user_new.email = user.email;
        user_new.avatar = user.avatar;
        user_new.rating = user.rating;
        user_new.theme = user.theme;
        user_new.font = user.font;
        user_new.active_course = user.active_course;
        user_new.chosen_courses = user.chosen_courses;
        user_new.achievements = user.achievements;
        user_new.selected_character = user.selected_character;
        user_new.department = user.department;
        user_new.updated_at = user.updated_at ?? DateTime.UtcNow;
        user_new.is_active = user.is_active;

        // Не меняем user_new.id — это важно!
        var result = await _surrealDbClient.Upsert(user_new, cancellationToken);

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
    public async Task<IActionResult> Delete_user(string name_user)
    {
        await _surrealDbClient.Delete(name_user);
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
    public async Task<IActionResult> Edit_Min_user(string id, [FromBody] UserDto user, CancellationToken cancellationToken)
    {
        var query = await _surrealDbClient.Select<User>((table, id), cancellationToken);
        if (query == null)
            return NotFound();

        if (user.FirstName != null) query.first_name = user.FirstName;
        if (user.SecondName != null) query.second_name = user.SecondName;
        if (user.LastName != null) query.last_name = user.LastName;

        // Никаких изменений других полей!

        var result = await _surrealDbClient.Upsert(query, cancellationToken);
        return Ok(result);
    }
}