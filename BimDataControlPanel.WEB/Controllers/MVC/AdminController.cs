using BimDataControlPanel.DAL.DbContexts;
using BimDataControlPanel.WEB.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.MVC
{
    [Authorize(Roles = RolesConstants.AdminRoleName)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public AdminController(AppIdentityDbContext context)
        {
            _context = context;
        }
        public ViewResult Index() => View();
    }
}
