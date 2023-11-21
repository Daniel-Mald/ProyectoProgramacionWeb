using Microsoft.AspNetCore.Mvc;

namespace BarbieQ.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
