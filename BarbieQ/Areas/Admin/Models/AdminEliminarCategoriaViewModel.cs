using BarbieQ.Models.Entities;

namespace BarbieQ.Areas.Admin.Models
{
    public class AdminEliminarCategoriaViewModel
    {
        public Categoria Categoria { get; set; } = null!;
        public IEnumerable<CategoriaModel> Categorias { get; set; } = null!;
        public int CantidadDeProductos { get; set; }
        public int IdNuevaCategoria { get; set; }
    }
}
