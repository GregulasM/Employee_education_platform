namespace eep_backend.Models.CourseModuleModels;

public class TestResultDto
{
    public int? UserId { get; set; }
    public int? TestId { get; set; }
    public double? Score { get; set; }
    public double? MaxScore { get; set; }
    public string? Answers { get; set; }

    public string? Status { get; set; }
}