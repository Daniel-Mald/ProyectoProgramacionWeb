using BarbieQ.Areas.Admin.Models;
using BarbieQ.Models.Entities;
using BarbieQ.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarbieQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador, Gerente")]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        CategoriaRepository _catRepos { get; }
        ProductosRepository _prodRepos { get; } 
        public CategoriasController(CategoriaRepository repos ,ProductosRepository repos2)
        {
            _catRepos = repos; 
            _prodRepos = repos2;
        }
        public IActionResult Index()
        {
            IEnumerable<AdminIndexCategoriaViewModel> vm = _catRepos.GetAll().Select(x => new AdminIndexCategoriaViewModel
            {
                Id = x.Id,
                Nombre = x.Nombre,
                NumeroDeProductos = x.Producto.Count()
            });
            
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {
            AdminAgregarCategoriaViewModel vm = new()
            {
                Nombre = "",
                Descripcion = "",

            };
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Agregar(AdminAgregarCategoriaViewModel vm)
        {
            //validar
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.Nombre))
                ModelState.AddModelError("", "Agrega un nombre para la categoria");
            if (string.IsNullOrWhiteSpace(vm.Descripcion))
                ModelState.AddModelError("", "Agrega una descripcion para la categoria");
            if(vm.Imagen != null)
            {
                if (vm.Imagen.ContentType != "image/jpeg")
                    ModelState.AddModelError("", "Solo se permiten imagenes en formato jpg y jpeg");
            }
            else { ModelState.AddModelError("", "Debes de agregar una imagen para la categoria"); }

            //Si es valido
            if (ModelState.IsValid)
            {
                var c = new Categoria
                {
                    Descripcion = vm.Descripcion,
                    Nombre = vm.Nombre,

                };
                _catRepos.Insert(c);
                if (vm.Imagen != null)
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/img/nav/{c.Nombre}.jpg");
                    vm.Imagen.CopyTo(fs);
                    fs.Close();
                }
                else
                {
                    System.IO.File.Copy("wwwroot/img/productos/sin-imagen.jpg", $"wwwroot/img/nav/{c.Nombre}.jpg");
                }
                return RedirectToAction("Index", "Home", new { area = "admin" });
            }
            //Si no lo es
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var cat = _catRepos.Get(id);
            if(cat == null) { return RedirectToAction("Index"); }

            AdminEditarCategoriaViewModel vm = new()
            {
                Nombre = cat.Nombre,
                Descripcion = cat.Descripcion,
                Id = cat.Id

            };
           
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Editar(AdminEditarCategoriaViewModel vm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.Nombre))
                ModelState.AddModelError("", "Agrega un nombre para la categoria");
            if (string.IsNullOrWhiteSpace(vm.Descripcion))
                ModelState.AddModelError("", "Agrega una descripcion para la categoria");
            if (vm.Imagen != null)
            {
                if (vm.Imagen.ContentType != "image/jpeg")
                    ModelState.AddModelError("", "Solo se permiten imagenes en formato jpg y jpeg");
            }
            if (ModelState.IsValid)
            {
                var cat = _catRepos.Get(vm.Id);
                if(cat == null) { return RedirectToAction("Index"); }
                cat.Nombre = vm.Nombre;
                cat.Descripcion = vm.Descripcion;
                _catRepos.Update(cat);
                if(vm.Imagen != null)
                {
                    System.IO.FileStream filestream = System.IO.File.Create($"wwwroot/img/nav/{cat.Nombre}.jpg");
                    vm.Imagen.CopyTo(filestream);
                    filestream.Close();
                }
                return RedirectToAction("Index", "Home", new { area = "admin" });

            }
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            var cat = _catRepos.GetById(id);
            if(cat == null)
            {
                return RedirectToAction("Index");
            }
            AdminEliminarCategoriaViewModel vm = new()
            {
                Categoria = cat,
                Categorias = _catRepos.GetAll().Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre

                }),
                //CantidadDeProductos = cat.Producto.Count
                // CantidadDeProductos = _prodRepos.GetAll().Where(x => x.IdCategoria == cat.Id).Count()
                CantidadDeProductos = cat.Producto.Count
            };
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Eliminar(AdminEliminarCategoriaViewModel c)
        {
            var cat = _catRepos.GetById(c.Categoria.Id);
            if(cat == null) 
            {
                
                return RedirectToAction("Index"); 
            }
            ModelState.Clear();
            if (c.CantidadDeProductos > 0 && c.IdNuevaCategoria == 0)
            {
                ModelState.AddModelError("", "Esta categoria tiene productos, no la puedes eliminar");                
            }
                
            if (ModelState.IsValid)
            {
                if(c.CantidadDeProductos >0)
                {
                    foreach (var item in cat.Producto.ToList())
                    {
                        item.IdCategoria = c.IdNuevaCategoria;
                        _prodRepos.Update(item);
                    }
                }
                 _catRepos.Delete(cat);
                string ruta = $"wwwroot/img/nav/{cat.Nombre}.jpg";
                if (System.IO.File.Exists(ruta))
                {
                    System.IO.File.Delete(ruta);
                }
                return RedirectToAction("Index", "Home", new { area = "admin" });

            }
            c.Categoria = cat;
            c.CantidadDeProductos = cat.Producto.Count;
            c.Categorias = _catRepos.GetAll().Select(x => new CategoriaModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            return View(c);
        }
    }
}
