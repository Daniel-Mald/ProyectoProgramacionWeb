using Microsoft.AspNetCore.Mvc;
using BarbieQ.Repositories;
using BarbieQ.Models.ViewModels;
using BarbieQ.Models.Entities;

namespace BarbieQ.Controllers
{
    public class HomeController : Controller
    {
        public Repository<Producto> productosRepository { get; }
        public Repository<Categorium> categoriassRepository { get; }
        public HomeController(Repository<Producto> pR, Repository<Categorium> cR)
        {
            productosRepository = pR;
            categoriassRepository = cR;
        }




        public IActionResult Index()
        {
            var x = categoriassRepository.GetAll();
            int v = 49;
            IndexViewModel vm = new IndexViewModel()
            {
                Ultimos3Productos = productosRepository.GetAll().Select(x => new ProductosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Precio = x.Precio
                }).Take(3),
                ProductosFavoritos = productosRepository.GetAll().Select(x=> new ProductosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Precio = x.Precio
                }).Take(12).OrderBy(x=> x.Nombre)
            };
            return View(vm);
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
