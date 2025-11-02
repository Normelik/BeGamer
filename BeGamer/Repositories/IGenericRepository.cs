namespace BeGamer.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(T entity);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
