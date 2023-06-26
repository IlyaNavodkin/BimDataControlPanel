using BimDataControlPanel.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BimDataControlPanel.DAL.Configurations;

public class ChangeConfiguration : IEntityTypeConfiguration<Change>
{
    public void Configure(EntityTypeBuilder<Change> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.ChangeTime)
            .IsRequired();

        builder.Property(c => c.Description)
            .IsRequired();

        builder.Property(c => c.ChangeType)
            .IsRequired();
        
        builder.HasOne(c => c.Project)
            .WithMany(p => p.Changes)
            .HasForeignKey(c => c.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.RevitUserInfo)
            .WithMany(p => p.Changes)
            .HasForeignKey(c => c.RevitUserInfoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}