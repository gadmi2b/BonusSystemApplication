namespace BonusSystemApplication.Models.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T Get(long id);
        IQueryable<T> GetQueryForAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(long id);
    }
}
