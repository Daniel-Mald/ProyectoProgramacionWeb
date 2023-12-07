namespace BarbieQ.Areas.Admin.Models
{
    public class AdminAgregarCategoriaViewModel
    {
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public IFormFile? Imagen { get; set; } = null!;
    }
}
