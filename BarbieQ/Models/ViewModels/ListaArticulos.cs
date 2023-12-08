namespace BarbieQ.Models.ViewModels
{
    public class ListaArticulos
    {
        public int TotalProductos { get; set; }
        public decimal TotalPagar { get; set; }
        public List<CarritoViewModel> Carrito { get; set; } = new();
    }
}
