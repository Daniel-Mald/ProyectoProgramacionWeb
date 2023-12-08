namespace BarbieQ.Models.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public IEnumerable<ProductosModel> Productos { get; set; } = null!;
        public int CantidadProductos { get; set; } 
    }
}
