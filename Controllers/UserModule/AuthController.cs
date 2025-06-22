using eep_backend;
using eep_backend.Models.UserModuleModels;
using eep_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SiteDbContext _dbContext;
    private readonly JwtTokenService _jwtService;

    public AuthController(SiteDbContext dbContext, JwtTokenService jwtService)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
    }

    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Логин и пароль обязательны");
        
        var user = await _dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .Include(u => u.SelectedCharacter)
            .Include(u => u.ActiveCourse)
            .FirstOrDefaultAsync(u => u.Login == request.Login && (u.IsActive ?? true));

        if (user == null)
            return Unauthorized("Неверный логин или пароль");

        if (!PasswordHasher.VerifyPassword(request.Password, user.Password))
            return Unauthorized("Неверный логин или пароль");

        var token = _jwtService.GenerateToken(user);
        
        var userDto = new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            PhoneNumber = user.PhoneNumber,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            LastName = user.LastName,
            Email = user.Email,
            Avatar = user.Avatar,
            Rating = user.Rating,
            ThemeId = user.ThemeId,
            FontId = user.FontId,
            ActiveCourseId = user.ActiveCourseId,
            SelectedCharacterId = user.SelectedCharacterId,
            DepartmentId = user.DepartmentId,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsActive = user.IsActive
        };

        return Ok(new { token, user = userDto });
    }
}
