using eep_backend;
using eep_backend.Models.CourseModuleModels;
using eep_backend.Models.GameModuleModels;
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
        var users = await _dbContext.Users
            .Where(u => u.IsActive != false)
            .Include(u => u.Department)
            .Include(u => u.Role)
            .Include(u => u.SelectedCharacter)
            .Include(u => u.ActiveCourse)
            .Include(u => u.ChosenCourses)
            .Include(u => u.Achievements)
            .ToListAsync(cancellationToken);

        var themeIds = users.Where(u => u.ThemeId.HasValue).Select(u => u.ThemeId.Value).Distinct().ToList();
        var fontIds = users.Where(u => u.FontId.HasValue).Select(u => u.FontId.Value).Distinct().ToList();
        
        var allSettingIds = themeIds.Concat(fontIds).Distinct().ToList();

        var settings = await _dbContext.Settings
            .Where(s => allSettingIds.Contains(s.Id) && (s.IsActive ?? true))
            .ToListAsync(cancellationToken);
        
        var themeMap = settings.Where(s => s.Type == "theme").ToDictionary(s => s.Id, s => s);
        var fontMap = settings.Where(s => s.Type == "font").ToDictionary(s => s.Id, s => s);

        var courseIds = users.Where(u => u.ActiveCourseId.HasValue).Select(u => u.ActiveCourseId.Value).ToList();
        var courses = await _dbContext.Courses
            .Where(c => courseIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        
        var dtos = users.Select(u =>
        {
            SettingDto themeDto = null;
            if (u.ThemeId.HasValue && themeMap.TryGetValue(u.ThemeId.Value, out var theme))
            {
                themeDto = new SettingDto
                {
                    Id = theme.Id,
                    Type = theme.Type,
                    Name = theme.Name,
                    Icon = theme.Icon
                };
            }

            SettingDto fontDto = null;
            if (u.FontId.HasValue && fontMap.TryGetValue(u.FontId.Value, out var font))
            {
                fontDto = new SettingDto
                {
                    Id = font.Id,
                    Type = font.Type,
                    Name = font.Name,
                    Icon = font.Icon
                };
            }

            var course = courses.FirstOrDefault(c => c.Id == u.ActiveCourseId);

            return new UserDto
            {
                Id = u.Id,
                Login = u.Login,
                PhoneNumber = u.PhoneNumber,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                LastName = u.LastName,
                Email = u.Email,
                Avatar = u.Avatar,
                Rating = u.Rating,
                ThemeId = u.ThemeId,
                Theme = themeDto,
                FontId = u.FontId,
                Font = fontDto,
                ActiveCourseId = u.ActiveCourseId,
                ActiveCourse = course == null ? null : new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title
                },
                SelectedCharacterId = u.SelectedCharacterId,
                SelectedCharacter = u.SelectedCharacter == null ? null : new CharacterDto
                {
                    Id = u.SelectedCharacter.Id,
                    Name = u.SelectedCharacter.Name
                },
                DepartmentId = u.DepartmentId,
                Department = u.Department == null ? null : new DepartmentDto
                {
                    Id = u.Department.Id,
                    Name = u.Department.Name
                },
                RoleId = u.RoleId,
                Role = u.Role == null ? null : new UserRoleDto
                {
                    Id = u.Role.Id,
                    Name = u.Role.Name
                },
                ChosenCourses = u.ChosenCourses?.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title
                }).ToList(),
                Achievements = u.Achievements?.Select(a => new AchievementDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList(),
                CreatedAt = u.CreatedAt != null ? u.CreatedAt.Value.AddHours(10) : null,
                UpdatedAt = u.UpdatedAt != null ? u.UpdatedAt.Value.AddHours(10) : null,
                IsActive = u.IsActive
            };
        }).ToList();

        return Ok(dtos);
    }

    /// <summary>
    /// Метод для создания пользователя.
    /// </summary>
    [SwaggerOperation(
        Summary = "Метод для создания пользователя.",
        Description = "Создает пользователя."
    )]
    [HttpPost("users")]
    public async Task<IActionResult> Create_user([FromBody] UserCreateDto userCreateDto,
        CancellationToken cancellationToken)
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
            Rating = userCreateDto.Rating,
            ThemeId = userCreateDto.ThemeId,
            FontId = userCreateDto.FontId,
            ActiveCourseId = userCreateDto.ActiveCourseId,
            SelectedCharacterId = userCreateDto.SelectedCharacterId,
            DepartmentId = userCreateDto.DepartmentId,
            RoleId = userCreateDto.RoleId,
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

        var u = await _dbContext.Users
            .Include(u => u.Department)
            .Include(u => u.Role)
            .Include(u => u.SelectedCharacter)
            .Include(u => u.ActiveCourse)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (u == null)
            return NotFound();
        
        SettingDto themeDto = null;
        SettingDto fontDto = null;

        if (u.ThemeId.HasValue)
        {
            var theme = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == u.ThemeId && s.Type == "theme" && (s.IsActive ?? true), cancellationToken);
            if (theme != null)
                themeDto = new SettingDto
                {
                    Id = theme.Id,
                    Type = theme.Type,
                    Name = theme.Name,
                    Icon = theme.Icon
                };
        }
        if (u.FontId.HasValue)
        {
            var font = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == u.FontId && s.Type == "font" && (s.IsActive ?? true), cancellationToken);
            if (font != null)
                fontDto = new SettingDto
                {
                    Id = font.Id,
                    Type = font.Type,
                    Name = font.Name,
                    Icon = font.Icon
                };
        }

        var dto = new UserDto
        {
            Id = u.Id,
            Login = u.Login,
            PhoneNumber = u.PhoneNumber,
            FirstName = u.FirstName,
            SecondName = u.SecondName,
            LastName = u.LastName,
            Email = u.Email,
            Avatar = u.Avatar,
            Rating = u.Rating,
            ThemeId = u.ThemeId,
            Theme = themeDto,
            FontId = u.FontId,
            Font = fontDto,
            ActiveCourseId = u.ActiveCourseId,
            ActiveCourse = u.ActiveCourse == null ? null : new CourseDto { Id = u.ActiveCourse.Id, Title = u.ActiveCourse.Title },
            SelectedCharacterId = u.SelectedCharacterId,
            SelectedCharacter = u.SelectedCharacter == null ? null : new CharacterDto { Id = u.SelectedCharacter.Id, Name = u.SelectedCharacter.Name },
            DepartmentId = u.DepartmentId,
            Department = u.Department == null ? null : new DepartmentDto { Id = u.Department.Id, Name = u.Department.Name },
            RoleId = u.RoleId,
            Role = u.Role == null ? null : new UserRoleDto { Id = u.Role.Id, Name = u.Role.Name },
            CreatedAt = u.CreatedAt != null ? u.CreatedAt.Value.AddHours(10) : null,
            UpdatedAt = u.UpdatedAt != null ? u.UpdatedAt.Value.AddHours(10) : null,
            IsActive = u.IsActive
        };

        return Ok(dto);
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
        var u = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == name_user, cancellationToken);
        if (u == null)
            return NotFound();

        var dto = new UserDto
        {
            Id = u.Id,
            Login = u.Login,
            PhoneNumber = u.PhoneNumber,
            FirstName = u.FirstName,
            SecondName = u.SecondName,
            LastName = u.LastName,
            Email = u.Email,
            Avatar = u.Avatar,
            Rating = u.Rating,
            ThemeId = u.ThemeId,
            FontId = u.FontId,
            ActiveCourseId = u.ActiveCourseId,
            SelectedCharacterId = u.SelectedCharacterId,
            DepartmentId = u.DepartmentId,
            RoleId = u.RoleId,
            CreatedAt = u.CreatedAt += TimeSpan.FromHours(10),
            UpdatedAt = u.UpdatedAt += TimeSpan.FromHours(10),
            IsActive = u.IsActive
        };

        return Ok(dto);
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