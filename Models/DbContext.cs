using System.Numerics;
using api.Models;
using Microsoft.EntityFrameworkCore;


public class InMemoryDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseInMemoryDatabase(databaseName: "UsersDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskModel>().Property(e => e.Id).ValueGeneratedNever();
    }

    public DbSet<UserModel> Users{ get; set; }
}

