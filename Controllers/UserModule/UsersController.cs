using eep_backend;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class UsersAdminController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public UsersAdminController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
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
        var result = _dbContext.Users.ToList();
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
    public async Task<IActionResult> Create_user([FromBody] UserCreateDto userCreateDto, CancellationToken cancellationToken)
    {
        if (userCreateDto == null)
            return BadRequest("Пользователь не передан.");

        if (string.IsNullOrWhiteSpace(userCreateDto.Login))
            return BadRequest("Логин обязателен.");

        var user = new User
        {
            Login = userCreateDto.Login,
            Password = userCreateDto.Password,
            PhoneNumber = userCreateDto.PhoneNumber,
            FirstName = userCreateDto.FirstName,
            SecondName = userCreateDto.SecondName,
            LastName = userCreateDto.LastName,
            Email = userCreateDto.Email,
            Avatar = userCreateDto.Avatar,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Read_user), new { id = user.Id }, user);
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
        if (!int.TryParse(id, out var userId))
            return BadRequest("Некорректный id пользователя.");

        var user = await _dbContext.Users.FindAsync(userId, cancellationToken);
        if (user == null)
            return NotFound();
        return Ok(user);
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
        if (!int.TryParse(id, out var userId))
            return BadRequest("Некорректный id пользователя.");

        var existingUser = await _dbContext.Users.FindAsync(userId, cancellationToken);
        if (existingUser == null)
            return NotFound();
        
        existingUser.Login = user.Login;
        existingUser.Password = user.Password;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.FirstName = user.FirstName;
        existingUser.SecondName = user.SecondName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.Avatar = user.Avatar;
        existingUser.Rating = user.Rating;
        existingUser.ThemeId = user.ThemeId;
        existingUser.FontId = user.FontId;
        existingUser.ActiveCourseId = user.ActiveCourseId;
        existingUser.SelectedCharacterId = user.SelectedCharacterId;
        existingUser.DepartmentId = user.DepartmentId;
        existingUser.RoleId = user.RoleId;
        existingUser.UpdatedAt = DateTime.UtcNow;
        existingUser.IsActive = user.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(existingUser);
    }
    
    /// <summary>
    /// Метод для удаления пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для удаления пользователя.", 
        Description = "Удаляет пользователя."
    )]
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> Delete_user(string id, CancellationToken cancellationToken)
    {
        if (!int.TryParse(id, out var userId))
            return BadRequest("Некорректный id пользователя.");

        var user = await _dbContext.Users.FindAsync(userId, cancellationToken);
        if (user == null)
            return NotFound();

        if (user.IsActive == false)
            return BadRequest("Пользователь уже неактивен.");

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Пользователь успешно помечен как неактивный.");
    }
}

[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    
    private readonly SiteDbContext _dbContext;

    public UsersController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;  
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
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Login == name_user, cancellationToken);

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
    public async Task<IActionResult> Edit_Min_user(string id, [FromBody] UserPatchDto userPatchDto,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(id, out var userId))
            return BadRequest("Некорректный id пользователя.");

        var user = await _dbContext.Users.FindAsync(userId, cancellationToken);
        if (user == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(userPatchDto.Login))
            user.Login = userPatchDto.Login;

        if (!string.IsNullOrWhiteSpace(userPatchDto.Password))
            user.Password = userPatchDto.Password;

        if (!string.IsNullOrWhiteSpace(userPatchDto.PhoneNumber))
            user.PhoneNumber = userPatchDto.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(userPatchDto.FirstName))
            user.FirstName = userPatchDto.FirstName;

        if (!string.IsNullOrWhiteSpace(userPatchDto.SecondName))
            user.SecondName = userPatchDto.SecondName;

        if (!string.IsNullOrWhiteSpace(userPatchDto.LastName))
            user.LastName = userPatchDto.LastName;

        if (!string.IsNullOrWhiteSpace(userPatchDto.Email))
            user.Email = userPatchDto.Email;

        if (!string.IsNullOrWhiteSpace(userPatchDto.Avatar))
            user.Avatar = userPatchDto.Avatar;

        if (userPatchDto.Rating.HasValue)
            user.Rating = userPatchDto.Rating;

        if (userPatchDto.ThemeId.HasValue)
            user.ThemeId = userPatchDto.ThemeId;

        if (userPatchDto.FontId.HasValue)
            user.FontId = userPatchDto.FontId;

        if (userPatchDto.ActiveCourseId.HasValue)
            user.ActiveCourseId = userPatchDto.ActiveCourseId;

        if (userPatchDto.SelectedCharacterId.HasValue)
            user.SelectedCharacterId = userPatchDto.SelectedCharacterId;

        if (userPatchDto.DepartmentId.HasValue)
            user.DepartmentId = userPatchDto.DepartmentId;
        
        if (userPatchDto.RoleId.HasValue)
            user.RoleId = userPatchDto.RoleId;

        if (userPatchDto.IsActive.HasValue)
            user.IsActive = userPatchDto.IsActive;

        user.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(user);
    }
}