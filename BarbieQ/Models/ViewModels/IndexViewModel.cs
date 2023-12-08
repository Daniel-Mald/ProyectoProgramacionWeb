namespace BarbieQ.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<ProductosModel> Ultimos3Productos { get; set; } = null!;
        public IEnumerable<ProductosModel> ProductosFavoritos { get; set; } = null!;
        public int TotalCarrito { get; set; }
    }
}
