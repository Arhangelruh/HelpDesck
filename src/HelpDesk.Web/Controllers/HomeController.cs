using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Web.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Main page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }
    }
}