namespace BarbieQ.Areas.Admin.Models
{
    public class AdminProductoIndexViewModel
    {
        public int IdCategoria { get; set; }
        public IEnumerable<ProductoModel> Productos { get; set; } = null!;
        public IEnumerable<CategoriaModel> Categorias { get; set; } = null!;
    }
    public class ProductoModel
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = null!;
        public string Categoria { get; set; } = null!;
    }
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
