using BimDataControlPanel.BLL.Dtos.Change;
using BimDataControlPanel.BLL.Dtos.Project;
using BimDataControlPanel.BLL.Dtos.RevitUserInfo;
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
        var projectService = app.ApplicationServices.GetRequiredService<ProjectService>();
        var changeService = app.ApplicationServices.GetRequiredService<ChangeService>();
        var revitUserInfoService = app.ApplicationServices.GetRequiredService<RevitUserInfoService>();

        var context = app.ApplicationServices.GetRequiredService<AppDbContext>();

        var projectCount = context.Projects.ToArray().Length;
        if (projectCount == 0)
        {
            var userInfo1Dto = new CreateRevitUserInfoDto
            {
                Name = "User 1",
                LastConnection = DateTime.Now,
                NameUserOs = "User1",
                RevitVersion = "2020",
                DsToolsVersion = "1.0",
            };
            
            var userInfo2Dto = new CreateRevitUserInfoDto
            {
                Name = "User 2",
                LastConnection = DateTime.Now,
                NameUserOs = "User2",
                RevitVersion = "2022",
                DsToolsVersion = "2.0",
            };
            
            var userInfo1Id = await revitUserInfoService.Create(userInfo1Dto);
            var userInfo2Id = await revitUserInfoService.Create(userInfo2Dto);
            
            var userInfo1 = await revitUserInfoService.GetById(userInfo1Id);
            var userInfo2 = await revitUserInfoService.GetById(userInfo2Id);
            
            var project1Dto = new CreateProjectDto
            {
                Name = "Project 1",
                RevitVersion = "2022",
                Complete = false,
            };

            var project2Dto = new CreateProjectDto
            {
                Name = "Project 2",
                RevitVersion = "2020",
                Complete = true
            };
            
            var project1Id = await projectService.Create(project1Dto);
            var project2Id = await projectService.Create(project2Dto);
            
            var project1 = await projectService.GetById(project1Id);
            var project2 = await projectService.GetById(project2Id);
            
            project1.RevitUserInfos.Add(userInfo1);
            project1.RevitUserInfos.Add(userInfo2);
            project2.RevitUserInfos.Add(userInfo2);
            
            var change1Dto = new CreateChangeDto()
            {
                ChangeTime = DateTime.Now,
                Description = "Change 1",
                ChangeType = "Type 1",
                ProjectId = project1Id,
                RevitUserInfoId = userInfo1Id
            };
            
            var change2Dto = new CreateChangeDto
            {
                ChangeTime = DateTime.Now,
                Description = "Change walls",
                ChangeType = "Add",
                ProjectId = project2Id,
                RevitUserInfoId = userInfo1Id
            };
            
            var change3Dto = new CreateChangeDto
            {
                ChangeTime = DateTime.Now,
                Description = "Change ducts",
                ChangeType = "Add",
                ProjectId = project2Id,
                RevitUserInfoId = userInfo2Id
            };
            
            var change4Dto = new CreateChangeDto
            {
                ChangeTime = DateTime.Now,
                Description = "Change blanks",
                ChangeType = "Add",
                ProjectId = project2Id,
                RevitUserInfoId = userInfo2Id
            };
            
            var change1Id = await changeService.Create(change1Dto);
            var change2Id = await changeService.Create(change2Dto);
            var change3Id = await changeService.Create(change3Dto);
            var change4Id = await changeService.Create(change4Dto);

            var allProjects = await projectService.GetAll();
            var allChanges = await changeService.GetAll();
            var allInfos = await revitUserInfoService.GetAll();
            
            context.SaveChanges();
        }
    }
}