using BeGamer.Data;
using BeGamer.Services.Interfaces.common;
using BeGamer.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services.Impl.common
{

    public abstract class BaseAppService<TEntity, TDto, TCreateDto, TUpdateDto, TRepository> : IBaseAppService<TEntity, TDto, TCreateDto, TUpdateDto, TRepository> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly GuidGenerator _guidGenerator;
        protected readonly ILogger<BaseAppService<TEntity, TDto, TCreateDto, TUpdateDto, TRepository>> _logger;

        public BaseAppService(
            AppDbContext context,
            ILogger<BaseAppService<TEntity, TDto, TCreateDto, TUpdateDto, TRepository>> logger,
            GuidGenerator guidGenerator)
        {
            _context = context;
            _logger = logger;
            _guidGenerator = guidGenerator;
        }

        public abstract Task<TDto> CreateAsync(TCreateDto createDto);

        public abstract Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto);



        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete {EntityName} entity with ID: {EntityId}", typeof(TEntity).Name, id);
            try
            {
                var entityExists = await _repository.
                var entity = await _context.Set<TEntity>().FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("{EntityName} entity with ID: {EntityId} not found. Deletion aborted.", typeof(TEntity).Name, id);
                    return false;
                }
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("{EntityName} entity with ID: {EntityId} deleted successfully.", typeof(TEntity).Name, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting {EntityName} entity with ID: {EntityId}", typeof(TEntity).Name, id);
                throw;
            }
        }

        public virtual async Task<TDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching {EntityName} entity with ID: {EntityId}", typeof(TEntity).Name, id);
            try
            {
                var entity = await _context.Set<TEntity>().FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("{EntityName} entity with ID: {EntityId} not found.", typeof(TEntity).Name, id);
                    return default;
                }
                _logger.LogInformation("{EntityName} entity with ID: {EntityId} fetched successfully.", typeof(TEntity).Name, id);
                return default; // TODO: implement mapping to DTO _mapper.ToDto(entity)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching {EntityName} entity with ID: {EntityId}", typeof(TEntity).Name, id);
                throw;
            }
        }
        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all {EntityName} entities from the database.", typeof(TEntity).Name);

            try
            {
                var entities = await _context.Set<TEntity>().ToListAsync();

                _logger.LogInformation("Fetched {Count} {EntityName} entities.", entities.Count, typeof(TEntity).Name);

                if (entities == null || !entities.Any())
                    return Enumerable.Empty<TDto>();

                return new List<TDto>(); // TODO: implement mapping to DTOList _mapper.ToDtoList(entities)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching {EntityName} entities.", typeof(TEntity).Name);
                throw;
            }
        }


        protected virtual async Task<bool> ExistsById(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id) != null;
        }
    }
}
