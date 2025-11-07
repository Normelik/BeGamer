
using BeGamer.Data;

namespace BeGamer.Repositories.common
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger<T> _logger;

        public GenericRepository(AppDbContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(id));
            }

            var foundEntity =  await _context.Set<T>().FindAsync(id);
            if (foundEntity is null) return null;
            return foundEntity;
        }

        public Task<IReadOnlyList<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
