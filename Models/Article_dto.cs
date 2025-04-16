namespace Employee_education_platform.Models;

public class Article_dto
{
    public string article_id { get; set; } = Guid.NewGuid().ToString();
    public string module_id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public object? Attachments { get; set; }
    public int Points { get; set; }
    public float Rating { get; set; }
    public object? Hidden_achievement { get; set; }
}