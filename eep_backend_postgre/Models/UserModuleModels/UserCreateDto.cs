namespace eep_backend.Models.UserModuleModels;

public class UserCreateDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public double? Rating { get; set; }
    public int? ThemeId { get; set; }
    public int? FontId { get; set; }
    public int? ActiveCourseId { get; set; }
    public int? SelectedCharacterId { get; set; }
    public int? DepartmentId { get; set; }
    public int? RoleId { get; set; }
}
