using Microsoft.AspNetCore.Mvc;
using BarbieQ.Repositories;
using BarbieQ.Models.ViewModels;
using BarbieQ.Models.Entities;

namespace BarbieQ.Controllers
{
    public class HomeController : Controller
    {
        public ProductosRepository productosRepository { get; }
        public Repository<Categoria> categoriassRepository { get; }
        public HomeController(ProductosRepository pR, Repository<Categoria> cR)
        {
            productosRepository = pR;
            categoriassRepository = cR;
        }




        public IActionResult Index()
        {
           
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
        public IActionResult VerCategoria(string Id)  // Id es el nombre de la categoria
        {
            Id = Id.Replace("-", " ");
            ProductosViewModel vm = new()
            {
                Categoria = Id,
                Productos = productosRepository.GetProductosByCategoria(Id)
                .Select(x => new ProductosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre?? "",
                    Precio = x.Precio
                })
            };
            return View(vm);
        }
        public IActionResult VerProducto(string Id)
        {
            Id = Id.Replace("-", " ");
            var producto = productosRepository.GetByNombre(Id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }

            VerProductosViewModel vm = new()
            {
                Id = producto.Id,
                Categoria = producto.IdCategoriaNavigation?.Nombre ?? "",
                Descripcion = producto.Descripcion ?? "",
                Precio = producto.Precio,
                Nombre = producto.Nombre ?? ""
            };
            return View(vm);
        }
    }

}

