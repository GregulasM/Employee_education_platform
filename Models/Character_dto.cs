namespace Employee_education_platform.Models;

public class Character_dto
{
    public string character_id { get; set; } = null!;
    public string user_id { get; set; } = null!;
    public int level { get; set; }
    public int experience { get; set; }
    public object cosmetics { get; set; } = null!;
}