using eep_backend;
using eep_backend.Models.UserModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Employee_education_platform.Controllers;

[ApiController]
[Route("api/admin_panel")]
public class UserRolesController : ControllerBase
{
    private readonly SiteDbContext _dbContext;

    public UserRolesController(SiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получить все роли.
    /// </summary>
    [SwaggerOperation(
        Summary = "Получить все роли пользователей.",
        Description = "Выгружает все роли пользователей."
    )]
    [HttpGet("user_roles")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var roles = await _dbContext.UserRoles
            .Where(r => r.IsActive ?? true)
            .Select(r => new UserRoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsActive = r.IsActive
            })
            .ToListAsync(cancellationToken);

        return Ok(roles);
    }

    /// <summary>
    /// Создать новую роль.
    /// </summary>
    [SwaggerOperation(
        Summary = "Создать новую роль.",
        Description = "Создаёт новую роль пользователя."
    )]
    [HttpPost("user_roles")]
    public async Task<IActionResult> Create([FromBody] UserRoleDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Название обязательно");

        var entity = new UserRole
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true
        };

        _dbContext.UserRoles.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        dto.Id = entity.Id;
        dto.IsActive = entity.IsActive;

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
    }

    /// <summary>
    /// Получить одну роль по id.
    /// </summary>
    [HttpGet("user_roles/{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var r = await _dbContext.UserRoles.FindAsync(new object[] { id }, cancellationToken);
        if (r == null || r.IsActive == false) return NotFound();

        return Ok(new UserRoleDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            IsActive = r.IsActive
        });
    }


    /// <summary>
    /// Изменить роль.
    /// </summary>
    [HttpPatch("user_roles/{id}")]
    public async Task<IActionResult> EditRole(int id, [FromBody] UserRoleDto dto, CancellationToken cancellationToken)
    {
        var role = await _dbContext.UserRoles.FindAsync(id);
        if (role == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Name))
            role.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            role.Description = dto.Description;

        if (dto.IsActive.HasValue)
            role.IsActive = dto.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new UserRoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive
        });
    }

    /// <summary>
    /// Удалить роль.
    /// </summary>
    [HttpDelete("user_roles/{id}")]
    public async Task<IActionResult> DeleteRole(int id, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Users.FindAsync(id, cancellationToken);
        if (role == null)
            return NotFound();

        if (role.IsActive == false)
            return BadRequest("Роль уже неактивена.");

        role.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok("Роль успешно помечена как неактивная.");
    }
}
