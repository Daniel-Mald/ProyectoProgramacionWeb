namespace BarbieQ.Models.ViewModels
{
    public class CarritoViewModel
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; } = "";
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
