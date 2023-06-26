using BimDataControlPanel.BLL.Services;
using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BimDataControlPanel.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBll(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<ChangeService>();
            services.AddTransient<ProjectService>();
            services.AddTransient<RevitUserInfoService>();
            
            return services;
        }
    }
}