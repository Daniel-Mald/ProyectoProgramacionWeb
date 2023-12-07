using Microsoft.AspNetCore.Mvc;
using BarbieQ.Repositories;
using BarbieQ.Models.ViewModels;
using BarbieQ.Models.Entities;
using BarbieQ.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BarbieQ.Controllers
{
    public class HomeController : Controller
    {
        public ProductosRepository productosRepository { get; }
        public Repository<Categoria> categoriassRepository { get; }
        public Repository<Cliente> _clienteRepos { get; }
        public HomeController(ProductosRepository pR, Repository<Categoria> cR , Repository<Cliente> clR)
        {
            productosRepository = pR;
            categoriassRepository = cR;
            _clienteRepos = clR;
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
                }).TakeLast(3),
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
        public IActionResult Login()
        {
            LoginViewModel vm = new()
            {
                Mail = "",
                Password = ""
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Mail))
                ModelState.AddModelError("", "Escribe el correo");
            if (string.IsNullOrWhiteSpace(vm.Password))
                ModelState.AddModelError("", "Escribe la contraseña");

            if (ModelState.IsValid)
            {
                var user = _clienteRepos.GetAll().FirstOrDefault(x => x.CorreoElectronico == vm.Mail &&
                x.Contrasena == Encriptacion.StringToSHA512(vm.Password));

                if(user == null)
                {
                    ModelState.AddModelError("", "El correo y/o la contraseña son incorrectos");
                }
                else
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role,Rol(user.Rol)));

                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = false
                    }) ;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return View(vm);
        }
        private string Rol(int id)
        {
            if(id == 1) { return "Administrador"; }
            else if(id == 2) { return "Gerente"; }
            else { return "Esclavo"; }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Index","Home");   
        }
        public IActionResult Denied()
        {
            return View();
        }
    }

}

