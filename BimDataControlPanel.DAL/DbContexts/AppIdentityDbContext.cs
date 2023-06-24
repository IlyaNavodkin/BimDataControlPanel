using BimDataControlPanel.DAL.Configurations;
using BimDataControlPanel.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BimDataControlPanel.DAL.DbContexts
{
    public class AppIdentityDbContext : IdentityDbContext<BimDataUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BimDataUserConfiguration());
            base.OnModelCreating(builder);
        }
    }
}