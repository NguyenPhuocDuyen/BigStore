namespace BigStore.DataAccess.Repository.IRepository
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T?> GetById(string id);
        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);
    }
}
