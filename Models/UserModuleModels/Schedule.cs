using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eep_backend.Models.CourseModuleModels;


namespace eep_backend.Models.UserModuleModels;

public class Schedule
{
    public int Id { get; set; }
    
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PublicId { get; set; } = Guid.NewGuid();

    public string? DayOfWeek { get; set; }
    public string? TimeSlot { get; set; }
    public string? Subject { get; set; } 
    public string? Teacher { get; set; } 
    public string? Details { get; set; } 
    
    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public int? ModuleId { get; set; }
    public Module? Module { get; set; }

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public bool? IsActive { get; set; }
}