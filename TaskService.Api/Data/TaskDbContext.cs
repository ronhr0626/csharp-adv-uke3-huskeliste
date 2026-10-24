using Microsoft.EntityFrameworkCore;
using TaskService.Api.Models;

namespace TaskService.Api.Data;

public class TaskDbContext : DbContext
{
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users { get; set; }

    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Two users can not have the same username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
