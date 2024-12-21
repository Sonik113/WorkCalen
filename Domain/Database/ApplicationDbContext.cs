using Microsoft.EntityFrameworkCore;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;

namespace WorkCalendarik.Domain.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserDb> UsersDb { get; set; }
    public DbSet<BronCalendarDb> BronCalendarsDb { get; set; }
    
    private readonly IConfiguration? Configuration;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BronCalendarDb>()
            .ToTable("broncalendars2");
        modelBuilder.Entity<UserDb>()
            .ToTable("users");
    }

    public ILoggerFactory CreateLoggerFactory() => 
        LoggerFactory.Create(builder => {builder.AddConsole();});
}