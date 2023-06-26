using System.Reflection;
using BimDataControlPanel.DAL.Configurations;
using BimDataControlPanel.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BimDataControlPanel.DAL.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<RevitUserInfo> RevitUserInfos { get; set; }
    public DbSet<Change> Changes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChangeConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new RevitUserInfoConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}