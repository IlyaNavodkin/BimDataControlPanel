using Microsoft.AspNetCore.Mvc;

namespace BimDataControlPanel.WEB.Controllers.MVC
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        public ViewResult Index() => View();
        public IActionResult Error404() => View();
        public IActionResult DeleteError() => View();
        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied() => View();

    }
}
