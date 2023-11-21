using BarbieQ.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarbieQ.Repositories
    
{
    public class ProductosRepository : Repository<Producto>
    {
        public ProductosRepository(BarbieAndCosmeticsContext context) : base(context)
        {
        }
        

        



        public IEnumerable<Producto>GetProductosByCategoria(int categoria)
        {
            return Context.Producto
                .Include(x => x.IdCategoriaNavigation)
                .Where(x => x.IdCategoriaNavigation != null &&
                x.IdCategoriaNavigation.Id
                == categoria)
                .OrderBy(x => x.Nombre);
        }

        public Producto? GetByNombre (string nombre)
        {
            return Context.Producto
                .Include(x => x.IdCategoriaNavigation)
                .FirstOrDefault(x => x.Nombre == nombre);
        }







    }

}
