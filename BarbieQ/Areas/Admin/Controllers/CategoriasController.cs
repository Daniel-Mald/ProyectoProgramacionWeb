using Microsoft.AspNetCore.Mvc;

namespace BarbieQ.Areas.Admin.Controllers
{
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
