using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.WEB.Constants;
using BimDataControlPanel.WEB.ViewModels.IdentityUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.MVC
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private UserManager<BimDataUser> _userManager;
        private SignInManager<BimDataUser> _signInManager;
        public AccountController(UserManager<BimDataUser> userMrg, SignInManager<BimDataUser> signInMrg)
        {
            _userManager = userMrg;
            _signInManager = signInMrg;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(viewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким адресом электронной почты уже зарегистрирован.");
                    return View(viewModel);
                }

                var user = new BimDataUser { Email = viewModel.Email, UserName = viewModel.Name };
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RolesConstants.UsersRoleName);

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(viewModel);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.Name);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var signIn = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);

                    if (signIn.Succeeded)
                    {
                        return Redirect("/");
                    }
                }
            }

            ModelState.AddModelError("", "Неверный логин или пароль");

            return View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<RedirectToActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
