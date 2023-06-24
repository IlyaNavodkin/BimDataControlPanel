using System.Security.Claims;
using System.Security.Principal;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.WEB.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Components;

public class AuthenticationViewComponent : ViewComponent
{
    private readonly UserManager<BimDataUser> _userManager;
    private readonly SignInManager<BimDataUser> _signInManager;

    public AuthenticationViewComponent(UserManager<BimDataUser> userManager, SignInManager<BimDataUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!_signInManager.IsSignedIn(HttpContext.User))
        {
            return View("Unauthenticated");
        }

        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        var isAdmin = await _userManager.IsInRoleAsync(currentUser, RolesConstants.AdminRoleName);
        var roles = await _userManager.GetRolesAsync(currentUser);
        
        ViewBag.UserName = currentUser.UserName;
        ViewBag.Roles = roles;
        
        return View("Authenticated", isAdmin);
    }
}