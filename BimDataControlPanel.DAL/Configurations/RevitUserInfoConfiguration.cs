using BimDataControlPanel.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BimDataControlPanel.DAL.Configurations;

public class RevitUserInfoConfiguration : IEntityTypeConfiguration<RevitUserInfo>
{
    public void Configure(EntityTypeBuilder<RevitUserInfo> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.RevitVersion)
            .IsRequired();

        builder.Property(p => p.NameUserOs)
            .IsRequired();

        builder.Property(p => p.DsToolsVersion)
            .IsRequired();

        builder.Property(p => p.LastConnection)
            .IsRequired();
        
        builder.HasMany(p => p.Changes)
            .WithOne(c => c.RevitUserInfo)
            .HasForeignKey(c => c.RevitUserInfoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(r => r.Projects)
            .WithMany(p => p.RevitUserInfos);
    }
}