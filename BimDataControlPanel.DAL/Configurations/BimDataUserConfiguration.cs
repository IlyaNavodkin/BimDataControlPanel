using BimDataControlPanel.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BimDataControlPanel.DAL.Configurations;

public class BimDataUserConfiguration : IEntityTypeConfiguration<BimDataUser>
{
    public void Configure(EntityTypeBuilder<BimDataUser> builder)
    {
        builder.Property(u => u.RevitUserNickName2020)
            .IsRequired()
            .HasDefaultValue("Unknown");

        builder.Property(u => u.RevitUserNickName2022)
            .IsRequired()
            .HasDefaultValue("Unknown");
    }
}