using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.DAL.Exeptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BimDataControlPanel.DAL.Extensions;

public static class UserManagerExtension
{
    public static async Task<IEnumerable<BimDataUser>> GetUsersByProjectAsync(this UserManager<BimDataUser> userManager,
        Project project)
    {
        var projectUserIds = project.ProjectUsers.Select(i => i.IdentityUserId);
        var projectUsers = new List<BimDataUser>();
        
        foreach (var id in projectUserIds)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null) continue;
            projectUsers.Add(user);
        }

        return projectUsers;
    }
    
    public static async Task<BimDataUser> GetUserByChangeAsync(this UserManager<BimDataUser> userManager,
        Change change)
    {
        var userRevitName = change.UserRevitName;
        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.RevitUserNickName2020 == userRevitName ||
                                 u.RevitUserNickName2022 == userRevitName);

        if (user == null)
        {
            var entityPropertyNames = $"{nameof(BimDataUser.RevitUserNickName2020)} " +
                                      $"or {nameof(BimDataUser.RevitUserNickName2022)}";
            throw new NotFoundPropertyException(nameof(BimDataUser), 
                entityPropertyNames, userRevitName);
            
        }

        return user;
    }
}