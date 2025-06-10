using Microsoft.EntityFrameworkCore;
using eep_backend.Models.UserModuleModels;
using eep_backend.Models.GameModuleModels;
using eep_backend.Models.CourseModuleModels;


namespace eep_backend;

public class SiteDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestResult> TestResults { get; set; }
    
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AchievementList> AchievementLists { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<UserCharacter> UserCharacters { get; set; }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    public DbSet<UserAchievement> UserAchievements { get; set; }
    
    
    public SiteDbContext(DbContextOptions<SiteDbContext> options)
        : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=eep_database;Username=Gregulas;Password=234432").UseSnakeCaseNamingConvention();
    //     
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
        modelBuilder.Entity<News>()
            .HasIndex(n => n.Slug)
            .IsUnique();
    }
    
    

}