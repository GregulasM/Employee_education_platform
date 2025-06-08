namespace eep_backend.Models.UserModuleModels;

public class UserCreateDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    
    public string? SecondName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
}