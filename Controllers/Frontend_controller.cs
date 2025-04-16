using Employee_education_platform.Models;
using Employee_education_platform.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employee_education_platform.Controllers;

/// <summary>
/// Контроллер для фронтенда, тут будем чисто выгружать данные.
/// </summary>
[ApiController]
[Route("api/frontend")]
public class Frontend_controller : ControllerBase
{
    private readonly SurrealDbService _db;

    public Frontend_controller(SurrealDbService db)
    {
        _db = db;
    }

    [HttpGet("courses")]
    public async Task<IActionResult> Get_courses()
    {
        var courses = await _db.SelectAll<Course_dto>("Course");
        return Ok(courses);
    }

    [HttpGet("courses/{course_id}")]
    public async Task<IActionResult> Get_course_details(string course_id)
    {
        string fullCourseId = $"course:{course_id}";
        var course = await _db.SelectById<Course_dto>("Course", fullCourseId);
        var modules = await _db.RunQuery<Module_dto>($"SELECT * FROM Module WHERE course_id = '{course_id}'");
        var result = new { course, modules };
        return Ok(result);
    }

    [HttpGet("users/{user_id}/profile")]
    public async Task<IActionResult> Get_user_profile(string user_id)
    {
        var user = await _db.SelectById<User_dto>("User", user_id);
        if (user == null) return NotFound("Пользователь не найден");

        var character = await _db.RunQuery<Character_dto>($"SELECT * FROM Character WHERE user_id = '{user_id}'");
        var courseProgress = await _db.RunQuery<CourseProgress_dto>($"SELECT * FROM CourseProgress WHERE user_id = '{user_id}'");
        var userAchievements = await _db.RunQuery<UserAchievement_dto>($"SELECT * FROM UserAchievement WHERE user_id = '{user_id}'");

        var result = new
        {
            user,
            character,
            course_progress = courseProgress,
            achievements = userAchievements
        };

        return Ok(result);
    }
}