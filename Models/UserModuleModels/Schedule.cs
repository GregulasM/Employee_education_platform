using SurrealDb.Net.Models;

namespace eep_backend.Models.UserModuleModels;

public class Schedule
{
    public RecordIdOfString id { get; set; }          // SurrealDB id

    public string day_of_week { get; set; }           // 'Понедельник', 'Вторник', и т.д.
    public string? time_slot { get; set; }             // '8:30-10:00'
    public string? subject { get; set; }               // 'Middleware в ASP'
    public string? teacher { get; set; }               // 'Крутой Е.И.'
    public string? details { get; set; }               // 'Совещательная, каб. 21, 2 этаж'
    public RecordIdOfString? course { get; set; }      // связь с курсом
    public RecordIdOfString? module { get; set; }      // связь с модулем
    public RecordIdOfString? department { get; set; }  // связь с отделом
    public RecordIdOfString? user { get; set; }        // связь с пользователем (если нужно уникальное для работника)
    public bool? is_active { get; set; }               // Активна ли запись
}