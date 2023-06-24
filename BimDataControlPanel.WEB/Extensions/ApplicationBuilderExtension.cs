using BimDataControlPanel.BLL.Services;
using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Extensions;
using BimDataControlPanel.WEB.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BimDataControlPanel.WEB.Extensions;

public static class ApplicationBuilderExtension
{
    public static async Task IdentityEnsurePopulated(this IApplicationBuilder app)
    {
        var userManager = app.ApplicationServices.GetRequiredService<UserManager<BimDataUser>>();
        var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();

        var users = new[]
        {
            new
            {
                UserName = UsersConstants.AdminName,
                Email = UsersConstants.AdminEmail,
                Password = UsersConstants.AdminPassword,
                Role = RolesConstants.AdminRoleName
            },
            new
            {
                UserName = UsersConstants.User1Name,
                Email = UsersConstants.User1Email,
                Password = UsersConstants.User1Password,
                Role = RolesConstants.UsersRoleName 
            },
            new
            {
                UserName = UsersConstants.User2Name,
                Email = UsersConstants.User2Email,
                Password = UsersConstants.User2Password,
                Role = RolesConstants.UsersRoleName 
            }
        };

        foreach (var user in users)
        {
            var existingUser = await userManager.FindByNameAsync(user.UserName);
            if (existingUser == null)
            {
                existingUser = new BimDataUser { UserName = user.UserName, Email = user.Email };
                var result = await userManager.CreateAsync(existingUser, user.Password);
            }

            if (user.Role != null)
            {
                var role = await roleManager.FindByNameAsync(user.Role);
                if (role == null)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(user.Role));
                }

                var userRoles = await userManager.GetRolesAsync(existingUser);
                if (!userRoles.Contains(user.Role))
                {
                    await userManager.AddToRoleAsync(existingUser, user.Role);
                }
            }
        }
    }    
    public static async void DataEnsurePopulated(this IApplicationBuilder app)
    {
        var userManager = app.ApplicationServices.GetRequiredService<UserManager<BimDataUser>>();
        var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();
        var context = app.ApplicationServices.GetRequiredService<AppDbContext>();
        var logger = app.ApplicationServices.GetRequiredService<ILogger<IApplicationBuilder>>();
        
        var user = await userManager.FindByNameAsync(UsersConstants.AdminName);
        var user1 = await userManager.FindByNameAsync(UsersConstants.User1Name);
        var user2 = await userManager.FindByNameAsync(UsersConstants.User2Name);
        
        if (context.Projects.ToList().Count == 0)
        {
            var project1 = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Project 1",
                RevitVersion = "2022",
                CreationTime = DateTime.Now,
                Complete = false
            };
            context.Projects.Add(project1);

            var project2 = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Project 2",
                RevitVersion = "2020",
                CreationTime = DateTime.Now,
                Complete = true
            };
            context.Projects.Add(project2);

            var change1 = new Change
            {
                Id = Guid.NewGuid().ToString(),
                UserRevitName = user.RevitUserNickName2022,
                ChangeTime = DateTime.Now,
                Description = "Change 1",
                ChangeType = "Type 1",
                ProjectId = project1.Id
            };
            context.Changes.Add(change1);

            var change2 = new Change
            {
                Id = Guid.NewGuid().ToString(),
                UserRevitName = user.RevitUserNickName2020,
                ChangeTime = DateTime.Now,
                Description = "Change walls",
                ChangeType = "Add",
                ProjectId = project2.Id
            };
            context.Changes.Add(change2);

            var change3 = new Change
            {
                Id = Guid.NewGuid().ToString(),
                UserRevitName = user1.RevitUserNickName2020,
                ChangeTime = DateTime.Now,
                Description = "Change ducts",
                ChangeType = "Add",
                ProjectId = project2.Id
            };
            context.Changes.Add(change3);

            var change4 = new Change
            {
                Id = Guid.NewGuid().ToString(),
                UserRevitName = user2.RevitUserNickName2020,
                ChangeTime = DateTime.Now,
                Description = "Change blaks",
                ChangeType = "Add",
                ProjectId = project2.Id
            };
            context.Changes.Add(change4);

            var projectUser1 = new ProjectUser
            {
                ProjectId = project1.Id,
                IdentityUserId = user.Id
            };
            context.ProjectUsers.Add(projectUser1);

            var projectUser2 = new ProjectUser
            {
                ProjectId = project2.Id,
                IdentityUserId = user.Id
            };
            context.ProjectUsers.Add(projectUser2);
            
            var projectUser3 = new ProjectUser
            {
                ProjectId = project2.Id,
                IdentityUserId = user1.Id
            };
            context.ProjectUsers.Add(projectUser3);
            
            var projectUser4 = new ProjectUser
            {
                ProjectId = project2.Id,
                IdentityUserId = user2.Id
            };
            context.ProjectUsers.Add(projectUser4);
            
            context.SaveChanges();
        }
    }
}