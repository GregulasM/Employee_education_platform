namespace eep_backend.Models.UserModuleModels;

public class DepartmentDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public bool? IsActive { get; set; }
}