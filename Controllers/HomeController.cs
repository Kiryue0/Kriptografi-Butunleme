using Microsoft.AspNetCore.Mvc;

namespace KriptografiWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
