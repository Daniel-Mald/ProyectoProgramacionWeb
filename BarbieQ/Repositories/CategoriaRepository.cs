using BarbieQ.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarbieQ.Repositories
{
    public class CategoriaRepository : Repository<Categoria>
    {
        public CategoriaRepository(Sistem21BarbieQcosmeticsContext context) : base(context)
        {
        }
        public override IEnumerable<Categoria> GetAll()
        {
            return Context.Categoria.Include(x => x.Producto).OrderBy(x => x.Nombre);
        }
        public  Categoria? GetById(int id)
        {
            return Context.Categoria.Include(x => x.Producto).Where(x => x.Id == id).FirstOrDefault();
        }
        public Categoria? GetByNombre(string id)
        {
            return Context.Categoria.Include(x => x.Producto).Where(x => x.Nombre == id).FirstOrDefault();
        }
    }
}
