using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BimDataControlPanel.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDal(this IServiceCollection services,
            IConfiguration configuration)
        {
            var identityConnectionString = configuration["DbConnectionIdentity"];
            var connectionString = configuration["DbConnection"];
            
            services.AddTransient<IPasswordValidator<BimDataUser>,
                CustomPasswordValidator>(serv => new CustomPasswordValidator(6));

            services.AddTransient<IUserValidator<BimDataUser>, CustomUserValidator>();
            services.AddScoped<ChangeValidator>();
            services.AddScoped<ProjectValidator>();
            services.AddScoped<RevitUserInfoValidator>();

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlite(identityConnectionString, b => 
                    b.MigrationsAssembly("BimDataControlPanel.WEB")));
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString, b => 
                    b.MigrationsAssembly("BimDataControlPanel.WEB")));
            
            return services;
        }
    }
}