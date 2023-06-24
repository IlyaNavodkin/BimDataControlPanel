using BimDataControlPanel.BLL;
using BimDataControlPanel.DAL;
using BimDataControlPanel.DAL.Configurations;
using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.WEB.Extensions;
using BimDataControlPanel.WEB.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BimDataControlPanel.WEB
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddDal(_configuration);
            services.AddBll(_configuration);
                        
            services.AddSwaggerGen();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            
            services.AddMemoryCache();
            services.AddSession();

            services.AddIdentity<BimDataUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            
            app.UseStatusCodePages(async context =>
            {
                var statusCode = context.HttpContext.Response.StatusCode;
                if (statusCode == 404)
                {
                    context.HttpContext.Response.Redirect("/Error/Error404");
                }
                else if (statusCode == 500)
                {
                    context.HttpContext.Response.Redirect("/Error/Index");
                }
                else if (statusCode == 403)
                {
                    context.HttpContext.Response.Redirect("/Error/AccessDenied");
                }
            });
            
            app.UseRouting(); 

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });
            
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseSession();
            app.UseStaticFiles();
            
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/index.html", context =>
                {
                    context.Response.Redirect("/Home/Index");
                    return Task.CompletedTask;
                });
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
            
            app.IdentityEnsurePopulated();
            app.DataEnsurePopulated();
        }
    }
}
