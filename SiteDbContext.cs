using Microsoft.EntityFrameworkCore;
using eep_backend.Models.UserModuleModels;

namespace eep_backend;

public class SiteDbContext : DbContext
{
    public SiteDbContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=Gregulas;Password=234432");
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }
    
    

}