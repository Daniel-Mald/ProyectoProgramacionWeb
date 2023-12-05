using BarbieQ.Models.Entities;

namespace BarbieQ.Areas.Admin.Models
{
    public class AdminAgregarProductoViewModel
    {
        public Producto Producto { get; set; } = null!;
        public IEnumerable<CategoriaModel> Categorias { get; set; } = null!;
        public IFormFile? ImagenPrincipal { get; set; }
        public IFormFile? ImagenModelo { get; set; }
    }
}
