using BeGamer.Data;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Repositories.common
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger<T> _logger;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context, ILogger<T> logger, DbSet<T> dbSet)
        {
            _context = context;
            _logger = logger;
            _dbSet = context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
            {
                await _dbSet.AddAsync(entity!);
                //await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity!;
            }
            
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
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

        public async Task<bool> ExistsById(Guid id)
        {
            var entity = await _context.FindAsync<T>(id);
            return entity != null;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }





        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public Task<T> GetAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

    }
}
