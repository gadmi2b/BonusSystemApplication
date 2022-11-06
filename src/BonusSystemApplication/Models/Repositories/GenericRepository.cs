using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.Models.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DataContext context;
        public GenericRepository(DataContext ctx) => context = ctx;

        public virtual T Get(long id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual IQueryable<T> GetQueryForAll()
        {
            return context.Set<T>()
                .AsQueryable()
                .AsNoTracking();
        }

        public virtual void Add(T entity)
        {
            context.Add<T>(entity);
            context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            context.Update<T>(entity);
            context.SaveChanges();
        }

        public virtual void Delete(long id)
        {
            //context.Remove<T>(Get(id));
            context.SaveChanges();
        }

    }
}
