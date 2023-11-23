using Microsoft.AspNetCore.Mvc;
using BarbieQ.Repositories;
using BarbieQ.Models.ViewModels;

namespace BarbieQ.Controllers
{
    public class HomeController : Controller
    {
        

      


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Carrito()
        {
            return View();
        }
        //public IActionResult Producto(string Id)
        //{
        //    Id = Id.Replace("-", " ");
        //    ProductosViewModel vm = new()
        //    {
        //        Categoria = Id,
        //        Productos = repository.GetProductosByCategoria(Id)
        //        .Select(x => new ProductosModel)
        //        {
        //            Id = x.Id,aaaa
        //            Nombre = x.Nombre ?? "",
        //            Precio = x.Precio ?? 0m
        //        })
        //    };
        //   return View(vm);
        //}
        
    }
}
