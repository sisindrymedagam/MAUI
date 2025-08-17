using Microsoft.EntityFrameworkCore;
using YTShorts.Web.Entities;

namespace YTShorts.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Short> Shorts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Seed a default user (password is hashed for demo: "Password123!")
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Name = "Default Admin",
            Email = "admin@ytshorts.com",
            Password = "$2a$11$p2ycaovI53KAt6CRNJmlVuCz8c8iEwhNUcaGrIspZ1gGNhPHBBiR2", // BCrypt hash for "Password123!"
            CreatedOn = new DateTime(2025, 01, 01),
            CreatedBy = "Seed"
        });
    }
}
