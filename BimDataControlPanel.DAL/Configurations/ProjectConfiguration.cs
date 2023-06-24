using BimDataControlPanel.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BimDataControlPanel.DAL.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.RevitVersion)
            .IsRequired();

        builder.Property(p => p.CreationTime)
            .IsRequired();

        builder.Property(p => p.Complete)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasMany(p => p.Changes)
            .WithOne(c => c.Project)
            .HasForeignKey(c => c.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.ProjectUsers)
            .WithOne()
            .HasForeignKey(pu => pu.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}