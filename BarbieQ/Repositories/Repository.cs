using BarbieQ.Models.Entities;

namespace BarbieQ.Repositories
{
    public class Repository<T> where T : class
    {
        //a
        public Sistem21BarbieQcosmeticsContext Context { get; }
        public Repository(Sistem21BarbieQcosmeticsContext context)
        {
            Context = context;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
        public virtual T? Get(object id)
        {
            return Context.Find<T>(id);
        }
        public virtual void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public virtual void Delete(object id)
        {
            var entity = Get(id);

            if (entity != null)
            {
                Delete(entity);
            }


        }
        public virtual void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
    }
}
