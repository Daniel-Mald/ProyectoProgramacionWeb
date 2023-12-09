using Microsoft.AspNetCore.Mvc;
using BarbieQ.Repositories;
using BarbieQ.Models.ViewModels;
using BarbieQ.Models.Entities;
using BarbieQ.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace BarbieQ.Controllers
{
    public class HomeController : Controller
    {
        public ProductosRepository productosRepository { get; }
        public CategoriaRepository categoriassRepository { get; }
        public Repository<Cliente> _clienteRepos { get; }
        public HomeController(ProductosRepository pR, CategoriaRepository cR, Repository<Cliente> clR)
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
                ProductosFavoritos = productosRepository.GetAll().Select(x => new ProductosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Precio = x.Precio
                }).Take(12).OrderBy(x => x.Nombre),
                TotalCarrito = GetTotalCarrito()
            };


            return View(vm);
        }



        public IActionResult VerCategoria(string Id)  // Id es el nombre de la categoria
        {
            Id = Id.Replace("-", " ");
            var cat = categoriassRepository.GetByNombre(Id);
            if (cat == null) { return RedirectToAction("Index"); }
            CategoriaViewModel vm = new()
            {
                Nombre = cat.Nombre,
                Descripcion = cat.Descripcion,
                Id = cat.Id,
                CantidadProductos = cat.Producto.Count,
                Productos = cat.Producto.Select(x => new ProductosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Precio = x.Precio
                }).OrderBy(x => x.Nombre)
            };
            Random r = new Random();

            ViewBag.banner = r.Next(1, 6);

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
                Ingredientes = producto.Ingredientes,
                CantidadEnExistencia = producto.CantidadExistencia != null ? (int)producto.CantidadExistencia : 0,
                //Categoria = producto.IdCategoriaNavigation?.Nombre ?? "",
                Descripcion = producto.Descripcion ?? "",
                Precio = producto.Precio,
                Nombre = producto.Nombre ?? "",
                Productos = productosRepository.GetAll().Select(x => new ProductosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Precio = x.Precio
                }).Take(4)
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

                if (user == null)
                {
                    ModelState.AddModelError("", "El correo y/o la contraseña son incorrectos");
                }
                else
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role, Rol(user.Rol)));

                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = false
                    });
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return View(vm);
        }
        private string Rol(int id)
        {
            if (id == 1) { return "Administrador"; }
            else if (id == 2) { return "Gerente"; }
            else { return "Esclavo"; }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Denied()
        {
            return View();
        }


        #region Carrit de compras


        [HttpPost]
        public IActionResult CambiarCantidadArticulo([FromBody] CarritoViewModel carritoItem)
        {
            // Verificar si existe una cookie de carrito
            if (Request.Cookies["ShoppingCart"] != null)
            {
                // Obtener el carrito desde la cookie
                List<CarritoViewModel>? carritoItems = JsonConvert.DeserializeObject<List<CarritoViewModel>>(Request.Cookies["ShoppingCart"]);

                // Verificar si el producto ya está en el carrito
                CarritoViewModel? existingItem = carritoItems.FirstOrDefault(item => item.Id == carritoItem.Id);
                if (existingItem != null)
                {
                    // Actualizar la cantidad si el producto ya está en el carrito
                    existingItem.Cantidad = carritoItem.Cantidad;
                }
                // Guardar el carrito en la cookie
                string carritoJson = JsonConvert.SerializeObject(carritoItems);
                Response.Cookies.Append("ShoppingCart", carritoJson, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                });

                var cantidadActual = carritoItems.Sum(x => x.Cantidad);
                var totalPagar = GetTotalPagar(carritoItems);

                return Ok(new { CantidadActual = cantidadActual, TotalPagar = totalPagar });
            }

            // Redirigir a la acción Index del controlador Cart para mostrar el carrito actualizado
            return Ok();
        }

        private int GetTotalCarrito()
        {

            var total = 0;

            if (Request.Cookies["ShoppingCart"] != null)
            {

                List<CarritoViewModel>? carritoItems = JsonConvert.DeserializeObject<List<CarritoViewModel>>(Request.Cookies["ShoppingCart"]);
                if (carritoItems != null)
                    total = carritoItems.Sum(x => x.Cantidad);

            }
            
            return total;   
        }


        [HttpPost]
        public IActionResult EliminarDelCarrito(int productId)
        {
            // Verificar si existe una cookie de carrito
            if (Request.Cookies["ShoppingCart"] != null)
            {
                // Obtener el carrito desde la cookie
                List<CarritoViewModel>? carritoItems = JsonConvert.DeserializeObject<List<CarritoViewModel>>(Request.Cookies["ShoppingCart"]);

                // Buscar y eliminar el artículo del carrito
                CarritoViewModel? itemToRemove = carritoItems.FirstOrDefault(item => item.Id == productId);
                if (itemToRemove != null)
                {
                    carritoItems.Remove(itemToRemove);

                    // Guardar el carrito actualizado en la cookie
                    string carritoJson = JsonConvert.SerializeObject(carritoItems);
                    Response.Cookies.Append("ShoppingCart", carritoJson, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddMonths(1)
                    });
                }

                var cantidadActual = carritoItems.Sum(x => x.Cantidad);
                var totalPagar = GetTotalPagar(carritoItems);
                
                 return Ok(new { CantidadActual = cantidadActual, TotalPagar = totalPagar});
            }

            // Puedes redirigir a donde necesites, por ejemplo, a la página del carrito
            return Ok();
        }



        [HttpPost]
        public IActionResult Comprar()
        {
            // Verificar si existe una cookie de carrito
            if (Request.Cookies["ShoppingCart"] != null)
            {
                // Obtener el carrito desde la cookie
                List<CarritoViewModel>? carritoItems = JsonConvert.DeserializeObject<List<CarritoViewModel>>(Request.Cookies["ShoppingCart"]);

                // Limpiar la lista eliminando todos los elementos
                carritoItems.Clear();

                // Guardar la lista vacía en la cookie
                string carritoJson = JsonConvert.SerializeObject(carritoItems);
                Response.Cookies.Append("ShoppingCart", carritoJson, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                });

                // Devolver un objeto indicando que la lista ha sido eliminada
                return Ok(new { ListaEliminada = true });
            }

            // Devolver un error si no hay cookie de carrito
            return BadRequest("No se encontró el carrito");
        }



        [HttpPost]
        public IActionResult AgregarAlCarrito([FromBody] CarritoViewModel carritoItem)
        {
            // Verificar si existe una cookie de carrito
            if (Request.Cookies["ShoppingCart"] != null)
            {
                // Obtener el carrito desde la cookie
                List<CarritoViewModel>? carritoItems = JsonConvert.DeserializeObject<List<CarritoViewModel>>(Request.Cookies["ShoppingCart"]);

                // Verificar si el producto ya está en el carrito
                CarritoViewModel? existingItem = carritoItems.FirstOrDefault(item => item.Id == carritoItem.Id);
                if (existingItem != null)
                {
                    // Actualizar la cantidad si el producto ya está en el carrito
                    existingItem.Cantidad += carritoItem.Cantidad;
                }
                else
                {
                    // Agregar un nuevo artículo al carrito
                    CarritoViewModel newItem = new CarritoViewModel
                    {
                        Id = carritoItem.Id,
                        NombreProducto = carritoItem.NombreProducto.Replace("-", " ").ToUpper(),
                        Precio = carritoItem.Precio,
                        Cantidad = carritoItem.Cantidad
                    };
                    carritoItems.Add(newItem);
                }

                // Guardar el carrito en la cookie
                string carritoJson = JsonConvert.SerializeObject(carritoItems);
                Response.Cookies.Append("ShoppingCart", carritoJson, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                });
            }
            else
            {
                // Si no existe una cookie de carrito, crear una nueva
                List<CarritoViewModel> carritoItems = new List<CarritoViewModel>
            {
                new CarritoViewModel
                {
                      Id = carritoItem.Id,
                        NombreProducto = carritoItem.NombreProducto.Replace("-"," ").ToUpper(),
                        Precio = carritoItem.Precio,
                        Cantidad = carritoItem.Cantidad
                }
            };

                // Guardar el carrito en la cookie
                string carritoJson = JsonConvert.SerializeObject(carritoItems);
                Response.Cookies.Append("ShoppingCart", carritoJson, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                });
            }

            // Redirigir a la acción Index del controlador Cart para mostrar el carrito actualizado
            return Ok();
        }

        public IActionResult Carrito()
        {

            ListaArticulos vm = new(); 

            if (Request.Cookies["ShoppingCart"] != null)
            {
                // Obtener el carrito desde la cookie
                List<CarritoViewModel>? carritoItems = JsonConvert.DeserializeObject<List<CarritoViewModel>>(Request.Cookies["ShoppingCart"]);

                vm.Carrito = carritoItems;
                vm.TotalProductos = carritoItems.Sum(x => x.Cantidad);
                vm.TotalPagar = GetTotalPagar(carritoItems);
            }
            

            return View(vm);
        }

        private decimal GetTotalPagar(List<CarritoViewModel> carritoItems)
        {
            decimal total = 0;
            foreach (var articulo in carritoItems)
            {
                total += articulo.Cantidad * articulo.Precio;
            }
            return total;
        }
        #endregion
    }


}

