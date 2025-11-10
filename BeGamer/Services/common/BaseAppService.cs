using AutoMapper;
using BeGamer.Repositories.common;
using BeGamer.Utils;

namespace BeGamer.Services.common
{

    public abstract class BaseAppService<TEntity, TDto, TCreateDto, TUpdateDto> : IBaseAppService<TEntity, TDto, TCreateDto, TUpdateDto> where TEntity : class
    {
        protected readonly GuidGenerator _guidGenerator;
        protected readonly IMapper _mapper;
        protected readonly IGenericRepository<TEntity> _genericRepository;
        protected readonly ILogger<BaseAppService<TEntity, TDto, TCreateDto, TUpdateDto>> _logger;

        public BaseAppService(
            GuidGenerator guidGenerator,
            IMapper mapper,
            IGenericRepository<TEntity> genericRepository,
            ILogger<BaseAppService<TEntity, TDto, TCreateDto, TUpdateDto>> logger)
        {
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public abstract Task<TDto> CreateAsync(TCreateDto createDto);

        public abstract Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto);



        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete {EntityName} entity with ID: {EntityId}", typeof(TEntity).Name, id);
            try
            {
                var entity = await _genericRepository.FindByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("{EntityName} entity with ID: {EntityId} not found. Deletion aborted.", typeof(TEntity).Name, id);
                    return false;
                }
                await _genericRepository.DeleteAsync(entity);
                await _genericRepository.SaveChangesAsync();
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
                var entity = await _genericRepository.FindByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("{EntityName} entity with ID: {EntityId} not found.", typeof(TEntity).Name, id);
                    return _mapper.Map<TDto>(entity);
                }
                _logger.LogInformation("{EntityName} entity with ID: {EntityId} fetched successfully.", typeof(TEntity).Name, id);
                return _mapper.Map<TDto>(entity); 
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
                var entities = await _genericRepository.GetAllAsync();

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


        public virtual async Task<bool> ExistsById(Guid id)
        {
            return await _genericRepository.ExistsById(id);
        }
    }
}
