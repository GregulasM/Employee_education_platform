namespace eep_backend.Models.UserModuleModels;

public class CommentDto
{
    public int Id { get; set; }
    public int? NewsId { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Text { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}