using BarbieQ.Areas.Admin.Models;
using BarbieQ.Models.Entities;
using BarbieQ.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BarbieQ.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductosController : Controller
    {
        ProductosRepository _productosRepos { get; }
        Repository<Categoria> _catRepos { get; }
        public ProductosController(ProductosRepository repos, Repository<Categoria> reposC)
        {
            _productosRepos = repos;
            _catRepos = reposC;
        }
        [HttpGet]
        [HttpPost]
        public IActionResult Index(AdminProductoIndexViewModel vm)
        {
            vm.Categorias = _catRepos.GetAll().Select(x => new CategoriaModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            if (vm.IdCategoria != 0)
            {
                vm.Productos = _productosRepos.GetProductosByCategoria(vm.IdCategoria).Select(x => new ProductoModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Categoria = x.IdCategoriaNavigation.Nombre
                });
            }
            else
            {
                vm.Productos = _productosRepos.GetAll().Select(x => new ProductoModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Categoria = x.IdCategoriaNavigation.Nombre
                });
            }
            return View(vm);
        }

        public IActionResult Agregar()
        {
            AdminAgregarProductoViewModel vm = new()
            {
                Producto = new(),
                Categorias = _catRepos.GetAll().Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                })
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(AdminAgregarProductoViewModel p)
        {
            //validar
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(p.Producto.Nombre))
                ModelState.AddModelError("", "Dale un nombre al maldito producto");
            if (string.IsNullOrWhiteSpace(p.Producto.Descripcion))
                ModelState.AddModelError("", "Debes de describir al maldito producto");
            if (p.Producto.CantidadExistencia < 0 || p.Producto.CantidadExistencia > 1000)
                ModelState.AddModelError("", "La cantidad en existencia debe estar entre 0 y 1000");
            if (p.Producto.IdCategoria == 0)
                ModelState.AddModelError("", "Escoge una maldita categoria");
            if (p.Producto.Precio < 1 || p.Producto.Precio > 10000)
                ModelState.AddModelError("", "Dale un valor al precio entre 1 y 10000");
            //validar archivos
            if (p.ImagenPrincipal != null)
            {
                if (p.ImagenPrincipal.ContentType != "image/jpeg")
                    ModelState.AddModelError("", "Solo se pueden adjuntar imagenes de tipo JPEG");
            }
            else { ModelState.AddModelError("", "Se debe adjuntar una imagen principal del producto"); }
            if (p.ImagenModelo != null)
            {
                if (p.ImagenModelo.ContentType != "image/jpeg")
                    ModelState.AddModelError("", "Solo se pueden adjuntar imagenes de tipo JPEG");
            }
            else { ModelState.AddModelError("", "Se debe adjuntar una imagen del modelo del producto"); }


            if (ModelState.IsValid)
            {
                _productosRepos.Insert(p.Producto);
                if(p.ImagenPrincipal != null )
                {
                    System.IO.FileStream Principal = System.IO.File.Create($"wwwroot/productos/{p.Producto.Id}.jpg");
                    p.ImagenPrincipal.CopyTo(Principal);
                    Principal.Close();
                }
                else
                {
                    System.IO.File.Copy("wwwroot/productos/sin-imagen.jpg", $"wwwroot/productos/{p.Producto.Id}.jpg");
                }
                if(p.ImagenModelo != null)
                {
                    System.IO.FileStream Modelo = System.IO.File.Create($"wwwroot/productos/{p.Producto.Id}_alternate.jpg");
                    p.ImagenModelo.CopyTo(Modelo);
                    Modelo.Close();
                }
                else
                {
                    System.IO.File.Copy("wwwroot/productos/sin-imagen.jpg", $"wwwroot/productos/{p.Producto.Id}_alternate.jpg");
                }
                return RedirectToAction("Index","Home", new {area = "admin"});

            }
            p.Categorias = _catRepos.GetAll().Select(x => new CategoriaModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });


            return View(p);
        }
        public IActionResult Editar(int id)
        {
            var p = _productosRepos.Get(id);
            if (p != null)
            {
               
            }
            return View();
        }
        [HttpPost]
        public IActionResult Editar(Producto p)
        {
            return View();
        }
        public IActionResult Eliminar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Eliminar(Producto p)
        {
            return View();
        }
    }
}
