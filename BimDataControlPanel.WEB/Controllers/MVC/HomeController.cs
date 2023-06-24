using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.MVC
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
