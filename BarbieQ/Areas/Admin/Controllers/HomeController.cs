using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarbieQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador, Gerente, Esclavo")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
