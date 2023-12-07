namespace BarbieQ.Areas.Admin.Models
{
    public class AdminEditarCategoriaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public IFormFile? Imagen { get; set; } = null!;
    }
}
